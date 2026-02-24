using Microsoft.AspNetCore.Mvc;
using SchoolResultSystem.Web.Models;
using SchoolResultSystem.Web.Data;
using SchoolResultSystem.Web.Controllers;
using SchoolResultSystem.Web.Areas.Principal.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using SchoolResultSystem.Web.Filters;
using SchoolResultSystem.Web.Areas.Microservices.Models;
using System.Text;

[Area("Microservices")]
[AuthorizeUser("Admin")]
public class ExamRubrickController : SchoolDbController
{
    public ExamRubrickController(SchoolDbContext db) : base(db) { }

    [HttpPost]
    public async Task<IActionResult> CreateExam([FromBody] Exam ex)
    {

        try
        {
            var exists = await _db.ExamList.AnyAsync(e =>
                            e.ExamName == ex.ExamName &&
                             e.AcademicYear == ex.AcademicYear
                );

            if (exists)
            {
                return Ok(new { message = "Exam already exists" });
            }

            var examData = new ExamModel
            {
                ExamName = ex.ExamName,
                AcademicYear = ex.AcademicYear
            };

            _db.ExamList.Add(examData);
            await _db.SaveChangesAsync();

            return Ok(new { message = "Exam created. Now you can select the exam and fill rubric" });

        }
        catch
        {
            return Ok(new { message = "Could not create exam" });
        }
    }

    // select exams
    public async Task<IActionResult> GetExamList()
    {
        try
        {
            var examList = await _db.ExamList.Where(e => e.IsActive).ToListAsync();
            return Ok(examList);
        }
        catch
        {
            return Ok(new { message = "Some error occured" });
        }

    }

    [HttpPost]
    public async Task<IActionResult> CreateRubrick([FromBody] ExamSubjectsDTO data)
    {
        if (!ModelState.IsValid || data == null)
            return Json(new { message = "Invalid data type" });

        if (data.Subs == null || data.Subs.Count == 0)
            return Json(new { message = "No subject provided" });
        try
        {


            var sql = new StringBuilder();
            var parameters = new List<object>();

            sql.Append("INSERT INTO Marksheet (ExamId, SCode, CreditHour, FullMark) VALUES ");

            for (int i = 0; i < data.Subs.Count; i++)
            {
                sql.Append($"(@p{i}0, @p{i}1, @p{i}2, @p{i}3)");

                if (i < data.Subs.Count - 1)
                    sql.Append(",");

                parameters.Add(data.ExamId);
                parameters.Add(data.Subs[i].SCode);
                parameters.Add(data.Subs[i].CreditHour);
                parameters.Add(data.Subs[i].FullMark);
            }

            sql.Append(@"
                ON CONFLICT(ExamId, SCode)
                DO UPDATE SET
                CreditHour = excluded.CreditHour,
                FullMark = excluded.FullMark;
            ");

            await _db.Database.ExecuteSqlRawAsync(sql.ToString(), parameters.ToArray());

            return Json(new { message = "Rubrick saved successfully" });
        }
        catch
        {
            return Json(new { message = "Some error occured!" });
        }
    }


    // all subjects

    public async Task<IActionResult> GetAllSubjects()
    {
        var allSubjects = await _db.Subjects.Where(a => a.IsActive).ToListAsync();
        return Ok(allSubjects);
    }
}