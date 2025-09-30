using System.ComponentModel.DataAnnotations;

namespace SchoolResultSystem.Web.Models
{
    public class SubjectModel
    {
        [Key]
        public string SCode { get; set; } = null!;
        [Required]
        public string SName { get; set; } = null!; // e.g. Mathematics
        public bool IsActive { get; set; } = true;
    }
}
