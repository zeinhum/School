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
public class AttendenceController : SchoolDbController
{
    public AttendenceController(SchoolDbContext db) : base(db) { }

    public IActionResult TeacherAttendence(string id)
    {
        return Json(new { success = true });
    }
}