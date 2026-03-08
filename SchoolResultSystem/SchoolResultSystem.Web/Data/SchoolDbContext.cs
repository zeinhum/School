using Microsoft.EntityFrameworkCore;
using SchoolResultSystem.Web.Models;

namespace SchoolResultSystem.Web.Data
{
    public class SchoolDbContext : DbContext
    {
        public SchoolDbContext()
        {
        }

        public SchoolDbContext(DbContextOptions<SchoolDbContext> options) : base(options) { }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<SchoolInfoModel> SchoolInfo { get; set; }
        public DbSet<ClassModel> Classes { get; set; }
        public DbSet<MarksheetModel> Marksheet { get; set; }
        public DbSet<CSModel> ClassStudent { get; set; }
        public DbSet<SubjectModel> Subjects { get; set; }
        public DbSet<StudentModel> Students { get; set; }
        public DbSet<CSTModel> CST { get; set; }
        public DbSet<TeacherAttendanceModel> TeacherAttendance { get; set; }
        public DbSet<StudentAttendanceModel> StudentAttendance { get; set; }

        public DbSet<LeavesCalendarModel> LeaveDates { get; set; }

        public DbSet<ClassSubjectModel> ClassSubject { get; set; }
        public DbSet<ExamModel> ExamList { get; set; }
        public DbSet<ExamRubrickModel> ExamRubrick { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // uniq constraint to avoid duplicate entries

            modelBuilder.Entity<ExamRubrickModel>()
            .HasIndex(m => new
            {
                m.ExamId,
                m.SCode
            }).IsUnique();

            modelBuilder.Entity<CSTModel>()
            .HasKey(c => new { c.ClassId, c.SCode, c.UserId });
            base.OnModelCreating(modelBuilder);

        }
    }

}
