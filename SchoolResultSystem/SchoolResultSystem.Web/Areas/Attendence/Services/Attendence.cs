
using SchoolResultSystem.Web.Areas.Attendence.Model;
using SchoolResultSystem.Web.Data;
using SchoolResultSystem.Web.Models;


namespace SchoolResultSystem.Web.Areas.Attendence.Model
{
    public static class TakeAttendence
    {



        public static  async Task<bool> TeacherAttendance(SchoolDbContext _db, string id)
        {
            var today = DateTime.UtcNow.Date;
            var tomorrow = today.AddDays(1);

            bool isAttendanceDone = _db.TeacherAttendance
                .Any(a => a.TeacherId == id &&
                          a.LoginDateTime >= today &&
                          a.LoginDateTime < tomorrow);


            if (!isAttendanceDone)
            {
                try
                {
                    var payload = new TeacherAttendanceModel
                    {
                        TeacherId = id,
                        LoginDateTime = DateTime.UtcNow
                    };

                    _db.TeacherAttendance.Add(payload);
                    _db.SaveChanges();

                    return true;
                }
                catch
                {

                    return false;
                }
            }

            return true;
        }

        public static async Task<bool> MarkStudentAttendance(SchoolDbContext db, StudentAttendanceDTO dto)
        {
            if (dto == null || dto.StudentDetails == null)
                return false;

            var todayUtc = DateTime.UtcNow.Date;

            try
            {
                var AttendanceList = new List<StudentAttendanceModel>();

                foreach (var student in dto.StudentDetails)
                {


                    var payload = new StudentAttendanceModel
                    {
                        NSN = student.NSN,
                        AttendanceBy = dto.TeacherId,
                        AttendanceDate = DateTime.UtcNow,
                        Present = student.Present,
                        AbsentReason = student.AbsentReason
                    };

                    AttendanceList.Add(payload);
                }

                if (AttendanceList.Count>0)
                {
                    await db.StudentAttendance.AddRangeAsync(AttendanceList);
                    await db.SaveChangesAsync();
                    return true;
                }
                return false;
                
            }
            catch
            {

                return false;
            }
        }
    }
}