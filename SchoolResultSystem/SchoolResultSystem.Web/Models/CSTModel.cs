using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SchoolResultSystem.Web.Models
{
    public class CSTModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey("Class")]
        public int ClassId { get; set; }
        [Required]
        [ForeignKey("Subject")]
        public string SCode { get; set; } = null!;
        [Required]
        [ForeignKey("User")] // teacher
        public string UserId { get; set; } = null!;
        // Navigation properties
        public virtual ClassModel Class { get; set; } = null!;
        public virtual SubjectModel Subject { get; set; } = null!;
        public virtual UserModel User { get; set; } = null!;
    }
}
