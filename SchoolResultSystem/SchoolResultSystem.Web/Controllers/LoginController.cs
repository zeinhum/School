using Microsoft.AspNetCore.Mvc;
using SchoolResultSystem.Web.Data;
using SchoolResultSystem.Web.Models;


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
                 UserModel user = new();
                 var session = HttpContext.Session;
                 var userRole = session.GetString("UserRole");
                if (userRole==null)
                {
                    user = _db.Users
                              .FirstOrDefault(u => u.UserName == username && u.Password == password)!;
                    userRole = user.Role;
                }
                

               
                    if (user.Role == "Admin")
                    {
                        if (user.IsActive == false)
                        {
                            TempData["error"] = "Your admin account is deactivated.";
                            return RedirectToAction("Start", "Home");
                        }
                        // set session
                        HttpContext.Session.SetString("UserRole", user.Role);
                        HttpContext.Session.SetString("UserName", user.UserName);
                    TempData["user"] = user.UserName;
                        var school =_db.SchoolInfo.Select(s => s.Name).FirstOrDefault();
                    TempData["schoolName"] = school;
                    Console.WriteLine($"school Name: {TempData["schoolName"]}");
                        return RedirectToAction("Index", "PrincipalDashboard", new { area = "Principal" });
                    }

                    if (user.Role == "Teacher")
                    {
                        if (user.IsActive == false)
                        {
                            TempData["error"] = "Your teacher account is deactivated.";
                            return RedirectToAction("Start", "Home");
                        }
                        HttpContext.Session.SetString("UserRole", user.Role);
                        HttpContext.Session.SetString("UserName", user.UserName);

                         TempData["user"] = user.UserName;
                         TempData["schoolName"] = _db.SchoolInfo.Select(s => s.Name).FirstOrDefault();
                        return RedirectToAction("Index", "TeachersDashboard", new { area = "Teachers", id = user.TeacherId });
                    }

                TempData["error"] = "Username or password was not found.";
                return RedirectToAction("Start", "Home");
            }
            catch (Exception)
            {
                TempData["error"] = "Login system is not set up. Please contact admin.";
                // optionally log ex.Message
                return RedirectToAction("Start", "Home");
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["error"] = "you are logged out.";
            return RedirectToAction("Login", "Home");
        }
    }
}
