using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolResultSystem.Web.Models
{
    public class UserModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }
        public string? UserName { get; set; }
        public string? Passwords { get; set; }
        public string? TeacherName { get; set; }
        public string? Role { get; set; }
       // public string? Email { get; set; }
    }
}