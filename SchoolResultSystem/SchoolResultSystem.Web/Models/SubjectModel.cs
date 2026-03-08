using System.ComponentModel.DataAnnotations;

namespace SchoolResultSystem.Web.Models
{
    public class SubjectModel
    {
        [Key]
        public string SCode { get; set; } = null!;
        [Required]
        public string SName { get; set; } = null!; // e.g. Mathematics
        [Required]
        public string SType{get;set;} = null!; // theory / practical
        public string LinkedPr {get; set;}="none";
        public bool IsActive { get; set; } = true;
    }
}
