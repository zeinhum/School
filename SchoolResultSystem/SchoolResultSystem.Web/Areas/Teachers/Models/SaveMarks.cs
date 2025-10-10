using System.ComponentModel.DataAnnotations.Schema;
using SchoolResultSystem.Web.Models;

namespace SchoolResultSystem.Web.Areas.Teachers.Models
{
    public class SaveMarks
    {
        public int ExamId { get; set; }
        public string SCode { get; set; } = null!;
        public List<Marks> Marks{ get; set; }= new List<Marks>();
    }
    public class Marks
    {
        public string NSN { get; set; } = null!;
        public decimal ThMark { get; set; }
        public decimal PrMark { get; set; }
    }
    
}