<!doctype html>
<html lang="en">
<head>
  <meta charset="utf-8" />
  <meta name="viewport" content="width=device-width,initial-scale=1" />
  <title>School Result System — Readme</title>
  <meta name="description" content="School Result & Analytics Management System — ASP.NET Core MVC, EF Core, SQLite" />
  <style>
    :root{
      --bg:#0f1724; /* deep midnight */
      --card:#0b1220;
      --muted:#9aa4b2;
      --accent:#7c5cff;
      --accent-2:#00d4ff;
      --glass: rgba(255,255,255,0.03);
      --success:#36d399;
      --danger:#ff6b6b;
      --mono: "SFMono-Regular", Consolas, "Liberation Mono", Menlo, monospace;
      --radius:14px;
      color-scheme: dark;
    }
    *{box-sizing:border-box}
    html,body{height:100%;margin:0;font-family:Inter,ui-sans-serif,system-ui,-apple-system,"Segoe UI",Roboto,"Helvetica Neue",Arial; background: linear-gradient(180deg,var(--bg) 0%, #071023 100%); color:#e6eef8;}

    .container{max-width:1060px;margin:40px auto;padding:28px;background:linear-gradient(180deg,rgba(255,255,255,0.02),rgba(255,255,255,0.01));border-radius:20px;box-shadow:0 8px 30px rgba(2,6,23,0.7);border:1px solid rgba(255,255,255,0.03)}

    header{display:flex;align-items:center;gap:18px}
    .logo{width:72px;height:72px;background:linear-gradient(135deg,var(--accent),var(--accent-2));border-radius:14px;display:flex;align-items:center;justify-content:center;font-weight:700;font-size:20px;color:white;box-shadow:0 6px 20px rgba(92,73,255,0.18)}
    h1{margin:0;font-size:22px;letter-spacing:-0.2px}
    p.lead{margin:6px 0 0;color:var(--muted)}

    .grid{display:grid;grid-template-columns:1fr 320px;gap:22px;margin-top:24px}

    .card{background:var(--card);padding:20px;border-radius:14px;border:1px solid rgba(255,255,255,0.02)}
    .section-title{font-size:14px;color:var(--accent);text-transform:uppercase;letter-spacing:1px;margin-bottom:12px}

    /* Feature list */
    .features{display:grid;grid-template-columns:repeat(2,1fr);gap:12px}
    .feature{display:flex;gap:12px;align-items:flex-start;padding:10px;background:var(--glass);border-radius:10px}
    .icon{width:44px;height:44px;border-radius:10px;background:linear-gradient(180deg,rgba(255,255,255,0.02), rgba(255,255,255,0.01));display:grid;place-items:center;font-weight:700}
    .f-title{font-weight:600}
    .f-desc{color:var(--muted);font-size:13px}

    /* Tech stack table */
    table{width:100%;border-collapse:collapse;margin-top:8px}
    td,th{padding:10px 8px;text-align:left;border-bottom:1px dashed rgba(255,255,255,0.03)}
    th{color:var(--muted);font-weight:600;font-size:13px}

    /* Code block */
    pre{background:#061022;padding:14px;border-radius:10px;overflow:auto;border:1px solid rgba(255,255,255,0.03);font-family:var(--mono);font-size:13px}

    /* Columns in right rail */
    .right .meta{display:flex;flex-direction:column;gap:10px}
    .pill{display:inline-block;padding:8px 12px;border-radius:999px;background:linear-gradient(90deg, rgba(255,255,255,0.02), rgba(255,255,255,0.01));font-weight:600}

    footer{margin-top:20px;display:flex;justify-content:space-between;align-items:center;color:var(--muted);font-size:13px}

    /* Responsive */
    @media (max-width:980px){.grid{grid-template-columns:1fr;}.features{grid-template-columns:1fr}} 

    /* Print-friendly for A4 */
    @media print{
      body{background:white;color:black}
      .container{box-shadow:none;border:none;background:transparent}
    }

  </style>
</head>
<body>
  <main class="container">
    <header>
      <div class="logo">SRS</div>
      <div>
        <h1>School Result System</h1>
        <p class="lead">A full‑stack School Result & Analytics Management System — ASP.NET Core MVC • EF Core • SQLite</p>
      </div>
    </header>

    <div class="grid">
      <section>
        <article class="card">
          <div class="section-title">Overview</div>
          <p>Streamlines grading, analytics and reporting with:</p>
          <ul style="margin-top:12px;line-height:1.6;color:var(--muted)">
            <li>Student marksheets with auto GPA computation</li>
            <li>Class-level & subject analytics</li>
            <li>Role-based access for Principals and Teachers</li>
            <li>One-click DB backup & replace, printable grade sheets</li>
          </ul>

          <hr style="margin:18px 0;border:none;border-top:1px dashed rgba(255,255,255,0.03)" />

          <div class="section-title">Core Features</div>
          <div class="features">
            <div class="feature">
              <div class="icon">🎓</div>
              <div>
                <div class="f-title">Student Marksheet</div>
                <div class="f-desc">Computes GPA & letter grades automatically.</div>
              </div>
            </div>

            <div class="feature">
              <div class="icon">📊</div>
              <div>
                <div class="f-title">Analytics Dashboard</div>
                <div class="f-desc">Aggregates and visualizes class/subject trends.</div>
              </div>
            </div>

            <div class="feature">
              <div class="icon">🧮</div>
              <div>
                <div class="f-title">Grade Engine</div>
                <div class="f-desc">Modular GPA logic for reuse across modules.</div>
              </div>
            </div>

            <div class="feature">
              <div class="icon">🖨️</div>
              <div>
                <div class="f-title">Print-Optimized</div>
                <div class="f-desc">A4 PDF-style layout with custom CSS for clean prints.</div>
              </div>
            </div>

          </div>

          <hr style="margin:18px 0;border:none;border-top:1px dashed rgba(255,255,255,0.03)" />

          <div class="section-title">Tech Stack</div>
          <table>
            <tr><th>Backend</th><td>ASP.NET Core MVC 8, C#, Entity Framework Core, SQLite</td></tr>
            <tr><th>Frontend</th><td>HTML5, CSS3, Vanilla JavaScript, Razor Partial Views</td></tr>
            <tr><th>Tools</th><td>Visual Studio 2022, LINQ, Session & TempData</td></tr>
            <tr><th>Architecture</th><td>Modular MVC with Areas (Principal / Analytics / Teachers)</td></tr>
          </table>

        </article>

        <article class="card" style="margin-top:16px">
          <div class="section-title">Key Implementations (snippets)</div>

          <div style="margin-top:6px">
            <div style="font-weight:700;margin-bottom:8px">Database Backup</div>
            <pre><code>// Copy DB and return as download (C#)
public IActionResult CopyDb()
{
    var dbPath = Path.Combine(Directory.GetCurrentDirectory(), "SchoolDatabase.db");
    var backupPath = Path.Combine(Path.GetTempPath(), "SchoolDatabase_Backup.db");
    System.IO.File.Copy(dbPath, backupPath, true);
    return PhysicalFile(backupPath, "application/octet-stream", "SchoolDatabase_Backup.db");
}
</code></pre>

            <div style="font-weight:700;margin:10px 0 8px">Replace Database (Upload)</div>
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

          </div>
        </article>

        <article class="card" style="margin-top:16px">
          <div class="section-title">GPA Calculation</div>
          <p style="color:var(--muted)">Weighted GPA formula used across the system:</p>
          <pre><code>GPA = Σ (GradePoint × CreditHours) / Σ (CreditHours)

Subjects marked "NG" (No Grade) are skipped during computation.
</code></pre>
        </article>

        <article class="card" style="margin-top:16px">
          <div class="section-title">How to Run Locally</div>
          <pre><code>git clone https://github.com/zeinhum/school.git
cd school/SchoolResultSystem.Web
dotnet run
# Open: http://localhost:5062
</code></pre>
          <p style="color:var(--muted);margin-top:8px">SQLite database <code>SchoolDatabase.db</code> is included for quick testing.</p>
        </article>

      </section>

      <aside class="right">
        <div class="card">
          <div class="section-title">Project Summary</div>
          <p style="color:var(--muted)">A complete result management system built for clarity, performance and school-grade reporting automation. Organized by areas and modular services for role-based workflows.</p>

          <div style="margin-top:12px" class="meta">
            <div><span class="pill">Roles: Admin · Principal · Teacher</span></div>
            <div style="margin-top:8px"><span class="pill">DB: SQLite</span></div>
            <div style="margin-top:8px"><span class="pill">Print: A4-ready</span></div>
          </div>

          <hr style="margin:12px 0;border:none;border-top:1px dashed rgba(255,255,255,0.03)" />

          <div class="section-title">Future Enhancements</div>
          <ul style="color:var(--muted)">
            <li>Visual analytics with Recharts.js</li>
            <li>Cloud DB migration (Azure / PostgreSQL)</li>
            <li>JWT authentication for API</li>
            <li>Result notifications (email / SMS)</li>
          </ul>

          <hr style="margin:12px 0;border:none;border-top:1px dashed rgba(255,255,255,0.03)" />

          <div class="section-title">Author</div>
          <p style="margin:0">Zein Hum — Full Stack Developer</p>
          <p style="color:var(--muted);margin-top:6px;font-size:13px">📧 zeinhumn@gmail.com · 🌐 github.com/zeinhum</p>

        </div>

        <div class="card" style="margin-top:14px">
          <div style="display:flex;gap:10px;align-items:center;justify-content:space-between">
            <div>
              <div style="font-weight:700">License</div>
              <div style="color:var(--muted);font-size:13px">MIT License — open source</div>
            </div>
            <div style="text-align:right;color:var(--muted);font-size:12px">Version 1.0</div>
          </div>
        </div>

      </aside>
    </div>

    <footer>
      <div>“Data-driven automation isn’t just efficiency — it’s how education scales.”</div>
      <div>Made with ❤️ by Zein Hum</div>
    </footer>
  </main>
</body>
</html>
