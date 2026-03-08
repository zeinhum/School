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
    // 1. Basic Guard Clauses
    if (data == null || data.Subs == null || data.Subs.Count == 0)
    {
        return BadRequest(new { message = "No valid subject data provided." });
    }

    // 2. Safety Limit (SQLite Parameter Limit is usually 999 or 32766)
    // 4 parameters per subject. 200 subjects = 800 parameters. Safe.
    if (data.Subs.Count > 200) 
    {
        return BadRequest(new { message = "Too many subjects in a single request. Please batch your data." });
    }

    // 3. Start a Transaction for Atomic operations
    using var transaction = await _db.Database.BeginTransactionAsync();

    try
    {
        var sql = new StringBuilder();
        var parameters = new List<object>();

        sql.Append("INSERT INTO ExamRubrick (ExamId, SCode, CreditHour, FullMark) VALUES ");

        for (int i = 0; i < data.Subs.Count; i++)
        {
            var sub = data.Subs[i];
            
            // Validate row data before adding to SQL
            if (string.IsNullOrWhiteSpace(sub.SCode) || sub.FullMark <= 0) continue;

            int start = i * 4;
            sql.Append($"(@p{start}, @p{start + 1}, @p{start + 2}, @p{start + 3})");

            if (i < data.Subs.Count - 1) sql.Append(",");

            parameters.Add(data.ExamId);
            parameters.Add(sub.SCode);
            parameters.Add(sub.CreditHour);
            parameters.Add(sub.FullMark);
        }

        sql.Append(@"
            ON CONFLICT(ExamId, SCode)
            DO UPDATE SET
            CreditHour = excluded.CreditHour,
            FullMark = excluded.FullMark;");

        // 4. Execute the command
        await _db.Database.ExecuteSqlRawAsync(sql.ToString(), parameters.ToArray());

        // 5. Commit if everything succeeded
        await transaction.CommitAsync();
        
        return Ok(new { message = "Rubrick saved successfully" });
    }
    catch (Exception)
    {
        // 6. Rollback on failure to prevent partial data save
        await transaction.RollbackAsync();


        return Ok( new { message = "An internal error occurred while saving the rubrick." });
    }
}


    // all subjects

    public async Task<IActionResult> GetAllSubjects()
    {
        var allSubjects = await _db.Subjects.Where(a => a.IsActive).ToListAsync();
        return Ok(allSubjects);
    }

    [HttpPost]
    public async Task<IActionResult> ClassSubs([FromBody] SubId Id)
    {
        try
        {
            var subs = await _db.CST.Where(c=>c.ClassId==Id.Id)
            .Include(s=>s.Subject)
            .Select(s => new
            {
                sCode = s.SCode,
                sName = s.Subject.SName
            }).ToListAsync();
            return Ok(subs);
        }
        catch
        {
            return Json(new{message="subs not found"});
        }
    }

}