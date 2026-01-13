// calendar for school leaves , holidays, weekdays

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolResultSystem.Web.Models
{
    public class LeavesCalendarModel
{
    [Key]
    public int Id{get;set;}
    [Required]
    public DateTime Date{get;set;}
    [Required]
    public bool Weekday{get;set;}
    [Required]
    public bool Holiday{get;set;}
    public string Description{get;set;} =null!;
}
}
