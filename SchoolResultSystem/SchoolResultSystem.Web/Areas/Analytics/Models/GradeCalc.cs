using System;
using System.Collections.Generic;
using System.Linq;
using SchoolResultSystem.Web.Models;

namespace SchoolResultSystem.Web.Areas.Analytics.Models
{
    public static class FindGrade
    {
        public static List<GPADto> SubjectGrades(
            List<ExamModel> examSubjects,
            List<Subject> studentObtainedMarks)
        {
            var gpas = new List<GPADto>();
Console.WriteLine($"exam subject from grade calculator");
            foreach (var examSub in examSubjects)
            {
                // Check if student has marks for this subject
                var obtained = studentObtainedMarks
                    .FirstOrDefault(m => m.Sub.SCode == examSub.SCode);

                Console.WriteLine($" subject === {examSub.SCode}");

                decimal thMark = obtained?.ThMark ?? 0;
                decimal prMark = obtained?.PrMark ?? 0;

                // Full marks (avoid divide by zero)
                decimal fullTh = examSub.ThMark == 0 ? 1 : examSub.ThMark;
                decimal fullPr = examSub.PrMark == 0 ? 1 : examSub.PrMark;

                // If student absent → AB grade, 0 point
                (string Grade, decimal GradePoint) thGrade = (obtained == null) ? ("AB", 0.0m) : AssignGrade(thMark / fullTh * 100);
                (string Grade, decimal GradePoint) prGrade = (obtained == null) ? ("AB", 0.0m) : AssignGrade(prMark / fullPr * 100);

                // Credit hours
                decimal credithr = examSub.ThCrh + examSub.PrCrh;
                decimal weight = (examSub.ThCrh * thGrade.GradePoint) + (examSub.PrCrh * prGrade.GradePoint);

                // Final grade
                decimal finalGradeValue = credithr == 0 ? 0 : weight / credithr;
                (string Grade, decimal GradePoint) finalGrade = (obtained == null) ? ("AB", 0.0m) : AssignGrade(finalGradeValue * 25);

                gpas.Add(new GPADto
                {
                    Sub = new SubjectModel
                    {
                        SCode = examSub.SCode,
                        SName = examSub.Subject?.SName ?? "Unknown"
                    },
                    ThGrade = thGrade,
                    PrGrade = prGrade,
                    FinalGrade = finalGrade,
                    CreditHour = credithr,
                    Weight = weight
                });
            }

            return gpas;
        }

        /// <summary>
        /// Calculate overall GPA for a student
        /// </summary>
        public static (string Grade, decimal GradePoint) OverAllGPA(
            List<GPADto> subjectGpas,
            List<ExamModel> examSubjects)
        {
            decimal totalCreditHours = 0;
            decimal totalCreditPoints = 0;

            foreach (var gpaItem in subjectGpas)
            {
                var creditObj = examSubjects.FirstOrDefault(e => e.SCode == gpaItem.Sub.SCode);
                if (creditObj == null) continue;

                decimal thCrh = creditObj.ThCrh;
                decimal prCrh = creditObj.PrCrh;

                totalCreditHours += thCrh + prCrh;
                totalCreditPoints += (thCrh * gpaItem.ThGrade.GradePoint)
                                   + (prCrh * gpaItem.PrGrade.GradePoint);
            }

            if (totalCreditHours == 0)
                return ("NG", 0.0m);

            decimal gpa = totalCreditPoints / totalCreditHours;
            var grade = AssignGrade(gpa * 25); // scale to 100

            return grade;
        }

        // ----------------------
        // Helper methods
        // ----------------------

        private static (string Grade, decimal GradePoint) AssignGrade(decimal percentage)
        {
            if (percentage >= 90) return ("A+", 4.0m);
            if (percentage >= 80) return ("A", 3.6m);
            if (percentage >= 70) return ("B+", 3.2m);
            if (percentage >= 60) return ("B", 2.8m);
            if (percentage >= 50) return ("C", 2.4m);
            if (percentage >= 40) return ("D", 2.0m);
            return ("NG", 0.0m);
        }
    }
}
