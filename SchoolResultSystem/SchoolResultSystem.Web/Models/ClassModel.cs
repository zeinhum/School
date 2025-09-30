using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolResultSystem.Web.Models
{
    public class ClassModel
    {
        [Key]
        public int ClassId { get; set; } = 0;
        [Required]
        public string ClassName { get; set; } = null!;
        [Required]
        public bool IsActive { get; set; } = true;
        
         
    }
}
