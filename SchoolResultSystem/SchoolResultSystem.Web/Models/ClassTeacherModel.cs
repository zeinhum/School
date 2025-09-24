using Microsoft.EntityFrameworkCore;

namespace SchoolResultSystem.Web.Models
{
    [Keyless]
    public class ClassTeacherModel
    {
        public int? ClassId { get; set; }
        public int? UserId { get; set; }
    }
}