namespace SchoolResultSystem.Web.Areas.Teachers.Models
{
    public class CSList
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; } = null!;
        public string SCode { get; set; } = null!;
        public string SName { get; set; } = null!;
    }
    // show subject teacher of the class
    public class TDashDto
    {
        public string SchoolName { get; set; } = null!;
        public string TeacherId { get; set; } = null!;
        public string TeacherName { get; set; } = null!;
        public List<CSList> CSList = new List<CSList>();
    }
}