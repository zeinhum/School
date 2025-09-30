using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolResultSystem.Web.Models
{
    public class UserModel
    {
        [Key]
        public string TeacherId { get; set; } = null!;
        [Required]
        public string UserName { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
        [Required]
        public string TeacherName { get; set; } = null!;
        [Required]
        public string Role { get; set; } = null!;
        public bool IsActive { get; set; } = true;

    }
}