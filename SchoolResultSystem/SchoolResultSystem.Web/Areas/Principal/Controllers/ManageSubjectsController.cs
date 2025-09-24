using Microsoft.AspNetCore.Mvc;
using SchoolResultSystem.Web.Models;
using SchoolResultSystem.Web.Data;
using SchoolResultSystem.Web.Controllers;

[Area("Principal")]
public class ManageSubjectsController : SchoolDbController
{
    public ManageSubjectsController(SchoolDbContext db) : base(db) { }

    [HttpPost]
    public IActionResult SaveSubject(SubjectModel model)
    {
        if (ModelState.IsValid)
        {
            _db.Subject.Add(model);
            _db.SaveChanges();
            return RedirectToAction("Index"); // adjust to your view
        }
        ViewBag.error = "Some error occured";
        return View("Index"); // return back with validation errors
    }

    public IActionResult Index()
    {
        // get all subjects
        var Subjects = _db.Subject.ToList();
        return View("ManageSubjects",Subjects);
    }
}
