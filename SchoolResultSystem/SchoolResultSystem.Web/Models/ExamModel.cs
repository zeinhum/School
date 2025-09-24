using System.ComponentModel.DataAnnotations;

namespace SchoolResultSystem.Web.Models
{
    public class ExamModel
    {
        [Key]
        public string? ExamId { get; set; }
        public string? ExamName { get; set; }
        public string? AcademicYear { get; set; }
}
}

