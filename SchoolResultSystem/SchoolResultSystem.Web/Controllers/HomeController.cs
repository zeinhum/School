using System.Data.Common;
using Microsoft.AspNetCore.Mvc;
using SchoolResultSystem.Web.Data;
using Microsoft.Data.Sqlite;


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
            try
            {
                var school = _db.SchoolInfo.FirstOrDefault();

                if (school == null)
                {
                    // Table exists, but no data
                    ViewBag.Error = "No school information found. Please run setup.";
                    return View("Index");
                }

                // School info exists → go to login
                return View("Login", school);
            }
            catch (SqliteException ex)
            {
                // Table does not exist at all
                ViewBag.Error = "Database error: " + ex.Message;
                return View("Index");
            }
            catch (Exception ex)
            {
                // Any other unexpected error
                ViewBag.Error = "Unexpected error:\n" + ex.Message;
                return View("Index");
            }
        }


        // Check if school info exists in DB

        public IActionResult Welcome()
        {
            var Users = _db.Users.ToList();
            return View("Welcome", Users);
        }

        public IActionResult Login()
        {
            return View("Login");
        }
    }
}
