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
        public async Task<IActionResult> SetCalendar([FromBody] CalendarDto dto)
        {
            
            try{
            var payload = new List<LeavesCalendarModel>();

            // Current year
            int currentYear = DateTime.Now.Year;
            DateTime start = new DateTime(currentYear, 1, 1);
            DateTime end = new DateTime(currentYear, 12, 31);

            // ---------------------------
            // Handle Weekdays (recurring)
            // ---------------------------
            
            if (dto.Weekdays != null && dto.Weekdays.Count > 0)
            {
                var selectedWeekdays = dto.Weekdays
                    .Select(d => d.Date.DayOfWeek)
                    .Distinct()
                    .ToList();
                    


                foreach (var dayOfWeek in selectedWeekdays)
                {
                    // Find first occurrence of that weekday in the year
                    DateTime first = start;
                    while (first.DayOfWeek != dayOfWeek)
                        first = first.AddDays(1);

                    // Add all recurring weekdays (every 7 days) until end of year
                    for (DateTime date = first; date <= end; date = date.AddDays(7))
                    {
                        payload.Add(new LeavesCalendarModel
                        {
                            Date = date.Date,
                            Weekday = true,
                            Holiday = false,
                            Description = string.Empty
                        });
                    }
                }
            }

            // ---------------------------
            // Handle Holidays (specific dates)
            // ---------------------------
            if (dto.Leaves != null && dto.Leaves.Count > 0)
            {
                foreach (var leave in dto.Leaves)
                {
                    payload.Add(new LeavesCalendarModel
                    {
                        Date = leave.Leave.Date,
                        Description = leave.Description ?? string.Empty,
                        Weekday = false,
                        Holiday = true
                    });
                }
            }

            // ---------------------------
            // Remove duplicates by date
            // If both weekday and holiday exist, keep holiday
            // ---------------------------
            payload = payload
                .GroupBy(x => x.Date)
                .Select(g =>
                {
                    // If any holiday exists for the date, prefer it
                    var holidayEntry = g.FirstOrDefault(x => x.Holiday);
                    return holidayEntry ?? g.First();
                })
                .ToList();

            // ---------------------------
            // Get existing dates from DB for current year
            // ---------------------------
            var existingDates = await _db.Leaves
                .Where(x => x.Date.Year == currentYear)
                .Select(x => x.Date.Date)
                .ToHashSetAsync();

            // ---------------------------
            // Filter out already existing dates
            // ---------------------------
            var finalToInsert = payload
                .Where(x => !existingDates.Contains(x.Date))
                .ToList();

            // ---------------------------
            // Insert new leaves
            // ---------------------------
            if (finalToInsert.Count > 0)
            {
                await _db.Leaves.AddRangeAsync(finalToInsert);
                await _db.SaveChangesAsync();
                }
                else
                {
                    return Ok(new{message="No new entries were made."});
                }

            // ---------------------------
            // Return JSON response
            // ---------------------------
            return Json(new
            {
                success = true,
                totalProcessed = payload.Count,
                count = finalToInsert.Count,
                message = "Leaves set successfully."
            });
            }catch (Exception)
            {
                return Ok(new{message="some error occured"});
            }
        }
    
    public async Task<IActionResult> CleanCalendar()
        {try{
            var currentYear = DateTime.Now.Year;
            await _db.Leaves.Where(y=>y.Date.Year==currentYear).ExecuteDeleteAsync();
            return Ok(new{message="All entries of weekends and leaves for current year have been deleted."});
            }
            catch (Exception)
            {
                return Ok(new{message="some error occured."});
            }
        }
    }

}