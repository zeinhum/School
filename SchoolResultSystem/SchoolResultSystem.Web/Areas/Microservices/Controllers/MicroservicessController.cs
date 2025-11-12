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
        if (!ModelState.IsValid) return BadRequest("inccorect request");
        if (data.FromClass == data.Toclass) return Json(new { success = false, message = "Can't move students within the class, choose a diffirent class." });

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

        _db.CS.AddRange(assignClass);

        // Deactivate existing assignments

        _db.CS
            .Where(c => data.MovingStudents.Contains(c.NSN)
                        && c.ClassId == data.FromClass
                        && c.IsActive)
            .ExecuteUpdate(s => s.SetProperty(c => c.IsActive, false));

        _db.SaveChanges();


        return Json(new { success = true });
    }

    // Deactivate Teacher
    [HttpPost]
    public IActionResult DeactivateTeacher(string id)
    {
        if (id == null) return BadRequest("Bad Request to deactivate teacher");
        _db.Users.Where(t => t.TeacherId == id).ExecuteUpdate(s => s.SetProperty(t => t.IsActive, false));
        _db.SaveChanges();

        return Json(new { success = true, mesaage = "Teacher deactivated" });
    }

// Deactivate Student
 [HttpPost]
    public IActionResult DeactivateStudent(string id)
    {
        if (id == null) return BadRequest("Bad Request to deactivate teacher");
        _db.Students.Where(t => t.NSN == id).ExecuteUpdate(s => s.SetProperty(t => t.IsActive, false));
        _db.SaveChanges();

        return Json(new { success = true, mesaage = "Teacher deactivated" });
    }
}