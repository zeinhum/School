using System.Data.Common;
using Microsoft.AspNetCore.Mvc;
using SchoolResultSystem.Web.Data;
using Microsoft.Data.Sqlite;
using SchoolResultSystem.Web.Models;


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

                // take to login
                return View("Login", school);
            }
            catch (SqliteException ex)
            {
                // Table does not exist at all
                ViewBag.Error = "Database error: " + ex.Message;
                return View("Start");
            }
            catch (Exception ex)
            {
                // Any other unexpected error
                ViewBag.Error = "Unexpected error:\n" + ex.Message;
                return View("Start");
            }
        }

        public IActionResult SchoolAccount()
        {
            var firstAcc = _db.Users.Any();
            if (!firstAcc)
            {
                return View();
            }
           return View("Login");
        }


        // save first admin account

        [HttpPost]
        public IActionResult SavePrincipal(UserModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("SchoolAccount");
            }
            try
            {
                // remove old admin if exists
                var oldAdmins = _db.Users.Where(u => u.Role == "Admin").ToList();
                _db.Users.RemoveRange(oldAdmins);

                // enforce admin role
                model.Role = "Admin";
                _db.Users.Add(model);

                _db.SaveChanges();

                TempData["success"] = "Principal account set successfully!"; // redirect to schoolsetup
                return View("Login");

            }
            catch (Exception)
            {
                ViewBag.error = "Some error occured. Try again.";
                return View("SchoolAccount");
            }
        }
        // Check if school info exists in DB

        public IActionResult Start()
        {
            
            return View("Start");
        }

        public IActionResult Login()
        {
            return View("Login");
        }
    }
}
