namespace SchoolResultSystem.Web.Areas.Principal.Models
{
    public class CSDto
    {
        public string NSN { get; set; } = null!;
        public string StudentName { get; set; } = null!;
    }
    public class CSViewModel
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; } = null!;
        public List<CSDto> Students { get; set; } = new List<CSDto>();
    }
}