namespace SchoolResultSystem.Web.Areas.Microservices.Models
{
    public class Exam
    {
        public int AcademicYear{ get; set; }
        public string ExamName { get; set; } = null!;
        public List<string> MovingStudents { get; set; } = null!;
    }

}