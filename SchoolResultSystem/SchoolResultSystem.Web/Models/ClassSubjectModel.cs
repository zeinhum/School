
using Microsoft.EntityFrameworkCore;
namespace SchoolResultSystem.Web.Models
{
    [Keyless]
    public class ClassSubjectModel
    {

        public string? ClassId { get; set; }
        public string? SubjectCode { get; set; }
    }
}