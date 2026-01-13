// displays histry of candidates attence : DTO

namespace SchoolResultSystem.Web.Areas.Attendence.Models
{
    public class DisplayAttendenceDto
    {
        
        public List<DateTime> Present{get;set;}=[];
        public List<DateTime>Absent{get;set;} =[];
        public List<DateTime> Leaves{get;set;} = []!;
        public List<DateTime> SchoolDays{get;set;}=[];

        public AttendenceRequestDto RequestData{get;set;} = new();
    }

    // receive request for attendence display dto

    public class AttendenceRequestDto
    {
        public string CandidateType{get;set;} = null!; // student or teacher
        public string CandidateName{get;set;} = null!;
        public string Id{get;set;}=null!;
        public DateTime From{get;set;}
        public DateTime Till{get;set;}
    }
}