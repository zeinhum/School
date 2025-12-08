using Microsoft.AspNetCore.Mvc;
using SchoolResultSystem.Web.Models;
using SchoolResultSystem.Web.Areas.Teachers.Models;
using SchoolResultSystem.Web.Data;
using System.Linq.Expressions;
using SchoolResultSystem.Web.Filters;
using SchoolResultSystem.Web.Areas.Attendence.Model;
using AspNetCoreGeneratedDocument;
using System.Threading.Tasks;

namespace SchoolResultSystem.Web.Areas.Teacher.Controllers
{
    [AuthorizeUser("Teacher")]
    [Area("Teachers")]
    public class ClassAttendenceController : Controller
    {
        private readonly SchoolDbContext _db;

        public ClassAttendenceController(SchoolDbContext db)
        {
            _db = db;
        }

        public IActionResult AttendenceLayout(int classId)
        {

            var CSObject = _db.CS.Where(s=>s.ClassId==classId && s.IsActive).ToList();
            return PartialView("_Attendence", CSObject);
        }
        public async Task<IActionResult> SubmitAttendence(StudentAttendanceDTO dto)
        {

            var attendenc = await TakeAttendence.MarkStudentAttendance(_db, dto);
            if (attendenc)
            {
                return Json(new { succes = true });
            }
            else
            {
                return Json(new{success=false});
            }
            
        }
    }
}