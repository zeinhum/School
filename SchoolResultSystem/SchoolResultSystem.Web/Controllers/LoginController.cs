using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using SchoolResultSystem.Web.Areas.Attendence.Model;
using SchoolResultSystem.Web.Data;
using SchoolResultSystem.Web.Models;
using System.IO;
using System.Threading.Tasks;

namespace SchoolResultSystem.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly SchoolDbContext _db;
        private readonly IWebHostEnvironment _env;

        public LoginController(SchoolDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate(string username, string password)
        {
            try
            {
                // Check session first
                var session = HttpContext.Session;
                var userRole = session.GetString("UserRole");

                if (userRole == null)
                {
                    // Fetch user if not in session
                    var user = _db.Users.FirstOrDefault(u => u.UserName == username && u.Password == password);

                    if (user == null)
                    {
                        TempData["error"] = "Username or password was not found.";
                        return RedirectToAction("Start", "Home");
                    }

                    if (!user.IsActive)
                    {
                        TempData["error"] = user.Role == "Admin"
                            ? "Your admin account is deactivated."
                            : "Your teacher account is deactivated.";
                        return RedirectToAction("Start", "Home");
                    }
                    userRole = user.Role;


                    //attendence
                    bool Attendence = await TakeAttendence.TeacherAttendance(_db, user.TeacherId);

                    if (!Attendence)
                    {
                        // redirect to login
                        TempData["error"] = "Sorry! attendence could not be marked. Please try again.";
                        return RedirectToAction("Login", "Home");

                    }
                    // ✅ Set session & TempData in one place

                    SetUserSession(user);

                }



                // Redirect based on role
                if (userRole == "Admin")
                {
                    return RedirectToAction("Index", "PrincipalDashboard", new { area = "Principal" });
                }
                else if (userRole == "Teacher")
                {
                    var userId = session.GetString("UserId");

                    return RedirectToAction("Index", "TeachersDashboard", new { area = "Teachers", id = userId });
                }

                TempData["error"] = "Invalid role.";
                return RedirectToAction("Start", "Home");
            }
            catch
            {
                TempData["error"] = "Login system is not set up. Please contact admin.";
                return RedirectToAction("Start", "Home");
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["error"] = "You are logged out.";
            return RedirectToAction("Login", "Home");
        }

        /// <summary>
        /// Helper to set session and TempData (avoid repetition)
        /// </summary>
        private void SetUserSession(UserModel user)
        {
            var session = HttpContext.Session;

            // Session
            session.SetString("UserRole", user.Role);
            session.SetString("UserName", user.UserName);
            session.SetString("UserId", user.TeacherId);

            // TempData
            TempData["user"] = user.UserName;
            var school = _db.SchoolInfo.Select(s => s.Name).FirstOrDefault();
            TempData["schoolName"] = school;
            TempData["userId"] = user.TeacherId;

        }
    }
}
