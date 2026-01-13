// generate exam report of class of recent exam

using Microsoft.AspNetCore.Mvc;
using SchoolResultSystem.Web.Areas.Analytics.Models;
using SchoolResultSystem.Web.Data;
using SchoolResultSystem.Web.Models;
using System.Linq;

namespace SchoolResultSystem.Web.Areas.Analytics.Controllers
{
    [Area("Analytics")]
    public class ClassController : Controller
    {
        private readonly SchoolDbContext _db;

        public ClassController(SchoolDbContext db)
        {
            _db = db;
        }

        [HttpPost]
        public IActionResult Report([FromBody] Requet req)
        {
            var className = req.NSN;
            var examId = _db.Exams.Any() ? _db.Exams.Max(e => e.ExamId): 0;

            if (string.IsNullOrEmpty(className))
            {
                // Populate dropdown if no class selected yet
                ViewBag.Classes = _db.Classes
                    .Where(c => c.IsActive)
                    .Select(c => c.ClassName)
                    .ToList();

                ViewBag.Exams = _db.Exams
                    .Select(e => new { e.ExamId, e.ExamName })
                    .Distinct()
                    .ToList();

                return View();
            }

            var generator = new ClassExamReportGenerator(_db);
            var report = generator.GenerateReport(className, examId);

            ViewBag.Classes = _db.Classes
                .Where(c => c.IsActive)
                .Select(c => c.ClassName)
                .ToList();

            ViewBag.Exams = _db.Exams
                .Select(e => new { e.ExamId, e.ExamName })
                .Distinct()
                .ToList();

            return PartialView("ClassReport", report);
        }
    }
}
