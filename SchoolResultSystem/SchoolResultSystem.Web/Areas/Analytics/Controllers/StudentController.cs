// this controller makes marsksheet per student of recent exam


using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolResultSystem.Web.Areas.Analytics.Models;
using SchoolResultSystem.Web.Areas.Analytics.Services;
using SchoolResultSystem.Web.Data;
using SchoolResultSystem.Web.Filters;
using SchoolResultSystem.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SchoolResultSystem.Web.Areas.Analytics.Controllers
{
    [Area("Analytics")]
    [AuthorizeUser]
    public class StudentController : Controller
    {
        private readonly SchoolDbContext _db;

        public StudentController(SchoolDbContext db)
        {
            _db = db;
        }

        [HttpPost]
        public async Task<IActionResult> Marksheet([FromBody] StdReq ns)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid model data.");


            string nsn = ns.NSN;

            int examId = _db.ExamList
             .Where(e => e.AcademicYear == ns.exYear && e.ExamName == ns.exName)
             .Select(e => e.ExamId)
             .FirstOrDefault(); // returns 0 if not found

            if (examId == 0)
            {
                return Json(new { message = "No Exam found" });
            }

            var Student = _db.ClassStudent.Where(s=>s.NSN==nsn && s.IsActive)
                                .Include(s=>s.Student)
                                .Select(s=> new
                                {
                                    ClassId =s.ClassId,
                                    Name =s.Student.StudentName,
                                    DOB = s.Student.D_O_B,
                                    Registration = s.Student.RegistrationN
                                }
                                )
                                .FirstOrDefault(); // iquery

            var StudentSCodes = _db.CST.Where(s=>s.ClassId==Student!.ClassId).Select(s=>s.SCode);
            var StudentSubjectMarks = _db.Marksheet
                                        .Where(s=>s.ExamId==examId && StudentSCodes.Contains(s.SCode))
                                        .Include(s=>s.Subject)
                                        .Select(m => new
                                        {
                                            
                                            SType =m.Subject.SType,
                                            LinkedPr =m.Subject.LinkedPr,
                                            SCode = m.SCode,
                                            Mark = m.Mark
                                        }).ToList();

            var ExamRubrick = _db.ExamRubrick
                                .Where(s=>s.ExamId==examId && StudentSCodes.Contains(s.SCode)).Select(s=> new
            {
                SName = s.Subs.SName,
                SCode = s.SCode,
                SType =s.Subs.SType,
                LinkedPr =s.Subs.LinkedPr,
                FullMark = s.FullMark,
                CreditHour = s.CreditHour
            }).ToList();
            
            

           

            // prepare Obtained marks model
            var MarkedSubjects = new List<ExamSubjects>();
            var ExamSubjects = new List<ExamSubjects>();

            

            foreach (var item in StudentSubjectMarks)
            {
                var subj = new ExamSubjects();
                if (item.SType == "theory")
                {
                    subj.theoryCode = item.SCode;
                    subj.theoryMark = item.Mark;
                    if (item.LinkedPr != null)
                    {
                        var practical = StudentSubjectMarks.Where(s => s.SCode == item.LinkedPr).FirstOrDefault();
                        subj.practicalCode = practical!.SCode;
                        subj.practicalMark = practical!.Mark;
                    }

                    MarkedSubjects.Add(subj);
                }

            }

            foreach (var item in ExamRubrick)
            {
                var subj = new ExamSubjects();
                if (item.SType == "theory")
                {
                    subj.theorySName=item.SName;
                    subj.theoryCode = item.SCode;
                    subj.theoryMark = item.FullMark;
                    if (item.LinkedPr != null)
                    {
                        var practical = ExamRubrick.Where(s => s.SCode == item.LinkedPr).FirstOrDefault();
                        subj.practicalSName = practical!.SName;
                        subj.practicalCode = practical!.SCode;
                        subj.practicalMark = practical!.FullMark;
                    }

                    MarkedSubjects.Add(subj);
                }

            }

             var result  = FindGrade.SubjectGrades(ExamSubjects, MarkedSubjects);

            return PartialView("Marksheet", result);
        }

    }
}
