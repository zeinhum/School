using Microsoft.AspNetCore.Mvc;
using SchoolResultSystem.Web.Models;
using SchoolResultSystem.Web.Data;
using Microsoft.EntityFrameworkCore;
using SchoolResultSystem.Web.Areas.Principal.Models;
using System.Security.Cryptography.X509Certificates;
using SchoolResultSystem.Web.Filters;
using Microsoft.Data.Sqlite;


[Area("Principal")]
[AuthorizeUser("Admin")]
public class SetupPrincipal : Controller
{
    private readonly SchoolDbContext _db;
    private readonly IWebHostEnvironment _hostEnvironment;

    public SetupPrincipal(SchoolDbContext db, IWebHostEnvironment hostEnvironment)
    {
        _db = db;
        _hostEnvironment = hostEnvironment;
    }


    // create shcool's information
    public IActionResult SetupSchool()
    {
        return View();
    }

    // Create Class
    public IActionResult CreateClass()
    {
        return View("CreateClass");
    }



    // create subject
    public IActionResult CreateSubject()
    {
        return View("CreateSubject");
    }





    [HttpPost]
    public IActionResult SavePrincipal(UserModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("SetupPrincipalView");
        }
        try
        {
            // remove old admin if exists
            var oldAdmins = _db.Users.Where(u => u.Role == "Admin").ToList();
            _db.Users.RemoveRange(oldAdmins);

            // enforce admin role
            model.Role = "Admin";
            _db.Users.Add(model);

            _db.SaveChanges();

            TempData["success"] = "Principal account set successfully!";
            return RedirectToAction("SetupSchool");
        }
        catch (Exception)
        {
            ViewBag.error = "Some error occured. Try again.";
            return View("SetupPrincipalView");
        }
    }

   [HttpPost]
public async Task<IActionResult> SaveSchool(SchoolInfoModel model, IFormFile logoFile)
{
    try
    {
        // 1. Handle File Upload (Save logo)
        if (logoFile != null)
        {
            // Define the path to the wwwroot/images folder
            string wwwRootPath = _hostEnvironment.WebRootPath;
            string imagePath = Path.Combine(wwwRootPath, "image");

            // Ensure the directory exists
            if (!Directory.Exists(imagePath))
            {
                Directory.CreateDirectory(imagePath);
            }

            // Get the file extension (e.g., .png, .jpg)
            string extension = Path.GetExtension(logoFile.FileName);
            
            // Define the new file name: "logo" + extension
            string fileName = "logo" + extension;
            string filePath = Path.Combine(imagePath, fileName);

            // Save the file to the physical path
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await logoFile.CopyToAsync(fileStream);
            }
        }

        // 2. Handle Database Operations
        // optional: clear existing school info
        _db.SchoolInfo.RemoveRange(_db.SchoolInfo);
        _db.SchoolInfo.Add(model);
        await _db.SaveChangesAsync(); // Use async SaveChanges

        TempData["success"] = "School info saved!";
        return RedirectToAction("Index", "PrincipalDashboard");
    }
    catch (Exception ex)
    {
        ViewBag.error = "Error occurred. Try again. Details: " + ex.Message;
        // Correct the ViewBag property name to match what is used in the view
        ViewBag.ErrorMessage = ViewBag.error; 
        return View("SetupSchoolView", model); // Pass model back to view for form persistence
    }
}


    [HttpPost]
    public IActionResult SaveClass([FromBody] List<ClassModel> data)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "No classes provided." });
            }

            var existingNames = _db.Classes
                .Select(c => c.ClassName)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var newClasses = new List<ClassModel>();
            var skipped = new List<string>();

            foreach (var cl in data)
            {
                if (existingNames.Contains(cl.ClassName))
                {
                    skipped.Add(cl.ClassName);
                }
                else
                {
                    newClasses.Add(new ClassModel { ClassName = cl.ClassName });
                }
            }

            if (newClasses.Any())
            {
                _db.Classes.AddRange(newClasses);
                _db.SaveChanges();
            }

            return Json(new
            {
                success = true,
                added = newClasses.Count,
                message = $"{newClasses.Count} classes added. Skipped: {skipped.Count}"
            });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "Error creating classes: " + ex.Message });
        }
    }




    [HttpPost]
    public IActionResult SaveSubject([FromBody] List<SubjectModel> data)
    {
        try
        {
            if (data == null || data.Count == 0)
            {
                return Json(new { success = false, message = "No subjects provided." });
            }

            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Invalid data format." });
            }

            // 1️⃣ Get all existing subject codes from the database
            var existingCodes = _db.Subjects
                .Select(s => s.SCode)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var newSubjects = new List<SubjectModel>();
            var skipped = new List<string>();

            // 2️⃣ Separate new ones from existing
            foreach (var sub in data)
            {
                if (string.IsNullOrWhiteSpace(sub.SCode) || string.IsNullOrWhiteSpace(sub.SName))
                    continue; // skip invalid rows

                if (existingCodes.Contains(sub.SCode))
                {
                    skipped.Add(sub.SCode);
                }
                else
                {
                    newSubjects.Add(new SubjectModel
                    {
                        SCode = sub.SCode.Trim(),
                        SName = sub.SName.Trim(),
                    });
                }
            }

            if (newSubjects.Any())
            {
                _db.Subjects.AddRange(newSubjects);
                _db.SaveChanges();
            }

            return Json(new
            {
                success = true,
                added = newSubjects.Count,
                message = $"{newSubjects.Count} subject(s) added. Skipped {skipped.Count} duplicate(s)."
            });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "Error saving subjects: " + ex.Message });
        }
    }



    // Create teacher's account
    public IActionResult CreateTeacher()
    {
        return View("CreateTeacher");
    }

    [HttpPost]
    public IActionResult SaveTeacher(UserModel model)
    {
        try
        {

            bool userExists = _db.Users.Any(u => u.UserName == model.UserName);
            if (userExists)
            {
                ViewBag.error = "Teacher already exists. Try a different one.";
                return View("CreateTeacher");
            }
            model.Role = "Teacher";
            model.IsActive = true;
            _db.Users.Add(model);
            _db.SaveChanges();
            ViewBag.success = "Account created";
            return View("CreateTeacher");
        }
        catch (Exception)
        {
            ViewBag.error = "Account not created. Try again.";
            return View("CreateTeacher");
        }
    }

    [HttpDelete]
    public IActionResult DeleteTeacher(string id)
    {
        try
        {
            var teacherToDelete = _db.Users.FirstOrDefault(u => u.TeacherId == id);

            if (teacherToDelete == null)
            {
                return NotFound(new { success = false, message = $"Teacher with ID {id} not found." });
            }
            // Soft delete: mark inactive
            teacherToDelete.IsActive = false;

            _db.Users.Update(teacherToDelete);
            _db.SaveChanges();

            return Ok(new { success = true, message = "Teacher deactivated successfully." });
        }
        catch (Exception)
        {
            return StatusCode(500, new { success = false, message = "An error occurred during deletion." });
        }
    }

    // create exams
    public IActionResult CreateExam()
    {
        var allSubjects = _db.Subjects.Where(a => a.IsActive).ToList();
        return View(allSubjects);
    }
    //save Exam
    [HttpPost]
    public IActionResult SaveExam([FromBody] SaveExam data)
    {
        // --- Initial Validation ---
        if (!ModelState.IsValid || data == null)
        {
            return Json(new { success = false, message = "Invalid data type" });
        }
        if (data.SubjectMarks.Count == 0)
        {
            return Json(new { success = false, message = "no subject provided" });
        }

        try
        {
            // Try to find an existing ExamId for given name & year
            var matchedExam = _db.Exams
                .Where(e => e.ExamName == data.ExamName && e.AcademicYear == data.AcademicYear)
                .GroupBy(e => e.ExamId)
                .Select(g => new
                {
                    ExamId = g.Key,
                    SCodes = g.Select(x => x.SCode).ToList()
                })
                .FirstOrDefault();

            int exId;
            HashSet<string> existingSCodes = new();

            if (matchedExam != null)
            {
                // Found a match → use its ID and SCodes
                exId = matchedExam.ExamId;
                existingSCodes = matchedExam.SCodes.ToHashSet();
            }
            else
            {
                // No exact match → just get the latest ExamId
                exId = _db.Exams
                    .OrderByDescending(e => e.ExamId)
                    .Select(e => e.ExamId)
                    .FirstOrDefault();
                exId++;
            }
            int addedCount = 0;
            int skippedCount = 0;
            var newExams = new List<ExamModel>();

            // 3. Process the incoming data
            foreach (var ex in data.SubjectMarks)
            {
                // Check for a duplicate using the fast HashSet lookup
                if (existingSCodes.Contains(ex.SCode))
                {
                    skippedCount++;
                    continue; // skip duplicates
                }

                // Create new ExamModel
                var examModel = new ExamModel
                {
                    ExamId=exId,
                    ExamName = data.ExamName,
                    AcademicYear = data.AcademicYear,
                    SCode = ex.SCode,
                    ThMark = ex.ThMark,
                    PrMark = ex.PrMark,
                    ThCrh = ex.ThCrh,
                    PrCrh = ex.PrCrh
                };

                newExams.Add(examModel);
                addedCount++;
            }

            // 4. Save new records
            if (newExams.Count > 0)
            {
                _db.Exams.AddRange(newExams);
                _db.SaveChanges();
            }

            return Json(new
            {
                success = true,
                message = $"Exams created for {addedCount} subjects, skipped {skippedCount} duplicates."
            });
        }
        catch (Exception ex)
        {
            // Log the exception details here for production use (e.g., using ILogger)
            return Json(new { success = false, message = $"Error saving exams: {ex.Message}" });
        }
    }

    public IActionResult Help()
    {
        return View();
    }
    public IActionResult CopyDb()
    {
        var dbPath = Path.Combine(Directory.GetCurrentDirectory(), "SchoolDatabase.db");
        var backupPath = Path.Combine(Path.GetTempPath(), $"SchoolDatabase_Backup.db");

        // 1️⃣ Perform SQLite-safe backup
        using (var connection = new SqliteConnection($"Data Source={dbPath}"))
        using (var destination = new SqliteConnection($"Data Source={backupPath}"))
        {
            connection.Open();
            destination.Open();

            connection.BackupDatabase(destination);
        } // ✅ connections are fully closed and disposed here

        // 2️⃣ Now safely read file after all handles are released
        byte[] fileBytes;
        using (var stream = new FileStream(backupPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        using (var ms = new MemoryStream())
        {
            stream.CopyTo(ms);
            fileBytes = ms.ToArray();
        }

        // 3️⃣ Return file for download (browser Save As)
        var fileName = Path.GetFileName(backupPath);
        return File(fileBytes, "application/octet-stream", fileName);
    }

    [HttpPost]
    public IActionResult ReplaceDb(IFormFile file, [FromServices] SchoolDbContext db)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file selected.");

        if (!file.FileName.EndsWith(".db", StringComparison.OrdinalIgnoreCase))
            return BadRequest("Invalid file type. Only .db files are allowed.");

        string dbPath = Path.Combine(Directory.GetCurrentDirectory(), "SchoolDatabase.db");
        string tempPath = Path.Combine(Path.GetTempPath(), "Uploaded_SchoolDatabase.db");

        try
        {
            // Step 1️⃣: Save uploaded file temporarily
            using (var stream = new FileStream(tempPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            // Step 2️⃣: Close and dispose DB connections
            db.Database.CloseConnection();
            db.Dispose();

            // Step 3️⃣: Clear connection pools
            Microsoft.Data.Sqlite.SqliteConnection.ClearAllPools();

            // Step 4️⃣: Replace DB
            if (System.IO.File.Exists(dbPath))
            {
                System.IO.File.Delete(dbPath);
            }

            System.IO.File.Move(tempPath, dbPath);

            return Ok("Database replaced successfully.");
        }
        catch (IOException ioEx)
        {
            return StatusCode(500, $"File access error: {ioEx.Message}. Make sure the database isn't open in another process.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Unexpected error: {ex.Message}");
        }
    }

}
