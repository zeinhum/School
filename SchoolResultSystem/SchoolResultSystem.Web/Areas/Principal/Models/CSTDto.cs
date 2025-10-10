namespace SchoolResultSystem.Web.Areas.Principal.Models
{
    public class CSTDto
    {
        public string SCode { get; set; } = null!;
        public string SubjectName { get; set; } = null!;
        public string TeacherId { get; set; } = null!;
        public string TeacherName { get; set; } = null!;
    }

    public class CSTView
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; } = null!;
        public List<CSTDto> SubjectTeachers { get; set; } = new List<CSTDto>();
    }

    public class AddCSTDto
    {
        public int ClassId { get; set; }
        public string SCode { get; set; } = null!;
        public string TeacherId { get; set; } = null!;
    }
}