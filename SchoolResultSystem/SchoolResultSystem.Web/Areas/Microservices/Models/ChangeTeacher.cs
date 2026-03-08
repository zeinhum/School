namespace SchoolResultSystem.Web.Areas.Microservices.Models
{
    public class Teacher
    {
        public string Id{get; set;} = null!;
    }

    public class ChangePw
    {
        public string Id{get;set;} = null!;
        public string NewPw{get;set;} = null!;
    }

    public class SubjectTeacher
    {
        public string Scode{get;set;} = null!;
        public int ClassId {get; set;} 
        public string UserId {get; set;} = null!;
    }
}