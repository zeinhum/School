using Microsoft.AspNetCore.Mvc;
using SchoolResultSystem.Web.Models;
using SchoolResultSystem.Web.Data;
using Microsoft.EntityFrameworkCore;
using SchoolResultSystem.Web.Areas.Principal.Models;
using System.Security.Cryptography.X509Certificates;
using SchoolResultSystem.Web.Filters;
using Microsoft.Data.Sqlite;


namespace SchoolResultSystem.Web.Areas.Principal.Controllers
{

    [Area("Principal")]
    [AuthorizeUser("Admin")]
    public class LeaveCallenderController : Controller
    {
        private readonly SchoolDbContext _db;
        public LeaveCallenderController(SchoolDbContext db)
        {
            _db = db;
        }


        public IActionResult DisplayLeaveCallenderForm()
        {

            return View("DisplayForm");
        }


// save leaves
        [HttpPost]
        public IActionResult SetCalandar([FromBody] CalendarDto dto)
        {
            if (dto == null)
            {
                return Json(new { success = false, message = "No data provided." });
            }

            List<LeavesCalendarModel> payload = new();


            // Handle Weekdays

            if (dto.Weekdays != null && dto.Weekdays.Count != 0)
            {
                // Unique weekday types (Mon, Tue, etc.)
                var selectedWeekdays = dto.Weekdays
                    .Select(d => d.Date.DayOfWeek)
                    .Distinct()
                    .ToList();

                int year = dto.Weekdays.First().Date.Year;
                DateTime start = new(year, 1, 1);
                DateTime end = new(year, 12, 31);

                foreach (var day in selectedWeekdays)
                {
                    // Find first match in the year
                    DateTime first = start;
                    while (first.DayOfWeek != day)
                        first = first.AddDays(1);

                    // Jump by 7 days (repeat weekday)
                    for (var date = first; date <= end; date = date.AddDays(7))
                    {
                        payload.Add(new LeavesCalendarModel
                        {
                            Date = date,
                            Weekday = true,
                            Holiday = false,
                            Description = null!
                        });
                    }
                }
            }


            // Handle Holidays

            if (dto.Leaves != null)
            {
                foreach (var leave in dto.Leaves)
                {
                    payload.Add(new LeavesCalendarModel
                    {
                        Date = leave.Leave,
                        Description = leave.Description,
                        Weekday = false,
                        Holiday = true
                    });
                }
            }


            // Remove Duplicate Entries

            payload = payload
             .GroupBy(x => x.Date.Date)
                .Select(g => g.First())
                .ToList();

            // ineficient, why to load all data?
            var existingDates = _db.Leaves
                .Select(x => x.Date.Date)
                .ToHashSet(); 

            var finalToInsert = payload
                .Where(x => !existingDates.Contains(x.Date.Date))
                .ToList();

            if (finalToInsert.Count > 0)
            {
                _db.Leaves.AddRange(finalToInsert);
                _db.SaveChanges();
            }

            return Json(new { success = true, count = payload.Count });
        }

    }
}