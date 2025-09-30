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
        public DbSet<MarksheetModel> Marksheet { get; set; }
        public DbSet<CSModel> CS { get; set; }
        public DbSet<SubjectModel> Subjects { get; set; }
        public DbSet<StudentModel> Students { get; set; }  
        public DbSet<CSTModel> CST { get; set; }  



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ExamModel>()
                .HasOne(e => e.Subject)
                .WithMany()
                .HasForeignKey(e => e.SCode)
                .OnDelete(DeleteBehavior.Restrict); // Restrict delete exams when subject is deleted

            // Relationship with Class
            modelBuilder.Entity<CSTModel>()
                .HasOne(cs => cs.Class)
                .WithMany()
                .HasForeignKey(cs => cs.ClassId)
                .OnDelete(DeleteBehavior.Restrict);

        
            modelBuilder.Entity<CSTModel>()
                .HasOne(cs => cs.Subject)
                .WithMany()
                .HasForeignKey(cs => cs.SCode)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<CSModel>()
                .HasOne(cs => cs.Student)
                .WithMany()
                .HasForeignKey(cs => cs.NSN)
                .OnDelete(DeleteBehavior.Restrict); // keep enrollment history

            modelBuilder.Entity<CSModel>()
                .HasOne(cs => cs.Class)
                .WithMany()
                .HasForeignKey(cs => cs.ClassId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
