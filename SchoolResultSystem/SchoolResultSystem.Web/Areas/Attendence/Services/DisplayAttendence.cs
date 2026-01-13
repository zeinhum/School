// logic to display attendence


using System.Data.Common;
using SchoolResultSystem.Web.Areas.Attendence.Models;
using SchoolResultSystem.Web.Data;

namespace SchoolResultSystem.Web.Areas.Attendence.Services
{


    public class DisplayAttendence(SchoolDbContext db)
    {


        public DisplayAttendenceDto ExtractAttendenceData(AttendenceRequestDto dto)
        {
            // Attendance data (dynamic via the switch)
            var presentDates = GetAttendanceQuery(dto)
                .Select(d => d.Date)
                .ToHashSet();

            // Leaves
            var leaveDates = db.Leaves
                .Where(l => l.Date >= dto.From && l.Date <= dto.Till)
                .Select(l => l.Date.Date)
                .ToHashSet();

            var result = new DisplayAttendenceDto();

            int totalDays = (dto.Till.Date - dto.From.Date).Days;

            for (int i = 0; i <= totalDays; i++)
            {
                var day = dto.From.Date.AddDays(i);

                if (leaveDates.Contains(day)){
                    result.Leaves.Add(day);
                }
                else
                {
                    result.SchoolDays.Add(day);
                    if (presentDates.Contains(day))
                    result.Present.Add(day);
                else
                    result.Absent.Add(day);
                }

                
            }
            result.RequestData=dto;
            
            return result;
        }


        // helper to get presentdates of candidates
        private IQueryable<DateTime> GetAttendanceQuery(AttendenceRequestDto dto)
        {
            switch (dto.CandidateType)
            {
                case "teacher":
                    return db.TeacherAttendance
                        .Where(d => d.TeacherId == dto.Id &&
                                    d.LoginDateTime >= dto.From &&
                                    d.LoginDateTime <= dto.Till)
                        .Select(d => d.LoginDateTime);

                case "student":
                    return db.StudentAttendance
                        .Where(d => d.NSN == dto.Id &&
                                    d.AttendanceDate >= dto.From &&
                                    d.AttendanceDate <= dto.Till)
                        .Select(d => d.AttendanceDate);

                default:
                    throw new Exception($"Invalid attendance Request");
            }
        }


    }
}
