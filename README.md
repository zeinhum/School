
```markdown
# 🏫 School Result System

A full-stack **School Result & Analytics Management System** built with **ASP.NET Core MVC**, **Entity Framework Core**, and **SQLite**, designed to automate student performance analysis and report generation with real-time data insights.

---

## 🚀 Overview

The **School Result System** streamlines how schools manage grading, analytics, and performance reports — featuring:
- Student marksheets with auto GPA computation  
- Class-level and subject analytics  
- Secure role-based access for Principals and Teachers  
- One-click database backup and replacement  
- Printable grade sheets with A4 formatting  

This project showcases backend architecture, system design, and data automation — crafted for clarity, performance, and scalability.

---

## 🧠 Core Features

| Feature | Description |
|----------|--------------|
| 🎓 **Student Marksheet** | Computes GPA and letter grades automatically. |
| 📊 **Analytics Dashboard** | Aggregates and visualizes class/subject trends. |
| 🧮 **Grade Calculation Engine** | GPA logic modularized for reusability. |
| 💾 **Database Management** | Built-in backup (download) and replace (upload) for `.db` files. |
| 🔐 **Role-Based Access** | Independent modules for Admin, Principal, and Teacher. |
| 🖨️ **Print-Optimized Reports** | A4 PDF-style layout with custom CSS. |
| ⚙️ **Modular MVC Architecture** | Divided into logical `Areas` for scalability. |

---

## 🏗️ Tech Stack

**Backend:**  
- ASP.NET Core MVC 8  
- Entity Framework Core  
- C#  
- SQLite  

**Frontend:**  
- HTML5, CSS3  
- Vanilla JavaScript (modular event-driven UI)  
- Partial Views (Razor templates)

**Tools:**  
- Visual Studio 2022  
- LINQ for query logic  
- Session & TempData management  
- Custom authorization middleware  

---

## 📂 Project Structure

```

SchoolResultSystem.Web/
│
├── Areas/
│   ├── Principal/
│   ├── Analytics/
│   └── Teachers/
│
├── wwwroot/
│   ├── css/
│   ├── js/
│   └── assets/
│
├── Controllers/
├── Data/
│   └── SchoolDbContext.cs
└── Models/

````

---

## ⚡ Key Implementations

### 💾 Database Backup
```csharp
public IActionResult CopyDb()
{
    var dbPath = Path.Combine(Directory.GetCurrentDirectory(), "SchoolDatabase.db");
    var backupPath = Path.Combine(Path.GetTempPath(), "SchoolDatabase_Backup.db");
    System.IO.File.Copy(dbPath, backupPath, true);
    return PhysicalFile(backupPath, "application/octet-stream", "SchoolDatabase_Backup.db");
}
````

### 📤 Replace Database (Upload)

```csharp
[HttpPost]
public IActionResult ReplaceDb(IFormFile file)
{
    var dbPath = Path.Combine(Directory.GetCurrentDirectory(), "SchoolDatabase.db");
    var tempPath = Path.Combine(Path.GetTempPath(), file.FileName);

    using (var stream = new FileStream(tempPath, FileMode.Create))
        file.CopyTo(stream);

    GC.Collect();
    GC.WaitForPendingFinalizers();

    System.IO.File.Delete(dbPath);
    System.IO.File.Move(tempPath, dbPath);

    return Ok("Database replaced successfully. Restart required.");
}
```

---

## 🧩 Analytics Workflow

1. User selects **Target** (Student / Subject / Teacher).
2. System dynamically populates actions.
3. Data is sent via fetch() POST to controller.
4. GPA and grade calculations run server-side.
5. Rendered view is returned without page reload.

---

## 🧮 GPA Calculation Logic

Each student’s GPA is calculated by combining all subject grade points using weighted average:

```
GPA = Σ (GradePoint × CreditHours) / Σ (CreditHours)
```

Subjects marked “NG” (No Grade) are skipped during computation.

---

## 💡 How to Run Locally

1. **Clone the repository**

   ```bash
   git clone https://github.com/zeinhum/school.git
   cd school/SchoolResultSystem.Web
   ```

2. **Run the application**

   ```bash
   dotnet run
   ```

3. **Open in browser**

   ```
   http://localhost:5062
   ```

SQLite database `SchoolDatabase.db` is included for quick testing.

---

## 🧩 Future Enhancements

* 📈 Visual analytics with Recharts.js
* ☁️ Cloud database migration (Azure / PostgreSQL)
* 🔐 JWT authentication for API access
* 📨 Result notifications via email or SMS

---

## 👤 Author

**Zein Hum**
💻 Full Stack Developer | ASP.NET | React | SQL | Data Analytics
📧 [zeinhumn@gmail.com](mailto:zeinhumn@gmail.com)
🌐 [github.com/zeinhum](https://github.com/zeinhum)

> “Data-driven automation isn’t just efficiency — it’s how education scales.”

---

## 🧾 License

This project is open-sourced under the [MIT License](LICENSE).

```

---
