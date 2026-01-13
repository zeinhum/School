using Microsoft.AspNetCore.Mvc;
using SchoolResultSystem.Web.Models;
using SchoolResultSystem.Web.Data;
using SchoolResultSystem.Web.Controllers;
using SchoolResultSystem.Web.Areas.Principal.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using SchoolResultSystem.Web.Filters;
using SchoolResultSystem.Web.Areas.Microservices.Models;
using SchoolResultSystem.Web.Areas.Attendence.Models;
using SchoolResultSystem.Web.Areas.Attendence.Services;


namespace SchoolResultSystem.Web.Areas.Attendence.Controllers
{


    [Area("Attendence")]
    [AuthorizeUser()]

    public class AttendenceController(SchoolDbContext db) : SchoolDbController(db)
    {
        [HttpPost]
        public IActionResult DisplpayAttendence([FromBody] AttendenceRequestDto dto)
        {
            var attendence = new DisplayAttendence(_db);
            var attendenceData = attendence.ExtractAttendenceData(dto);

            return PartialView("_DispAttendence", attendenceData);
        }
    }
}