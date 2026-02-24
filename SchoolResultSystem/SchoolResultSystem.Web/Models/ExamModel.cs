using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolResultSystem.Web.Models
{
    public class ExamModel
    {
        [Key]
        [Required]
        public int ExamId { get; set; }
        [Required]
        public int AcademicYear { get; set; } = DateTime.Now.Year;
        [Required]
        public string ExamName { get; set; } = null!;
        [Required]
        public bool IsActive {get; set;} = true;
        
    }
}

