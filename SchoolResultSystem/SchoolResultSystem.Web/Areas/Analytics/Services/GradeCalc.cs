
using SchoolResultSystem.Web.Models;
using SchoolResultSystem.Web.Areas.Analytics.Models;

namespace SchoolResultSystem.Web.Areas.Analytics.Services
{
    public static class FindGrade
    {
        public static (List<GPADto> gpas, string grade, decimal gradePoint) SubjectGrades(
            List<ExamSubjects> examSubjects, // selected exam's subject name, crh, full mark
            List<ExamSubjects> studentObtainedMarks) // students obtained marks 
        {
            var gpas = new List<GPADto>();
            decimal totalWeight = 0.0m;
            decimal totalcreditHour = 0.0m;

            foreach (var examSub in examSubjects)
            {
                // Check if student has marks for this subject
                var Subject = studentObtainedMarks
                    .FirstOrDefault(m => m.theoryCode == examSub.theoryCode);

                decimal obtainedMark = Subject?.theoryMark ?? 0m;

                // Full marks (avoid divide by zero)
                decimal fullThMark = examSub.theoryMark == 0 ? 1 : examSub.theoryMark;


                // If student absent → AB grade, 0 point
                (string ThGrade, decimal ThPoint) = (Subject!.theoryMark <= 0) ? ("AB", 0.0m) : AssignGrade(obtainedMark / fullThMark * 100);
                (string PrGrade, decimal PrPoint, decimal PrCrh) = ("AB", 0.0m, 0.0m);

                if (Subject!.practicalMark >0)
                {
                    decimal obtainedPr = Subject?.practicalMark ?? 0m;
                    decimal fullPrMark = examSub.practicalMark == 0 ? 1 : examSub.practicalMark;
                    (PrGrade, PrPoint) = (Subject!.practicalMark <= 0) ? ("AB", 0.0m) : AssignGrade(obtainedPr / fullPrMark * 100);
                    PrCrh = examSub.practicalCredit;
                }

                // Credit hours
                decimal credithr = examSub.theoryCredit + PrCrh;
                decimal weight = (examSub.theoryCredit * ThPoint) + (PrCrh * PrPoint);

                totalcreditHour += credithr;
                totalWeight += weight;

                // Final grade
                decimal finalGradeValue = credithr == 0 ? 0 : weight / credithr;
                (string Grade, decimal GradePoint) = (Subject == null) ? ("AB", 0.0m) : AssignGrade(finalGradeValue * 25);

                gpas.Add(new GPADto
                {
                    ThSub = new SubjectModel
                    {
                        SCode = examSub.theoryCode,
                        SName = examSub.theorySName ?? "Unknown",
                        LinkedPr= examSub.practicalCode,
                    },
                    PrSub = new SubjectModel
                    {
                        SCode = examSub.practicalCode,
                        SName = examSub.practicalSName ?? "Unknown"
                    },
                    ThGrade = (ThGrade, ThPoint, examSub.theoryCredit),
                    PrGrade = (PrGrade, PrPoint, examSub.practicalCredit),
                    TotalGrade = (ThGrade, GradePoint)
                });
            }



            var studentPoint = totalWeight / totalcreditHour;
            (string studentGrade, decimal stdPoint) = AssignGrade(studentPoint * 25);

            return (gpas, studentGrade, studentPoint);
        }


        // ----------------------
        // Helper methods
        // ----------------------

        private static (string Grade, decimal GradePoint) AssignGrade(decimal percentage)
        {
            return percentage switch
            {
                >= 90 => ("A+", 4.0m),
                >= 80 => ("A", 3.6m),
                >= 70 => ("B+", 3.2m),
                >= 60 => ("B", 2.8m),
                >= 50 => ("C+", 2.4m),
                >= 40 => ("C", 2.0m),
                >= 35 => ("D", 1.6m),
                _ => ("NG", 0.0m)
            };
        }
    }



}
