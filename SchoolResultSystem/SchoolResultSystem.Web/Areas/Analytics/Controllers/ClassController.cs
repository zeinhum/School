// generate exam report of class of recent exam

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolResultSystem.Web.Areas.Analytics.Models;
using SchoolResultSystem.Web.Areas.Analytics.Services;
using SchoolResultSystem.Web.Data;
using SchoolResultSystem.Web.Filters;
using SchoolResultSystem.Web.Models;
using System.Linq;

namespace SchoolResultSystem.Web.Areas.Analytics.Controllers
{
    [Area("Analytics")]
    [AuthorizeUser]
    public class ClassController(SchoolDbContext db) : Controller
    {
        private readonly SchoolDbContext _db = db;

        [HttpPost]
        public async Task<IActionResult> ExamReport([FromBody] StdReq req)
        {
            try{
            var className = req.NSN;
            var examId = await _db.ExamList
            .Where(e=>e.ExamName==req.exName && e.AcademicYear==req.exYear)
            .Select(e=>e.ExamId)
            .FirstOrDefaultAsync();

            if (examId == 0)
            {
                return NotFound("No such exam found.");
            }

            var generator = new ClassExamReportGenerator(_db);
            var report = generator.GenerateReport(className, examId);

            return PartialView("ClassReport", report);
            }catch (Exception)
            {
                return NotFound("Make sure to match class name, exam year and exam name.");
            }
        }
    }
}
