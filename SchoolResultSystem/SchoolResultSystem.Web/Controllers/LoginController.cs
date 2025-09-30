using Microsoft.AspNetCore.Mvc;
using SchoolResultSystem.Web.Models;
using SchoolResultSystem.Web.Data;

namespace SchoolResultSystem.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly SchoolDbContext _db;

        public LoginController(SchoolDbContext db)
        {
            _db = db;
        }


        [HttpPost]
        public IActionResult Authenticate(string username, string password)
        {
            var Users = _db.Users.FirstOrDefault();
            try
            {
                var user = _db.Users
                              .FirstOrDefault(u => u.UserName == username && u.Password == password);

                if (user != null)
                {
                    if (user.Role == "Admin") return RedirectToAction("Index", "PrincipalDashboard", new { area = "Principal" });

                    if (user.Role == "Teacher") return RedirectToAction("Index", "TeacherDashboard", new { area = "Teachers" });

                }
                TempData["error"] = "Username or password was not found.";
                return RedirectToAction("Welcome", "Home");
            }
            catch (Exception)
            {
                TempData["error"] = "Login system is not set up. Please contact admin.";
                // optionally log ex.Message
                return RedirectToAction("Index", "Home");
            }
        }

    }
}
