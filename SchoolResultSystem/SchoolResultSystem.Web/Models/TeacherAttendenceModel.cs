using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SchoolResultSystem.Web.Models
{
    public class TeacherAttendanceModel
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string TeacherId { get; set; } = null!;

    [Required]
    public DateTime LoginDateTime { get; set; } = DateTime.UtcNow;

    public DateTime? LogoutDateTime { get; set; }


    [ForeignKey(nameof(TeacherId))]
    public virtual UserModel User { get; set; } = null!;
}

}