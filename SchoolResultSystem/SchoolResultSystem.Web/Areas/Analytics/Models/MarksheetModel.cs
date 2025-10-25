using Microsoft.CodeAnalysis.CSharp.Syntax;
using SchoolResultSystem.Web.Models;
using System.Collections.Generic;
using System.Linq;

namespace SchoolResultSystem.Web.Areas.Analytics.Models
{
    public class StdReq
    {
        public string NSN { get; set; } = null!;
    }


    public class Req
    {
        public string ClassName { get; set; } = null!;
        public int ExamId { get; set; }
    }

    public class MarkSheetDto
    {
        public string Schoolname { get; set; } = null!;
        public ExamModel Exam { get; set; } = new ExamModel();
        public StudentModel Student { get; set; } = new StudentModel();
        public List<GPADto> gpas { get; set; } = new List<GPADto>();
        public (string gpaL, decimal gpa) GPA { get; set; }
    }

    public class Subject
    {
        public SubjectModel Sub { get; set; } = new SubjectModel();
        public decimal ThMark { get; set; }
        public decimal PrMark { get; set; }
    }

    public class GPADto
    {
        public SubjectModel Sub { get; set; } = new SubjectModel();
        public (string Grade, decimal GradePoint) ThGrade { get; set; }
        public (string Grade, decimal GradePoint) PrGrade { get; set; }
        public (string Grade, decimal GradePoint) FinalGrade { get; set; }
        public decimal CreditHour { get; set; }
        public decimal Weight { get; set; }
    }
}
