using Microsoft.AspNetCore.Mvc;
using SchoolResultSystem.Web.Models;
using SchoolResultSystem.Web.Data;
using SchoolResultSystem.Web.Controllers;
using SchoolResultSystem.Web.Areas.Principal.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using SchoolResultSystem.Web.Filters;
using SchoolResultSystem.Web.Areas.Microservices.Models;

[Area("Microservices")]
[AuthorizeUser("Admin")]
public class MicroservicessController : SchoolDbController
{
    public MicroservicessController(SchoolDbContext db) : base(db) { }

    // return classes
    public IActionResult ClassObject()
    {
        // get all the classes
        var Classes = _db.Classes.Where(c => c.IsActive).ToList();


        return Json(new { Classes });
    }

    [HttpPost]
    public IActionResult MoveStudents([FromBody] Changeclass data)
    {
        if (!ModelState.IsValid) return Json(new{message="Incalid data"});
        if (data.FromClass == data.Toclass) return Json(new { message = "Can't move students within the class, choose a diffirent class." });

        var assignClass = new List<CSModel>();

        // Add new class assignments
        foreach (string nsn in data.MovingStudents)
        {
            assignClass.Add(new CSModel
            {
                ClassId = data.Toclass,
                NSN = nsn,
                IsActive = true  // ensure new assignments are active
            });
        }

        _db.ClassStudent.AddRange(assignClass);

        // Deactivate existing assignments

        _db.ClassStudent
            .Where(c => data.MovingStudents.Contains(c.NSN)
                        && c.ClassId == data.FromClass
                        && c.IsActive)
            .ExecuteUpdate(s => s.SetProperty(c => c.IsActive, false));

        _db.SaveChanges();


        return Json(new { success = true, message = "Class changed for selected students." });
    }

    // Deactivate Teacher
    [HttpPost]
    public IActionResult DeactivateTeacher([FromBody] Teacher id)
    {
        if (id == null) return BadRequest("Bad Request to deactivate teacher");
        _db.Users.Where(t => t.UserId == id.Id).ExecuteUpdate(s => s.SetProperty(t => t.IsActive, false));
        _db.SaveChanges();

        return Json(new { success = true, message = "Teacher deactivated" });
    }

    // Deactivate Student
    [HttpPost]
    public IActionResult DeactivateStudent([FromBody] Std id)
    {
        if (id == null) return BadRequest("Bad Request to deactivate student");
        _db.Students.Where(t => t.NSN == id.Id).ExecuteUpdate(s => s.SetProperty(t => t.IsActive, false));
        _db.SaveChanges();

        return Json(new { success = true, message = "Student  deactivated" });
    }


    [HttpPost]
    public IActionResult UserId([FromBody] Teacher id)
    {
        if (id == null) return BadRequest("Bad request");
        var TeacherInfo = _db.Users.Where(u => u.UserId == id.Id).FirstOrDefault();
        return Ok(TeacherInfo);
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePw id)
    {
        if (id == null || string.IsNullOrWhiteSpace(id.NewPw))
            return Ok(new { message = "Bad request" });

        var updatedRows = await _db.Users
            .Where(u => u.UserId == id.Id)
            .ExecuteUpdateAsync(setters =>
                setters.SetProperty(u => u.Password, id.NewPw)
            );

        if (updatedRows == 0)
            return Ok(new { message = "User not found" });

        return Ok(new { succes = true, message = "Password changed." });
    }

    [HttpPost]
    public async Task<IActionResult> RemoveSubjectTeacher([FromBody] SubjectTeacher info)
    {
        if (info == null) return Ok(new { message = "Bad request" });

        await _db.CST
                .Where(u => u.ClassId == info.ClassId
                && u.UserId == info.UserId
                && u.SCode == info.Scode)
                .ExecuteDeleteAsync();
                return Ok(new{message="Class-Subject-Teacher assignment removed."});

    }

    public  async Task<IActionResult> AllClasses()
    {
        var classes = await _db.Classes.Where(c=>c.IsActive).ToListAsync();
        if(classes != null)
        {
            return Json(new{Classes=classes});
        }
        return Json(new{messag="No class found"});
        
    }

    public async Task<IActionResult> AllTeachers()
    {
        var teacher = await _db.Users.Where(u=>u.IsActive && u.Role =="Teacher").Select(u => new
        {
            userId = u.UserId,
            teacher = u.FullName
        }).ToListAsync();
        return Json(teacher);
    }

    public async Task<IActionResult> AllSubjects()
    {
        var sub = await _db.Subjects.Where(s=>s.IsActive).ToListAsync();
        return Json(sub);
    }
}