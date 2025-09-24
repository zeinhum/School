using Microsoft.AspNetCore.Mvc;
using SchoolResultSystem.Web.Models;
using SchoolResultSystem.Web.Data;


[Area("Principal")]
public class SetupPrincipal : Controller
{
    private readonly SchoolDbContext _db;

    public SetupPrincipal(SchoolDbContext db)
    {
        _db = db;
    }

    [HttpPost]
    public IActionResult SavePrincipal(UserModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("SetupPrincipalView");
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

            TempData["success"] = "Principal account set successfully!";
            return RedirectToAction("SetupSchoolView");
        }
        catch (Exception)
        {
            ViewBag.error = "Some error occured. Try again.";
            return View("SetupPrincipalView");
        }
    } 

    [HttpPost]
    public IActionResult SaveSchool(SchoolInfoModel model)
    {
        try
        {
            // optional: clear existing school info
            _db.SchoolInfo.RemoveRange(_db.SchoolInfo);
            _db.SchoolInfo.Add(model);
            _db.SaveChanges();

            TempData["success"] = "School info saved!";
            return RedirectToAction("Index");
        }
        catch (Exception)
        {
            ViewBag.error = "Error occured. Try again.";
            return View("SetupSchoolView");
        } 
        }
        

    public IActionResult SetupSchoolView()
    {
        return View();
    }


    [HttpGet] // principal set up view
    public IActionResult SetupPrincipalView()
    {
        return View();
    }
}
