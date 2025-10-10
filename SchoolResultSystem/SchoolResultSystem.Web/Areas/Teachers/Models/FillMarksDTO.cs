namespace SchoolResultSystem.Web.Areas.Teachers.Models
{
    //show subjects of the class
    public class FillMarks
    {
        public ClsSub Info { get; set; } = new ClsSub();
        public List<CS> Students { get; set; } = new List<CS>();
        public List<Exams> exams { get; set; } = new List<Exams>();
    }
    public class UpdateMarks
    {
        public ClsSub Info { get; set; } = new ClsSub();
        public List<Exams> exams { get; set; } = new List<Exams>();
    }
    public class Exams
    {
        public int ExamId { get; set; }
        public string ExamName { get; set; } = null!;
        public int AcademicYear { get; set; }
    }
    public class CS
    {
        public string NSN { get; set; } = null!;
        public string StudentName { get; set; } = null!;
    }
    public class ClsSub
    {
        public string ClassName { get; set; } = null!;
        public int ClassId { get; set; }
        public string SCode { get; set; } = null!;
        public string SName { get; set; } = null!;
    }

    
}