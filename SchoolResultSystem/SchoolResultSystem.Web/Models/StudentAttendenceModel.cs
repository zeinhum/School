using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SchoolResultSystem.Web.Models
{
    public class StudentAttendanceModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string NSN { get; set; } = null!;
        [Required]
        public int ClassId { get; set; }

        [Required]
        public string AttendanceBy { get; set; } = null!;

        [Required]
        public DateTime AttendanceDate { get; set; } = DateTime.UtcNow;

        public DateTime? AttendanceEndUtc { get; set; }

        public bool Present { get; set; } 
        public string AbsentReason { get; set; } = null!;

        [ForeignKey(nameof(NSN))]
        public virtual StudentModel Student { get; set; } = null!;

        [ForeignKey(nameof(AttendanceBy))]
        public virtual UserModel Teacher { get; set; } = null!;

        [ForeignKey(nameof(ClassId))]
        public virtual ClassModel Class { get; set; } = null!;
    }

}