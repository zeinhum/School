using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolResultSystem.Web.Models
{
    public class MarksheetModel
    {
        [Key]
        public int MarksheetId { get; set; }
        [Required]
        public int ExamId { get; set; }
        [Required]
        public string NSN { get; set; } = null!;
        [Required]
        public string SCode { get; set; } = null!;
        [Required]
        public decimal ThMark { get; set; }
        [Required]
        public decimal PrMark { get; set; }

        // Navigation properties
        [ForeignKey(nameof(ExamId))]
        public virtual ExamModel Exam { get; set; } = null!;
        [ForeignKey(nameof(NSN))]
        public virtual StudentModel Student { get; set; } = null!;
        [ForeignKey(nameof(SCode))]
        public virtual SubjectModel Subject { get; set; } = null!;
    }
}