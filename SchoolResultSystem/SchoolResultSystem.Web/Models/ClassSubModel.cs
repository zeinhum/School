using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SchoolResultSystem.Web.Areas.Analytics.Models;

namespace SchoolResultSystem.Web.Models
{
    public class ClassSubjectModel
    {
        [Key]
        public int rowId { get; set; }
        [Required]
        [ForeignKey("Class")]
        public int ClassId { get; set; }
        [Required]
        [ForeignKey("Subject")]
        public string SCode { get; set; } = null!;
        public virtual SubjectModel? Subject {get; set;}
        
        public virtual ClassModel? Class { get; set; } 
    }
}