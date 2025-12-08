
using Microsoft.AspNetCore.SignalR;
using SchoolResultSystem.Web.Data;
using SchoolResultSystem.Web.Models;


namespace SchoolResultSystem.Web.Areas.Attendence.Model
{
    public  class StudentAttendanceDTO
    {
        public string TeacherId{get;set;}=null!;
        public int ClassId{get;set;}
        public List<StudentDetail> StudentDetails{get;set;} = null!;

    }

    public class ClassAttendenceDTO
    {
        public string ClassName{get;set;} = null!;
        public int ClassId{get;set;}
        public List<StudsDTO> Studs{get;set;} =null!;
    }

public class StudsDTO
    {
        public string StudentName {get;set;} =null!;
        public string NSN {get;set;} = null!;
    }
    public class StudentDetail
    {
        public string NSN {get;set;} = null!;
        public bool Present{get;set;} = true;
        public string AbsentReason {get;set;} = null!;
    }
}