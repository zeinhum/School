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
            try
            {
                var user = _db.Users
                              .FirstOrDefault(u => u.UserName == username && u.Password == password);
                
                if (user != null)
                {
                    if (user.Role == "Admin")
                    {
                        if (user.IsActive == false)
                        {
                            TempData["error"] = "Your admin account is deactivated.";
                            return RedirectToAction("Index", "Home");
                        }
                        return RedirectToAction("Index", "PrincipalDashboard", new { area = "Principal" });
                    }

                    if (user.Role == "Teacher")
                    {
                        if (user.IsActive == false)
                        {
                            TempData["error"] = "Your teacher account is deactivated.";
                            return RedirectToAction("Index", "Home");
                        }
                        return RedirectToAction("Index", "TeachersDashboard", new { area = "Teachers" ,id=user.TeacherId});
                    }

                }
                TempData["error"] = "Username or password was not found.";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {
                TempData["error"] = "Login system is not set up. Please contact admin." ;
                // optionally log ex.Message
                return RedirectToAction("Index", "Home");
            }
        }

    }
}
