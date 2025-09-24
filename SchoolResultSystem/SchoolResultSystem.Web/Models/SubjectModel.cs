using System.ComponentModel.DataAnnotations;

namespace SchoolResultSystem.Web.Models
{
    public class SubjectModel
    {
        [Key]
        public string? SubjectCode { get; set; }
        public string? SubjectName { get; set; }        // e.g. Mathematics
        public int TheoryFullMark { get; set; }       // total marks
        public int PracticalFullMark { get; set; }       // pass marks
        public decimal CrHTheory { get; set; }     // teaching credit hours
        public decimal CrHPractical { get; set; }

    }
}
