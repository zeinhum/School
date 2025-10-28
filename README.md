<!DOCTYPE html>
<html lang="en">
<head>
<meta charset="UTF-8">
<meta name="viewport" content="width=device-width, initial-scale=1.0">
<title>School Result System</title>
<style>
  body {
    font-family: "Segoe UI", Roboto, Helvetica, Arial, sans-serif;
    line-height: 1.7;
    color: #2d2d2d;
    background: #fafafa;
    margin: 2rem auto;
    max-width: 900px;
    padding: 2rem;
  }

  h1, h2, h3 {
    color: #1f4e79;
  }

  h1 {
    border-bottom: 3px solid #1f4e79;
    padding-bottom: 0.4rem;
  }

  code {
    background: #f4f4f4;
    border-radius: 4px;
    padding: 0.2rem 0.4rem;
    font-size: 0.95em;
  }

  pre {
    background: #272822;
    color: #f8f8f2;
    padding: 1rem;
    border-radius: 8px;
    overflow-x: auto;
  }

  table {
    width: 100%;
    border-collapse: collapse;
    margin: 1rem 0;
    font-size: 0.95em;
  }

  th, td {
    border: 1px solid #ddd;
    padding: 0.7rem;
    text-align: left;
  }

  th {
    background: #1f4e79;
    color: white;
  }

  tr:nth-child(even) {
    background: #f9f9f9;
  }

  .tag {
    background: #e1ecf4;
    color: #0366d6;
    border-radius: 12px;
    padding: 0.2rem 0.6rem;
    font-size: 0.85em;
    margin-right: 0.3rem;
  }

  .quote {
    background: #eef6fc;
    border-left: 4px solid #1f4e79;
    padding: 1rem;
    margin: 1.5rem 0;
    font-style: italic;
  }

  footer {
    text-align: center;
    margin-top: 2rem;
    font-size: 0.9em;
    color: #666;
  }

  a {
    color: #0366d6;
    text-decoration: none;
  }

  a:hover {
    text-decoration: underline;
  }
</style>
</head>
<body>

<h1>🏫 School Result System</h1>
<p>A full-stack <strong>School Result & Analytics Management System</strong> built with <strong>ASP.NET Core MVC</strong>, <strong>Entity Framework Core</strong>, and <strong>SQLite</strong>, designed to automate student performance analysis and report generation with real-time data insights.</p>

<hr>

<h2>🚀 Overview</h2>
<p>The <strong>School Result System</strong> streamlines how schools manage grading, analytics, and performance reports — featuring:</p>
<ul>
  <li>Student marksheets with auto GPA computation</li>
  <li>Class-level and subject analytics</li>
  <li>Secure role-based access for Principals and Teachers</li>
  <li>One-click database backup and replacement</li>
  <li>Printable grade sheets with A4 formatting</li>
</ul>
<p>This project showcases backend architecture, system design, and data automation — crafted for clarity, performance, and scalability.</p>

<hr>

<h2>🧠 Core Features</h2>
<table>
  <tr><th>Feature</th><th>Description</th></tr>
  <tr><td>🎓 <strong>Student Marksheet</strong></td><td>Computes GPA and letter grades automatically.</td></tr>
  <tr><td>📊 <strong>Analytics Dashboard</strong></td><td>Aggregates and visualizes class/subject trends.</td></tr>
  <tr><td>🧮 <strong>Grade Calculation Engine</strong></td><td>GPA logic modularized for reusability.</td></tr>
  <tr><td>💾 <strong>Database Management</strong></td><td>Backup (download) and replace (upload) for <code>.db</code> files.</td></tr>
  <tr><td>🔐 <strong>Role-Based Access</strong></td><td>Independent modules for Admin, Principal, and Teacher.</td></tr>
  <tr><td>🖨️ <strong>Print-Optimized Reports</strong></td><td>A4 PDF-style layout with custom CSS.</td></tr>
  <tr><td>⚙️ <strong>Modular MVC Architecture</strong></td><td>Organized by <code>Areas</code> for scalability.</td></tr>
</table>

<hr>

<h2>🏗️ Tech Stack</h2>
<ul>
  <li><strong>Backend:</strong> ASP.NET Core MVC 8, Entity Framework Core, C#, SQLite</li>
  <li><strong>Frontend:</strong> HTML5, CSS3, Vanilla JavaScript, Razor Partial Views</li>
  <li><strong>Tools:</strong> Visual Studio 2022, LINQ, TempData & Session, Custom Middleware</li>
</ul>

<hr>

<h2>📂 Project Structure</h2>
<pre><code>SchoolResultSystem.Web/
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
</code></pre>

<hr>

<h2>⚡ Key Implementations</h2>

<h3>💾 Database Backup</h3>
<pre><code>public IActionResult CopyDb()
{
    var dbPath = Path.Combine(Directory.GetCurrentDirectory(), "SchoolDatabase.db");
    var backupPath = Path.Combine(Path.GetTempPath(), "SchoolDatabase_Backup.db");
    System.IO.File.Copy(dbPath, backupPath, true);
    return PhysicalFile(backupPath, "application/octet-stream", "SchoolDatabase_Backup.db");
}
</code></pre>

<h3>📤 Replace Database (Upload)</h3>
<pre><code>[HttpPost]
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
</code></pre>

<h3>🧩 Analytics Workflow</h3>
<ol>
  <li>User selects Target (Student / Subject / Teacher).</li>
  <li>System dynamically populates actions.</li>
  <li>Data is sent via <code>fetch()</code> POST to controller.</li>
  <li>GPA and grade calculations run server-side.</li>
  <li>Rendered view is returned without page reload.</li>
</ol>

<h3>🧮 GPA Calculation Logic</h3>
<p>Each student’s GPA is calculated by combining all subject grade points using weighted average:</p>
<pre><code>GPA = Σ (GradePoint × CreditHours) / Σ (CreditHours)</code></pre>
<p>Subjects marked “NG” (No Grade) are skipped during computation.</p>

<hr>

<h2>💡 How to Run Locally</h2>
<ol>
  <li><strong>Clone the repository:</strong></li>
  <pre><code>git clone https://github.com/zeinhum/school.git
cd school/SchoolResultSystem.Web</code></pre>
  <li><strong>Run the application:</strong></li>
  <pre><code>dotnet run</code></pre>
  <li><strong>Open in browser:</strong></li>
  <pre><code>http://localhost:5062</code></pre>
  <li>SQLite database <code>SchoolDatabase.db</code> is included for quick testing.</li>
</ol>

<hr>

<h2>🧩 Future Enhancements</h2>
<ul>
  <li>📈 Visual analytics with Recharts.js</li>
  <li>☁️ Cloud database migration (Azure / PostgreSQL)</li>
  <li>🔐 JWT authentication for API access</li>
  <li>📨 Result notifications via email or SMS</li>
</ul>

<hr>

<h2>👤 Author</h2>
<p><strong>Zein Hum</strong><br>
💻 Full Stack Developer | ASP.NET | React | SQL | Data Analytics<br>
📧 <a href="mailto:zeinhumn@gmail.com">zeinhumn@gmail.com</a><br>
🌐 <a href="https://github.com/zeinhum">github.com/zeinhum</a></p>

<div class="quote">“Data-driven automation isn’t just efficiency — it’s how education scales.”</div>

<footer>
  🧾 Licensed under the <strong>MIT License</strong>.
</footer>

</body>
</html>
