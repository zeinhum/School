using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolResultSystem.Web.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Classes",
                columns: table => new
                {
                    ClassId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClassGrade = table.Column<string>(type: "TEXT", nullable: false),
                    ClassName = table.Column<string>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.ClassId);
                });

            migrationBuilder.CreateTable(
                name: "ExamList",
                columns: table => new
                {
                    ExamId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AcademicYear = table.Column<int>(type: "INTEGER", nullable: false),
                    ExamName = table.Column<string>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamList", x => x.ExamId);
                });

            migrationBuilder.CreateTable(
                name: "LeaveDates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Weekday = table.Column<bool>(type: "INTEGER", nullable: false),
                    Holiday = table.Column<bool>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveDates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SchoolInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    Phone = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    NSN = table.Column<string>(type: "TEXT", nullable: false),
                    RegistrationN = table.Column<string>(type: "TEXT", nullable: false),
                    StudentName = table.Column<string>(type: "TEXT", nullable: false),
                    D_O_B = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    AdmissionDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.NSN);
                });

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    SCode = table.Column<string>(type: "TEXT", nullable: false),
                    SName = table.Column<string>(type: "TEXT", nullable: false),
                    SType = table.Column<string>(type: "TEXT", nullable: false),
                    LinkedPr = table.Column<string>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.SCode);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    FullName = table.Column<string>(type: "TEXT", nullable: false),
                    Role = table.Column<string>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "ClassStudent",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NSN = table.Column<string>(type: "TEXT", nullable: false),
                    ClassId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassStudent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassStudent_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "ClassId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassStudent_Students_NSN",
                        column: x => x.NSN,
                        principalTable: "Students",
                        principalColumn: "NSN",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClassSubject",
                columns: table => new
                {
                    rowId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClassId = table.Column<int>(type: "INTEGER", nullable: false),
                    SCode = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassSubject", x => x.rowId);
                    table.ForeignKey(
                        name: "FK_ClassSubject_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "ClassId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassSubject_Subjects_SCode",
                        column: x => x.SCode,
                        principalTable: "Subjects",
                        principalColumn: "SCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExamRubrick",
                columns: table => new
                {
                    rubrick = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ExamId = table.Column<int>(type: "INTEGER", nullable: false),
                    SCode = table.Column<string>(type: "TEXT", nullable: false),
                    CreditHour = table.Column<decimal>(type: "TEXT", nullable: false),
                    FullMark = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamRubrick", x => x.rubrick);
                    table.ForeignKey(
                        name: "FK_ExamRubrick_ExamList_ExamId",
                        column: x => x.ExamId,
                        principalTable: "ExamList",
                        principalColumn: "ExamId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExamRubrick_Subjects_SCode",
                        column: x => x.SCode,
                        principalTable: "Subjects",
                        principalColumn: "SCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Marksheet",
                columns: table => new
                {
                    MarksheetId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ExamId = table.Column<int>(type: "INTEGER", nullable: false),
                    NSN = table.Column<string>(type: "TEXT", nullable: false),
                    SCode = table.Column<string>(type: "TEXT", nullable: false),
                    Mark = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Marksheet", x => x.MarksheetId);
                    table.ForeignKey(
                        name: "FK_Marksheet_ExamList_ExamId",
                        column: x => x.ExamId,
                        principalTable: "ExamList",
                        principalColumn: "ExamId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Marksheet_Students_NSN",
                        column: x => x.NSN,
                        principalTable: "Students",
                        principalColumn: "NSN",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Marksheet_Subjects_SCode",
                        column: x => x.SCode,
                        principalTable: "Subjects",
                        principalColumn: "SCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CST",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClassId = table.Column<int>(type: "INTEGER", nullable: false),
                    SCode = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CST", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CST_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "ClassId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CST_Subjects_SCode",
                        column: x => x.SCode,
                        principalTable: "Subjects",
                        principalColumn: "SCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CST_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentAttendance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NSN = table.Column<string>(type: "TEXT", nullable: false),
                    ClassId = table.Column<int>(type: "INTEGER", nullable: false),
                    AttendanceBy = table.Column<string>(type: "TEXT", nullable: false),
                    AttendanceDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AttendanceEndUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Present = table.Column<bool>(type: "INTEGER", nullable: false),
                    AbsentReason = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentAttendance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentAttendance_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "ClassId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentAttendance_Students_NSN",
                        column: x => x.NSN,
                        principalTable: "Students",
                        principalColumn: "NSN",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentAttendance_Users_AttendanceBy",
                        column: x => x.AttendanceBy,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeacherAttendance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    LoginDateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LogoutDateTime = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherAttendance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeacherAttendance_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClassStudent_ClassId",
                table: "ClassStudent",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassStudent_NSN",
                table: "ClassStudent",
                column: "NSN");

            migrationBuilder.CreateIndex(
                name: "IX_ClassSubject_ClassId",
                table: "ClassSubject",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassSubject_SCode",
                table: "ClassSubject",
                column: "SCode");

            migrationBuilder.CreateIndex(
                name: "IX_CST_ClassId",
                table: "CST",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_CST_SCode",
                table: "CST",
                column: "SCode");

            migrationBuilder.CreateIndex(
                name: "IX_CST_UserId",
                table: "CST",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamRubrick_ExamId_SCode",
                table: "ExamRubrick",
                columns: new[] { "ExamId", "SCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExamRubrick_SCode",
                table: "ExamRubrick",
                column: "SCode");

            migrationBuilder.CreateIndex(
                name: "IX_Marksheet_ExamId",
                table: "Marksheet",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_Marksheet_NSN",
                table: "Marksheet",
                column: "NSN");

            migrationBuilder.CreateIndex(
                name: "IX_Marksheet_SCode",
                table: "Marksheet",
                column: "SCode");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAttendance_AttendanceBy",
                table: "StudentAttendance",
                column: "AttendanceBy");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAttendance_ClassId",
                table: "StudentAttendance",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAttendance_NSN",
                table: "StudentAttendance",
                column: "NSN");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherAttendance_UserId",
                table: "TeacherAttendance",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassStudent");

            migrationBuilder.DropTable(
                name: "ClassSubject");

            migrationBuilder.DropTable(
                name: "CST");

            migrationBuilder.DropTable(
                name: "ExamRubrick");

            migrationBuilder.DropTable(
                name: "LeaveDates");

            migrationBuilder.DropTable(
                name: "Marksheet");

            migrationBuilder.DropTable(
                name: "SchoolInfo");

            migrationBuilder.DropTable(
                name: "StudentAttendance");

            migrationBuilder.DropTable(
                name: "TeacherAttendance");

            migrationBuilder.DropTable(
                name: "ExamList");

            migrationBuilder.DropTable(
                name: "Subjects");

            migrationBuilder.DropTable(
                name: "Classes");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
