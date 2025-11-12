namespace SchoolResultSystem.Web.Areas.Microservices.Models
{
    public class Changeclass
    {
        public int FromClass{ get; set; }
        public int Toclass { get; set; }
        public List<string> MovingStudents { get; set; } = null!;
    }
}