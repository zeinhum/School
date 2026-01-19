// this controller makes marsksheet per student of recent exam


using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolResultSystem.Web.Areas.Analytics.Models;
using SchoolResultSystem.Web.Data;
using SchoolResultSystem.Web.Filters;
using SchoolResultSystem.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SchoolResultSystem.Web.Areas.Analytics.Controllers
{
    [Area("Analytics")]
    [AuthorizeUser]
    public class StudentController : Controller
    {
        private readonly SchoolDbContext _db;

        public StudentController(SchoolDbContext db)
        {
            _db = db;
        }

        [HttpPost]
        public IActionResult Marksheet([FromBody] StdReq ns)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid model data.");

            if (ns == null || string.IsNullOrWhiteSpace(ns.NSN))
                return BadRequest("Student NSN is required.");

            string nsn = ns.NSN;

            int examId;

            if (ns.exName == "default")
            {
                // get most recent exam
                examId = _db.Exams.Any()
                    ? _db.Exams.Max(e => e.ExamId)
                    : 0;
            }
            else
            {
                examId = _db.Exams
                .Where(e => e.AcademicYear == ns.exYear && e.ExamName == ns.exName)
                .Select(e => e.ExamId)
                .FirstOrDefault(); // returns 0 if not found
            }

            // Load all subjects in this exam
            var examSubjects = _db.Exams
                .Include(e => e.Subject)
                .Where(e => e.ExamId == examId)
                .ToList();

            if (!examSubjects.Any())
                return NotFound("No subjects found for this exam.");

            Console.WriteLine("Exam subjects (from ExamModel):");
            foreach (var ex in examSubjects)
            {
                Console.WriteLine($" - {ex.SCode}: Th={ex.ThMark}, Pr={ex.PrMark}");
            }

            // Load student
            var student = _db.Students.FirstOrDefault(s => s.NSN == nsn);
            if (student == null)
                return NotFound($"Student with NSN '{nsn}' not found.");
            var marks = _db.Marksheet.Select(s => s.SCode).ToList();

            // Load student's marks
            var studentMarks = _db.Marksheet
                .Include(m => m.Subject)
                .Where(m => m.ExamId == examId && m.NSN == nsn)
                .ToList();

            if (!studentMarks.Any())
                return NotFound($"No marks found for student {student.StudentName} in exam {examId}.");

            foreach (var mark in studentMarks)
            {
                Console.WriteLine(studentMarks);
                Console.WriteLine($" - {mark.SCode} ({mark.Subject?.SName}): Th={mark.ThMark}, Pr={mark.PrMark}");
            }

            // Build student’s obtained marks list
            var obtainedList = studentMarks.Select(m => new Subject
            {
                Sub = new SubjectModel
                {
                    SCode = m.SCode,
                    SName = m.Subject?.SName ?? "Unknown"
                },
                ThMark = m.ThMark,
                PrMark = m.PrMark
            }).ToList();

            // Calculate all subject grades
            var subjectGpas = FindGrade.SubjectGrades(examSubjects, obtainedList);

            //  Calculate overall GPA
            var overallGpa = FindGrade.OverAllGPA(subjectGpas, examSubjects);

            //  Build DTO
            var result = new MarkSheetDto
            {
                Schoolname = "Nanu Ai School",
                Exam = examSubjects.First(),
                Student = student,
                gpas = subjectGpas,
                GPA = overallGpa
            };

            return PartialView("Marksheet", result);
        }
    }
}
