using Microsoft.EntityFrameworkCore;
using SchoolResultSystem.Web.Models;

namespace SchoolResultSystem.Web.Data
{
    public class SchoolDbContext : DbContext
    {
        public SchoolDbContext(DbContextOptions<SchoolDbContext> options) : base(options) { }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<SchoolInfoModel> SchoolInfo { get; set; }
        public DbSet<ExamModel> Exams { get; set; }
        public DbSet<ClassModel> Classes { get; set; }
        public DbSet<ExamResultModel> ExamResult { get; set; }
        public DbSet<ClassTeacherModel> ClassTeacher { get; set; }
        public DbSet<ClassSubjectModel> ClassSubject { get; set; }
        public DbSet<SubjectModel> Subject { get; set; }
       
    }
}
