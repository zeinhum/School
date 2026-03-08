
// dto for leave calendar set

namespace SchoolResultSystem.Web.Areas.Principal.Models
{
    public class CalendarDto
    {
        public List<DateTime> Weekdays{get;set;} = [];
        public List<LeaveDates> LeaveDates{get;set;}=[];
    }

    public class LeaveDates
    {
        public DateTime Leave{get;set;}
        public string Description{get;set;}=null!;
    }
}