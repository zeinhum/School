using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolResultSystem.Web.Models
{
    public class ExamModel
    {
        [Key]
        public int Tablerow{ get; set; }
        [Required]
        public int ExamId { get; set; }
        [Required]
        public int AcademicYear { get; set; } = DateTime.Now.Year;
        [Required]
        public string ExamName { get; set; } = null!;
        [Required]
        public string SCode { get; set; } = null!;
        [Required]
        public decimal ThMark { get; set; }
        [Required]
        public decimal PrMark { get; set; }
        [Required]
        public decimal ThCrh { get; set; }
        [Required]
        public decimal PrCrh { get; set; }
        [ForeignKey(nameof(SCode))]
        public virtual SubjectModel? Subject { get; set; }
        
    }
}

