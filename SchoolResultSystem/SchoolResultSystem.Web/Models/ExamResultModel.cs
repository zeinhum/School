using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolResultSystem.Web.Models
{
    public class ExamResultModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? ExamId { get; set; }
        public string? StudentId { get; set; }
        public string? SubjectCode { get; set; }
        public decimal TheoryMark { get; set; }
        public decimal PracticalMark { get; set; }
    }
}