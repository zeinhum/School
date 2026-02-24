using SchoolResultSystem.Web.Areas.Analytics.Models;
using SchoolResultSystem.Web.Models;
using SchoolResultSystem.Web.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http.HttpResults;

namespace SchoolResultSystem.Web.Areas.Analytics.Services
{
    public class ClassExamReportGenerator(SchoolDbContext db)
    {
        private readonly SchoolDbContext _db = db;

        public ClassExamReportDTO GenerateReport(string className, int examId = 1)
        {
            var report  = new ClassExamReportDTO();
            return report;
            
        }
    }
}
