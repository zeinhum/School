using Microsoft.AspNetCore.Mvc;
using SchoolResultSystem.Web.Models;
using SchoolResultSystem.Web.Data;
using SchoolResultSystem.Web.Controllers;
using SchoolResultSystem.Web.Areas.Principal.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using SchoolResultSystem.Web.Filters;



[Area("Principal")]
[AuthorizeUser("Admin")]
public class PrincipalDashboardController : SchoolDbController
{
    public PrincipalDashboardController(SchoolDbContext db) : base(db) { }

    // main dashboard for admin view
    public IActionResult Index()
    {
        // get schoolinfo
        var school = _db.SchoolInfo.FirstOrDefault()?.Name;
        
        return View("PIndex", school);
    }
    // Class Subject teacher View
    public IActionResult CSTView(int id)
    {
        try
        {
            var classInfo = _db.Classes.FirstOrDefault(c => c.ClassId == id);
            if (classInfo == null)
            {
                return NotFound();
            }

            var subjectTeachers = _db.CST
                .Where(cst => cst.ClassId == id)
                .Select(cst => new CSTDto
                {
                    SCode = cst.SCode,
                    SubjectName = cst.Subject.SName,
                    TeacherId = cst.TeacherId,
                    TeacherName = cst.User.TeacherName
                })
                .ToList();

            var data = new CSTView
            {
                ClassId = classInfo.ClassId,
                ClassName = classInfo.ClassName,
                SubjectTeachers = subjectTeachers
            };

            return View(data);
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal server error");
        }

    }

    [HttpPost]
    public IActionResult AddSubjectTeacher([FromBody] AddCSTDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(new { status = "error", message = "Invalid data." });

        // Validate subject exists
        var subjectExists = _db.Subjects.Any(s => s.SCode == model.SCode);
        var teacherExists = _db.Users.Any(u => u.TeacherId == model.TeacherId && u.Role == "Teacher");

        if (!subjectExists || !teacherExists)
        {
            return BadRequest(new { status = "error", message = "Invalid Subject Code or Teacher ID." });
        }

        // Check if assignment already exists
        bool alreadyAssigned = _db.CST.Any(cst =>
            cst.ClassId == model.ClassId &&
            cst.SCode == model.SCode &&
            cst.TeacherId == model.TeacherId);

        if (alreadyAssigned)
        {
            return Conflict(new { status = "warning", message = "This Subject-Teacher is already assigned to this class." });
        }

        // Create new CST entry
        var newCST = new CSTModel
        {
            ClassId = model.ClassId,
            SCode = model.SCode,
            TeacherId = model.TeacherId
        };

        _db.CST.Add(newCST);
        _db.SaveChanges();

        return Ok(new { status = "success", message = "Subject-Teacher assigned successfully!" });
    }


    // Class Student 
    public IActionResult CSview(int id)
    {
        var classInfo = _db.Classes.FirstOrDefault(c => c.ClassId == id);
        if (classInfo == null)
        {
            return BadRequest("Invalid Class ID");
        }
        var students = _db.CS
            .Where(cs => cs.ClassId == id &&cs.IsActive)
            .Include(cs => cs.Student)
            .Select(cs => new CSDto
            {
                NSN = cs.NSN,
                StudentName = cs.Student.StudentName
            })
            .ToList();

        var data = new CSViewModel
        {
            ClassId = classInfo.ClassId,
            ClassName = classInfo.ClassName,
            Students = students
        };

        return View(data);
    }

    // all teachers and subjects
    [HttpGet]
    public IActionResult GetTeachersAndClasses()
    {
        var teachers = _db.Users
            .Where(u => u.Role == "Teacher")
            .Select(t => new { t.TeacherId, t.TeacherName })
            .ToList();

        var subjects = _db.Subjects
            .Select(s => new { s.SCode, s.SName })
            .ToList();

        return Json(new { teachers, subjects });
    }


    // Student Admission 
    public IActionResult SDForm()
    {
        var clsses = _db.Classes.Where(c => c.IsActive).ToList();
        return View(clsses);
    }

    // admission
    [HttpPost]
    public IActionResult Admission([FromBody] AdmiForm form)
    {
        if (!ModelState.IsValid)
            return BadRequest(new { status = "error", message = "Invalid data." });

        try
        {
            var oldNSNs = _db.Students.Select(s => s.NSN).ToList();
            var formNSNs = form.AdmissionForm.Select(s => s.NSN).ToList();

            foreach (var st in oldNSNs)
            {
                Console.WriteLine($"admited astudents NSN : {st}");
            }

            // Find new NSNs that are not already in database
            var newNSNs = formNSNs.Where(nsn => !oldNSNs.Contains(nsn)).ToList();

            // Optionally, find duplicates too
            var duplicateNSNs = formNSNs.Where(nsn => oldNSNs.Contains(nsn)).ToList();

            var newStudents = new List<StudentModel>();
            var newClassLinks = new List<CSModel>();

            foreach (var student in form.AdmissionForm.Where(s => newNSNs.Contains(s.NSN)))
            {
                newStudents.Add(new StudentModel
                {
                    NSN = student.NSN,
                    StudentName = student.StudentName,
                    D_O_B = student.D_O_B,
                    Address = student.Address,
                    AdmissionDate = student.AdmissionDate,
                    IsActive = true
                });

                newClassLinks.Add(new CSModel
                {
                    NSN = student.NSN,
                    ClassId = student.ClassId
                });
            }

            _db.Students.AddRange(newStudents);
            _db.CS.AddRange(newClassLinks);
            _db.SaveChanges();

            return Json(new
            {
                success = true,
                added = newNSNs.Count,
                duplicates = duplicateNSNs
            });
        }
        catch (Exception e)
        {
            return Json(new
            {
                success = false,
                info = e
            });
        }
    }

    // Anlytics partial
public IActionResult Analytics()
    {
        return PartialView("_Analytics");
    }

    public IActionResult Teachers()
    {
        var user = _db.Users.Where(u => u.Role == "Teacher").ToList();
        return PartialView("_Teachers", user);
    }
    public IActionResult Classes()
    {
        var classes = _db.Classes.Where(c => c.IsActive).ToList();
        return PartialView("_Classes", classes);
    }
}
