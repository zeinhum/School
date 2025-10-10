using Microsoft.AspNetCore.Mvc;
using SchoolResultSystem.Web.Models;
using SchoolResultSystem.Web.Data;
using Microsoft.EntityFrameworkCore;
using SchoolResultSystem.Web.Areas.Principal.Models;
using System.Security.Cryptography.X509Certificates;


[Area("Principal")]
public class SetupPrincipal : Controller
{
    private readonly SchoolDbContext _db;

    public SetupPrincipal(SchoolDbContext db)
    {
        _db = db;
    }

    // all views

    public IActionResult SetupSchool()
    {
        return View();
    }


    [HttpGet] // principal set up view
    public IActionResult SetupPrincipalView()
    {
        return View();
    }


    // Create Class
    public IActionResult CreateClass()
    {
        return View("CreateClass");
    }



    // create subject
    public IActionResult CreateSubject()
    {
        return View("CreateSubject");
    }





    [HttpPost]
    public IActionResult SavePrincipal(UserModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("SetupPrincipalView");
        }
        try
        {
            // remove old admin if exists
            var oldAdmins = _db.Users.Where(u => u.Role == "Admin").ToList();
            _db.Users.RemoveRange(oldAdmins);

            // enforce admin role
            model.Role = "Admin";
            _db.Users.Add(model);

            _db.SaveChanges();

            TempData["success"] = "Principal account set successfully!";
            return RedirectToAction("SetupSchool");
        }
        catch (Exception)
        {
            ViewBag.error = "Some error occured. Try again.";
            return View("SetupPrincipalView");
        }
    }

    [HttpPost]
    public IActionResult SaveSchool(SchoolInfoModel model)
    {
        try
        {
            // optional: clear existing school info
            _db.SchoolInfo.RemoveRange(_db.SchoolInfo);
            _db.SchoolInfo.Add(model);
            _db.SaveChanges();

            TempData["success"] = "School info saved!";
            return RedirectToAction("Index","PrincipalDashboard");
        }
        catch (Exception)
        {
            ViewBag.error = "Error occured. Try again.";
            return View("SetupSchoolView");
        }
    }


    [HttpPost]
    public IActionResult SaveClass([FromBody] List<ClassModel> data)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "No classes provided." });
            }

            var existingNames = _db.Classes
                .Select(c => c.ClassName)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var newClasses = new List<ClassModel>();
            var skipped = new List<string>();

            foreach (var cl in data)
            {
                if (existingNames.Contains(cl.ClassName))
                {
                    skipped.Add(cl.ClassName);
                }
                else
                {
                    newClasses.Add(new ClassModel { ClassName = cl.ClassName });
                }
            }

            if (newClasses.Any())
            {
                _db.Classes.AddRange(newClasses);
                _db.SaveChanges();
            }

            return Json(new
            {
                success = true,
                added = newClasses.Count,
                message = $"{newClasses.Count} classes added. Skipped: {skipped.Count}"
            });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "Error creating classes: " + ex.Message });
        }
    }




    [HttpPost]
    public IActionResult SaveSubject([FromBody] List<SubjectModel> data)
    {
        try
        {
            if (data == null || data.Count == 0)
            {
                return Json(new { success = false, message = "No subjects provided." });
            }

            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Invalid data format." });
            }

            // 1️⃣ Get all existing subject codes from the database
            var existingCodes = _db.Subjects
                .Select(s => s.SCode)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var newSubjects = new List<SubjectModel>();
            var skipped = new List<string>();

            // 2️⃣ Separate new ones from existing
            foreach (var sub in data)
            {
                if (string.IsNullOrWhiteSpace(sub.SCode) || string.IsNullOrWhiteSpace(sub.SName))
                    continue; // skip invalid rows

                if (existingCodes.Contains(sub.SCode))
                {
                    skipped.Add(sub.SCode);
                }
                else
                {
                    newSubjects.Add(new SubjectModel
                    {
                        SCode = sub.SCode.Trim(),
                        SName = sub.SName.Trim(),
                    });
                }
            }

            if (newSubjects.Any())
            {
                _db.Subjects.AddRange(newSubjects);
                _db.SaveChanges();
            }

            return Json(new
            {
                success = true,
                added = newSubjects.Count,
                message = $"{newSubjects.Count} subject(s) added. Skipped {skipped.Count} duplicate(s)."
            });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "Error saving subjects: " + ex.Message });
        }
    }



    // Create teacher's account
    public IActionResult CreateTeacher()
    {
        return View("CreateTeacher");
    }

    [HttpPost]
    public IActionResult SaveTeacher(UserModel model)
    {
        try
        {

            bool userExists = _db.Users.Any(u => u.UserName == model.UserName);
            if (userExists)
            {
                ViewBag.error = "Teacher already exists. Try a different one.";
                return View("CreateTeacher");
            }
            model.Role = "Teacher";
            model.IsActive = true;
            _db.Users.Add(model);
            _db.SaveChanges();
            ViewBag.success = "Account created";
            return View("CreateTeacher");
        }
        catch (Exception)
        {
            ViewBag.error = "Account not created. Try again.";
            return View("CreateTeacher");
        }
    }

    [HttpDelete]
    public IActionResult DeleteTeacher(string id)
    {
        try
        {
            var teacherToDelete = _db.Users.FirstOrDefault(u => u.TeacherId == id);

            if (teacherToDelete == null)
            {
                return NotFound(new { success = false, message = $"Teacher with ID {id} not found." });
            }
            // Soft delete: mark inactive
            teacherToDelete.IsActive = false;

            _db.Users.Update(teacherToDelete);
            _db.SaveChanges();

            return Ok(new { success = true, message = "Teacher deactivated successfully." });
        }
        catch (Exception)
        {
            return StatusCode(500, new { success = false, message = "An error occurred during deletion." });
        }
    }

    // view classwise students
    public async Task<IActionResult> ClassStudentView(int classId)
    {
        if (classId < 0)
        {
            return BadRequest("Class ID is required.");
        }

        try
        {
            // Use async methods for database access
            var students = await _db.CS.Include(cs => cs.Student)
                                    .Where(cs => cs.ClassId == classId)
                                    .ToListAsync();

            var className = await _db.Classes
                                     .Where(c => c.ClassId == classId)
                                     .Select(c => c.ClassName)
                                     .FirstOrDefaultAsync();

            ViewBag.ClassName = className;

            return View("ClassStudentView", students);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while fetching student data.");
        }
    }


    // class Subject view
    public async Task<IActionResult> ClassSubjectView(int classId)
    {
        // 1. Get Class Name 
        var className = await _db.Classes
            .Where(c => c.ClassId == classId)
            .Select(c => c.ClassName)
            .FirstOrDefaultAsync();

        if (className == null)
            return NotFound("Class not found.");
        // 2. Get Subjects and their assigned Teachers for the class
        var classSubjectTeacherData = await _db.CST
            .Include(cst => cst.Subject)
            .Include(cst => cst.User)
            .Where(cst => cst.ClassId == classId)
            .ToListAsync();


        var distinctSubjects = classSubjectTeacherData
            .Select(cst => cst.Subject)
            .DistinctBy(s => s.SCode) // Use DistinctBy (from System.Linq) to get unique subjects
            .ToList();

        // Prepare the dictionary for Subject Teachers (equivalent to your original ViewBag.SubjectTeachers)
        ViewBag.SubjectTeachers = classSubjectTeacherData
            .GroupBy(cst => cst.SCode)
            .ToDictionary(
                g => g.Key,
                g => g.Select(t => t.User).ToList()
            );

        // 4. Pass data to the View
        ViewBag.ClassId = classId;
        ViewBag.ClassName = className;

        // Pass the distinct list of subjects to the View
        return View("ClassSubjectView", distinctSubjects);
    }


    // GET: ClassSubject/AddSubjectTeacher/5
    public async Task<IActionResult> AddSubjectTeacher(int classId)
    {
        // Get all subjects in this class
        var subjects = await _db.Subjects.Where(s => s.IsActive)
                                .ToListAsync();

        // Get all active teachers
        var teachers = await _db.Users
                                .Where(u => u.IsActive)
                                .ToListAsync();

        ViewBag.ClassId = classId;
        ViewBag.Subjects = subjects;
        ViewBag.Teachers = teachers;

        return View("AddSubjectTeacher");
    }


    // create exams
    public IActionResult CreateExam()
    {
        var allSubjects = _db.Subjects.Where(a => a.IsActive).ToList();
        return View(allSubjects);
    }
    //save Exam
    [HttpPost]
    public IActionResult SaveExam([FromBody] SaveExam data)
    {
        if (!ModelState.IsValid || data == null) {
            return Json(new { success = false, message = "Invalid data type" });
        } 
        if( data.SubjectMarks.Count == 0)
        {
            return Json(new { success = false, message = "datacount is zero" });
        }

        try
        {
            // Get existing exams from the DB
            var oldExams = _db.Exams.ToList();

            int addedCount = 0;
            int skippedCount = 0;

            var newExams = new List<ExamModel>();

            foreach (var ex in data.SubjectMarks)
            {
                // Check if this exam+subject+year already exists
                bool exists = oldExams.Any(e =>
                    e.ExamName == data.ExamName &&
                    e.AcademicYear == data.AcademicYear &&
                    e.SCode == ex.SCode
                );

                if (exists)
                {
                    skippedCount++;
                    continue; // skip duplicates
                }

                // Create new ExamModel
                var examModel = new ExamModel
                {
                    ExamName = data.ExamName,
                    AcademicYear = data.AcademicYear,
                    SCode = ex.SCode,
                    ThMark = ex.ThMark,
                    PrMark = ex.PrMark,
                    ThCrh = ex.ThCrh,
                    PrCrh = ex.PrCrh
                };

                newExams.Add(examModel);
                addedCount++;
            }

            if (newExams.Count > 0)
            {
                _db.Exams.AddRange(newExams);
                _db.SaveChanges();
            }

            return Json(new
            {
                success = true,
                message = $"Exams created for {addedCount} subjects, skipped {skippedCount} duplicates."
            });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = $"Error saving exams: {ex.Message}" });
        }
    }

}
