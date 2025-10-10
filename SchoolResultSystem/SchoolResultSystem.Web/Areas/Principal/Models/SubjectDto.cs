namespace SchoolResultSystem.Web.Areas.Principal.Models
{
    public class SubjectDto
    {
        public string SCode { get; set; } = null!;
        public string SName { get; set; } = null!;
    }

    public class SubjectDtoModel
    {
        public List<SubjectDto> Subjects { get; set; } = new();
    }
}