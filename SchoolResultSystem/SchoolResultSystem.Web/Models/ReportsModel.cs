using System.ComponentModel.DataAnnotations;

namespace SchoolResultSystem.Web.Models
{
    public class ClassReports
    {
        [Key]
        public int ReportId{get; set;}
        [Required]
        public string ClassId{get; set;} = null!;
        [Required]
        public int ExamId{get;set;}
        [Required]
        public string ReportJson{get;set;} = null!;

    }
}