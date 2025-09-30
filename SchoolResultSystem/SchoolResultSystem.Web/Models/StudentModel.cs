using System.ComponentModel.DataAnnotations;

namespace SchoolResultSystem.Web.Models
{
    public class StudentModel
    {
        [Key] // Unique student identifier
        public string NSN { get; set; } = null!;

        [Required]
        public string StudentName { get; set; } = null!;

        [Required]
        public DateTime D_O_B { get; set; }

        public string Address { get; set; } = null!;
        public DateTime AdmissionDate { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;
    }
}
