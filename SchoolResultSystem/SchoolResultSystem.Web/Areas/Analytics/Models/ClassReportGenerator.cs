using SchoolResultSystem.Web.Models;
using SchoolResultSystem.Web.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace SchoolResultSystem.Web.Areas.Analytics.Models
{
    public class ClassExamReportGenerator
    {
        private readonly SchoolDbContext _db;

        public ClassExamReportGenerator(SchoolDbContext db)
        {
            _db = db;
        }

        public ClassExamReportDTO GenerateReport(string className, int examId = 1)
        {
            // 1️⃣ Get class
            var classObj = _db.Classes.FirstOrDefault(c => c.ClassName == className);
            if (classObj == null)
                throw new Exception($"Class '{className}' not found.");

            // 2️⃣ Get all students in the class
            var students = _db.CS
                .Where(cs => cs.ClassId == classObj.ClassId)
                .Select(cs => cs.Student)
                .ToList();

            // 3️⃣ Get all exam subjects (full marks and credit hours)
            var examSubjects = _db.Exams
                .Include(e => e.Subject) // load navigation
                .Where(e => e.ExamId == examId)
                .ToList();

            Console.WriteLine($"exam subjects from exam model--------------");
            foreach(var ex in examSubjects)
            {
                    Console.WriteLine($"subject {ex.SCode} theory ={ex.ThMark} = pr {ex.PrMark}--------------");
            }
            
            if (!examSubjects.Any())
                throw new Exception($"No subjects found for ExamId={examId}");

            // 4️⃣ Prepare report DTO
            var report = new ClassExamReportDTO
            {
                ClassName = classObj.ClassName,
                ExamId = examId,
                ExamName = examSubjects.First().ExamName
            };

            // 5️⃣ Loop through each student
            foreach (var student in students)
            {
                // a) Get all marks for this student for this exam
                var studentMarks = _db.Marksheet
                    .Include(m => m.Subject) // critical to include Subject
                    .Where(m => m.ExamId == examId && m.NSN == student.NSN)
                    .ToList();
                Console.WriteLine($"Student---------------");
                Console.WriteLine($"obtained marks from marksheet---------------");
                    foreach (var mark in studentMarks)
    {
        Console.WriteLine($"{mark.NSN} {mark.SCode} {mark.Subject?.SName} = practical {mark.PrMark}, Theory {mark.ThMark}");
    }

                

                // b) Build full marks list
                var fullMarks = examSubjects
                    .Select(e => new Subject
                    {
                        Sub = e.Subject!,
                        ThMark = e.ThMark,
                        PrMark = e.PrMark
                    })
                    .ToList();
                Console.WriteLine($"Full Marks should be comming from exam model ---------------");
                foreach(var fm in fullMarks)
                {
                    Console.WriteLine($"Full Thmark {fm.Sub.SCode} = {fm.ThMark} and {fm.PrMark}---------------");  
                }
                // c) Build obtained marks list for this student
                var obtainedMarks = studentMarks
                    .Select(m => new Subject
                    {
                        Sub = m.Subject!,
                        ThMark = m.ThMark,
                        PrMark = m.PrMark
                    })
                    .ToList();
                Console.WriteLine($"Obtained Marks from prepared model ----------------------"); 
                foreach(var om in obtainedMarks)
                {
                    Console.WriteLine($"obtained marks {om.Sub.SCode} = {om.ThMark} and {om.PrMark}---------------"); 
                }
                var gpass = FindGrade.SubjectGrades(examSubjects,obtainedMarks );
                var overallGPA = FindGrade.OverAllGPA(gpass,examSubjects);

                Console.WriteLine($"Grading per student ---------------"); 
                foreach(var gr in gpass)
                {
                    Console.WriteLine($" gardes {gr.Sub.SCode} = {gr.ThGrade}  and {gr.PrGrade}  final {gr.FinalGrade}---------------"); 
                }

                

                // f) Map subject grades to DTO
                var subjectDTOs = gpass.Select(s => new SubjectGradeDTO
                {
                    SubjectCode = s.Sub.SCode,
                    SubjectName = s.Sub.SName,
                    Grade = s.FinalGrade.Grade,
                    GradePoint = s.FinalGrade.GradePoint
                }).ToList();

                // g) Add student to report
                report.Students.Add(new StudentExamReportDTO
                {
                    NSN = student.NSN,
                    StudentName = student.StudentName,
                    Subjects = subjectDTOs,
                    GPA = overallGPA
                });
            }

            return report;
        }
    }
}
