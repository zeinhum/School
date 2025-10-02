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
    // CST View
    public IActionResult CSTView(int id)
    { try
        {
            var classInfo = _db.Classes.FirstOrDefault(c => c.ClassId == id);
        if (classInfo == null)
        {
            return NotFound();
        }

        var subjectTeachers = _db.CST
            .Where(cst => cst.ClassId == id)
            .Select(cst=> new CSTDto
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
        catch (Exception )
        {
            return StatusCode(500, "Internal server error");
        }
        
    }

}
