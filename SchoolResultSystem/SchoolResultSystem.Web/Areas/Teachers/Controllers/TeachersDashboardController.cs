using Microsoft.AspNetCore.Mvc;
using SchoolResultSystem.Web.Models;
using SchoolResultSystem.Web.Areas.Teachers.Models;
using SchoolResultSystem.Web.Data;
using System.Linq.Expressions;
using SchoolResultSystem.Web.Filters;

namespace SchoolResultSystem.Web.Areas.Teacher.Controllers
{
    [AuthorizeUser("Teacher")]
    [Area("Teachers")]
    public class TeachersDashboardController : Controller
    {
        private readonly SchoolDbContext _db;

        public TeachersDashboardController(SchoolDbContext db)
        {
            _db = db;
        }



        public IActionResult Index(string id)
        {
            try
            {
                var dto = new TDashDto
                {
                    SchoolName = _db.SchoolInfo.FirstOrDefault()?.Name ?? "Unknown School",
                    TeacherName = _db.Users
                                    .Where(u => u.TeacherId == id)
                                    .Select(u => u.TeacherName)
                                    .FirstOrDefault() ?? "Unknown Teacher",
                    CSList = _db.CST
                                .Where(t => t.TeacherId == id)
                                .Select(c => new CSList
                                {
                                    ClassId = c.ClassId,
                                    ClassName = c.Class.ClassName,
                                    SCode = c.SCode,
                                    SName = c.Subject.SName
                                })
                                .ToList()
                };

                return View("TIndex",dto);
            }
            catch (Exception e)
            {
                ViewBag.error = e;
                return View("err");
            }
            ;



        }

        [HttpPost]
        public IActionResult FillMarks([FromBody] ClsSub data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data type");
            }
            try
            {

                if (data == null)
                    return BadRequest("Invalid data");
                var exam = _db.Exams
                            .Select(e => new Exams { ExamId = e.ExamId, ExamName = e.ExamName, AcademicYear = e.AcademicYear })
                            .Distinct()
                            .ToList();


                // Get all students for the given class
                var students = _db.CS
                    .Where(st => st.ClassId == data.ClassId)
                    .Select(st => new CS
                    {
                        NSN = st.NSN,
                        StudentName = st.Student.StudentName
                    })
                    .ToList();

                // Construct FillMarks object
                var result = new FillMarks
                {
                    Info = new ClsSub
                    {
                        ClassName = data.ClassName,
                        ClassId = data.ClassId,
                        SCode = data.SCode,
                        SName = data.SName
                    },
                    Students = students,
                    exams = exam
                };

                return PartialView("_FillMPartial", result);
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e });
            }
            ;

        }

        // update Marks
        [HttpPost]
        public IActionResult UpdateMarks([FromBody] ClsSub data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data type");
            }
            try
            {

                if (data == null)
                    return BadRequest("Invalid data");
                var exam = _db.Exams
                            .Select(e => new Exams { ExamId = e.ExamId, ExamName = e.ExamName, AcademicYear = e.AcademicYear })
                            .Distinct()
                            .ToList();
                var result = new UpdateMarks
                {
                    Info = new ClsSub
                    {
                        ClassName = data.ClassName,
                        ClassId = data.ClassId,
                        SCode = data.SCode,
                        SName = data.SName
                    },
                    exams = exam
                };

                return PartialView("_UpdateMPartial", result);
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e });
            }
           ;

        }

        // save Marks
        [HttpPost]
        public IActionResult SaveMarks([FromBody] SaveMarks data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("invalid Data type");
            }

            try
            {
                var payload = new List<MarksheetModel>();
                // 1️⃣ Get NSNs that are already filled in DB for this exam + subject
                var filledNsns = _db.Marksheet
                    .Where(m => m.ExamId == data.ExamId && m.SCode == data.SCode)
                    .Select(m => m.NSN) // *get b supplied NSN
                    .ToList();

                // 2️⃣ Filter students in data.Marks whose NSNs are NOT in filledNsns
                var unfilledMarks = data.Marks
                    .Where(m => !filledNsns.Contains(m.NSN))
                    .ToList();

                foreach (var item in unfilledMarks)
                {
                    var payloadItem = new MarksheetModel
                    {
                        ExamId = data.ExamId,
                        SCode = data.SCode,
                        NSN = item.NSN,
                        ThMark = item.ThMark,
                        PrMark = item.PrMark
                    };
                    payload.Add(payloadItem);
                }
                _db.Marksheet.AddRange(payload);
                _db.SaveChanges();
                var allSupliedNsn = data.Marks.Select(n => n.NSN).ToList();
                var Skipped = allSupliedNsn.Count - unfilledMarks.Count;
                return Json(new { success = true, message = $"Saved items: {unfilledMarks.Count}, Skipped duplicate items: {Skipped}" });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "server error occured" });
            }

        }

        // save update
        [HttpPost]
        public IActionResult SaveUpdate([FromBody] SaveMarks data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("invalid Data type");
            }
            try
            {
                //  Get NSNs sent from frontend
                var nsns = data.Marks.Select(m => m.NSN).ToList();

                // Fetch only existing marks for given ExamId, SCode, and NSNs
                var existingMarks = _db.Marksheet
                    .Where(m => m.ExamId == data.ExamId
                             && m.SCode == data.SCode
                             && nsns.Contains(m.NSN))
                    .ToList();

                //  Update only existing marks
                foreach (var item in data.Marks)
                {
                    var existing = existingMarks.FirstOrDefault(m => m.NSN == item.NSN);
                    if (existing != null)
                    {
                        existing.ThMark = item.ThMark;
                        existing.PrMark = item.PrMark;
                        _db.Marksheet.Update(existing); // optional, EF tracks changes automatically
                    }
                }

                // Save changes
                _db.SaveChanges();

                return Json(new { success = true, message = $"Updated items: {existingMarks.Count}" });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "server error occured" });
            }

        }
    }

}
