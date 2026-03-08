using Microsoft.AspNetCore.Mvc;
using SchoolResultSystem.Web.Models;
using SchoolResultSystem.Web.Data;
using SchoolResultSystem.Web.Controllers;
using SchoolResultSystem.Web.Areas.Principal.Models;
using Microsoft.EntityFrameworkCore;
using SchoolResultSystem.Web.Filters;
using Microsoft.Data.Sqlite;



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
                    UserId = cst.UserId,
                    FullName = cst.User.FullName
                })
                .ToList();

            var data = new CSTView
            {
                ClassId = classInfo.ClassId,
                ClassName = classInfo.ClassName,
                SubjectTeachers = subjectTeachers
            };

            return PartialView(data);
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal server error");
        }

    }

    [HttpPost]
    public async Task<IActionResult> AddSubjectTeacher([FromBody] List<CSTModel> model)
    {
        if (model == null || !model.Any()) return Ok(new { message = "invalid data" });

        // 1. Build a list of parameters to avoid SQL Injection
        var parameters = new List<object>();
        var valueClauses = new List<string>();

        for (int i = 0; i < model.Count; i++)
        {
            // Parameter names
            var pClass = $"@c{i}";
            var pSCode = $"@s{i}";
            var pUser = $"@u{i}";

            // SQLite syntax for selecting a row of constants
            valueClauses.Add($"SELECT {pClass} AS ClassId, {pSCode} AS SCode, {pUser} AS UserId");

            parameters.Add(new SqliteParameter(pClass, model[i].ClassId));
            parameters.Add(new SqliteParameter(pSCode, model[i].SCode));
            parameters.Add(new SqliteParameter(pUser, model[i].UserId));
        }

        // 2. The Atomic "Upsert" Query
        // This inserts data only if the triple-key doesn't already exist
        string virtualTable = string.Join(" UNION ALL ", valueClauses);
        string sql = $@"
        INSERT INTO CST (ClassId, SCode, UserId)
        SELECT tmp.ClassId, tmp.SCode, tmp.UserId
        FROM ({virtualTable}) AS tmp
        WHERE NOT EXISTS (
            SELECT 1 FROM CST 
            WHERE CST.ClassId = tmp.ClassId 
              AND CST.SCode = tmp.SCode 
              AND CST.UserId = tmp.UserId
        );";

        try
        {
            // ExecuteSqlRawAsync ensures one round-trip to the DB
            var rowsAffected = await _db.Database.ExecuteSqlRawAsync(sql, parameters.ToArray());

            return Ok(new
            {
                message = $"{rowsAffected} new records added, {model.Count - rowsAffected} duplicates ignored."
            });
        }
        catch (Exception ex)
        {
            // Log the exception (ex) here
            return Ok(new { message = "Database sync failed.", error = ex.Message });
        }
    }


    // Class Student 
    public IActionResult CSview(int id)
    {
        var classInfo = _db.Classes.FirstOrDefault(c => c.ClassId == id);
        if (classInfo == null)
        {
            return BadRequest("Invalid Class ID");
        }
        var students = _db.ClassStudent
            .Where(cs => cs.ClassId == id && cs.IsActive)
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

        return PartialView(data);
    }

    // all teachers and subjects
    [HttpGet]
    public IActionResult GetTeachersAndClasses()
    {
        var teachers = _db.Users
            .Where(u => u.Role == "Teacher" && u.IsActive)
            .Select(t => new { t.UserId, t.FullName })
            .ToList();

        var subjects = _db.Subjects
            .Where(s => s.IsActive)
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
            return Json(new { message = "Invalid data" });

        try
        {
            var oldNSNs = _db.Students.Select(s => s.NSN).ToList();
            var formNSNs = form.AdmissionForm.Select(s => s.NSN).ToList();

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
                    RegistrationN = student.RegistrationN,
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
            _db.ClassStudent.AddRange(newClassLinks);
            _db.SaveChanges();

            return Json(new
            {
                message = $"Admitted: {newNSNs.Count}, duplicate: {duplicateNSNs}"

            });
        }
        catch (Exception)
        {
            return Json(new
            {
                message = "Some error occured"
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
        var user = _db.Users.Where(u => u.Role == "Teacher" && u.IsActive).ToList();
        return PartialView("_Teachers", user);
    }
    public IActionResult Classes()
    {
        var classes = _db.Classes.Where(c => c.IsActive).ToList();
        return PartialView("_Classes", classes);
    }

    // subjects
    public IActionResult Subjects()
    {
        var subjects = _db.Subjects.Where(s => s.IsActive).ToList();
        return PartialView("Subjects", subjects);
    }

    //exams
    public IActionResult Exams()
    {
        var exams = _db.ExamList.Where(e => e.IsActive).ToList();
        return PartialView("Exams", exams);
    }
}
