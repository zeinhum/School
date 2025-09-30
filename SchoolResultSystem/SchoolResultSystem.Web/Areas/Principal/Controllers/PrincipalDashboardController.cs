using Microsoft.AspNetCore.Mvc;
using SchoolResultSystem.Web.Models;
using SchoolResultSystem.Web.Data;
using SchoolResultSystem.Web.Controllers;
using SchoolResultSystem.Web.Areas.Principal.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;


[Area("Principal")]
public class PrincipalDashboardController : SchoolDbController
{
    public PrincipalDashboardController(SchoolDbContext db) : base(db) { }


    public IActionResult Index()
    {
        // get schoolinfo
        var school = _db.SchoolInfo.FirstOrDefault()?.Name;
        var teachers = _db.Users.Where(u => u.Role == "Teacher").ToList();
        var classes = _db.Classes.ToList();

        var viewModel = new PrincipalDashboardModel
        {
            SchoolName = school,
            Teachers = teachers,
            Classes = classes
        };
        return View(viewModel);
    }



    // Create Class
    public IActionResult CreateClass()
    {
        return View("CreateClass");
    }

    public IActionResult SaveClass(ClassModel model)
    {
        try
        {
            bool className = _db.Classes.Any(u => u.ClassName == model.ClassName);
            if (className)
            {
                ViewBag.error = "This class already exists";
                return View("CreateClass");
            }
            _db.Classes.Add(model);
            _db.SaveChanges();
            ViewBag.success = "Class Created";
            return View("CreateClass");
        }
        catch (Exception)
        {
            ViewBag.error = "Class not created";
            return View("CreateClass");
        }
    }

    // create subject
    public IActionResult CreateSubject()
    {
        return View("CreateSubject");
    }

    [HttpPost]
    public IActionResult SaveSubject(SubjectModel model)
    {

        try
        {
            bool subjectExists = _db.Subjects.Any(u => u.SCode == model.SCode);
            if (subjectExists)
            {
                ViewBag.error = "This subject already exists";
                return View("CreateSubject");
            }
            _db.Subjects.Add(model);
            _db.SaveChanges();
            ViewBag.success = "Subject saved.";
            return View("CreateSubject");
        }
        catch (Exception)
        {
            ViewBag.error = "Subject not saved";
            return View("CreateSubject");
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
    var subjects = await _db.Subjects.Where(s=>s.IsActive)
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


}
