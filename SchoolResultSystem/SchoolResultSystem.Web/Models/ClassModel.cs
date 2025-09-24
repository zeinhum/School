using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolResultSystem.Web.Models
{
    public class ClassModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // auto increment
        public int ClassId { get; set; }  

        public string? ClassName { get; set; }
        public int? TeacherId { get; set; }  // assuming Teacher is another entity
    }
}
