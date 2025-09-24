using System.Data.Common;
using Microsoft.AspNetCore.Mvc;
using SchoolResultSystem.Web.Data;


namespace SchoolResultSystem.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly SchoolDbContext _db;

        public HomeController(SchoolDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            // Check if school info exists in DB
            var school = _db.SchoolInfo.FirstOrDefault();

            if (school == null)
            {
                // No school info → redirect to principal setup
                return View("Index");
            }

            // If school info exists, show main home page
            return View("Login", school);
        }
        public IActionResult Welcome()
        {
            var Users = _db.Users.FirstOrDefault();
            return View("Welcome", Users);
        }
    }
}
