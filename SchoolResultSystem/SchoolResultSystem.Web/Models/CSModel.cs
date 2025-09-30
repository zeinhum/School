using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolResultSystem.Web.Models
{
    public class CSModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string NSN { get; set; } = null!; 
        [Required]
        public int ClassId { get; set; } 
        [ForeignKey(nameof(NSN))]
        public virtual StudentModel Student { get; set; } = null!;

        [ForeignKey(nameof(ClassId))]
        public virtual ClassModel Class { get; set; } = null!;
    }
}