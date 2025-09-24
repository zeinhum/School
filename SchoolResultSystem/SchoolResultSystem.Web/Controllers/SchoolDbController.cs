using Microsoft.AspNetCore.Mvc;
using SchoolResultSystem.Web.Data;

namespace SchoolResultSystem.Web.Controllers
{
    public abstract class SchoolDbController(SchoolDbContext db) : Controller
    {
        protected readonly SchoolDbContext _db = db;
    }
}
