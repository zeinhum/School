using System.ComponentModel.DataAnnotations;

namespace SchoolResultSystem.Web.Models
{
    public class StudentModel
    {
        [Key] // Unique student identifier
        [Required]
        public string NSN { get; set; } = null!;

        [Required]
        public string StudentName { get; set; } = null!;

        [Required]
        public DateOnly D_O_B { get; set; }

        public string Address { get; set; } = null!;
        public DateOnly AdmissionDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public bool IsActive { get; set; } = true;
    }
}
