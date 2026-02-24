namespace SchoolResultSystem.Web.Areas.Principal.Models
{
    public class SaveExam
    {
        public int AcademicYear { get; set; }
        public string ExamName { get; set; } = null!;
        public List<SubjectMarks> SubjectMarks { get; set; } = new List<SubjectMarks>();
    }

    public class SubjectMarks
    {
        public string SCode { get; set; } = null!;
        public decimal Mark { get; set; }
        public decimal Crh { get; set; }

    }
}