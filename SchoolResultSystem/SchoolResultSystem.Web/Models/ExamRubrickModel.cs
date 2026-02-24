using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolResultSystem.Web.Models
{
    public class ExamRubrickModel
    {
        [Key]
        public int rubrick{get; set;}
        [Required]
        public int ExamId {get; set;}
        [Required]
        public string SCode{get;set;} = null!;
        [Required]
        public decimal CreditHour{get; set;}
        public decimal FullMark {get; set;}
        [ForeignKey(nameof(ExamId))]
        public virtual ExamModel Exam {get; set;} = null!;
        [ForeignKey(nameof(SCode))]
        public virtual SubjectModel Subs {get; set;} = null!;
        
    }
    public class ExamSubjectsDTO
    {
        public int ExamId {get; set;}
        public List<Subs> Subs{get;set;}=[];  

    }

    public class Subs
    {
        public string SCode { get; set; } = null!;
        public decimal FullMark{get;set;} 
        public decimal CreditHour {get; set;}
    }
}