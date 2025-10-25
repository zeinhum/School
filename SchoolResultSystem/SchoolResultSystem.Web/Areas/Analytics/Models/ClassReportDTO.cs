namespace SchoolResultSystem.Web.Areas.Analytics.Models
{
    public class Requet
    {
        public string NSN { get; set; } = null!;
    }
    public class ClassExamReportDTO
    {
        public string ClassName { get; set; } = null!;
        public int ExamId { get; set; }
        public string ExamName { get; set; } = null!;
        public List<StudentExamReportDTO> Students { get; set; } = new();
    }

    public class StudentExamReportDTO
    {
        public string NSN { get; set; } = null!;
        public string StudentName { get; set; } = null!;
        public List<SubjectGradeDTO> Subjects { get; set; } = new();
        public (string GradeLetter, decimal GPA) GPA { get; set; }
    }

    public class SubjectGradeDTO
    {
        public string SubjectName { get; set; } = null!;
        public string SubjectCode { get; set; } = null!;
        public string Grade { get; set; } = null!;
        public decimal GradePoint { get; set; }
    }
}
