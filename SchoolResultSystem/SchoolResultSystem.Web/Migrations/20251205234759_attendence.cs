using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolResultSystem.Web.Migrations
{
    /// <inheritdoc />
    public partial class attendence : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                        principalColumn: "TeacherId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeacherAttendance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TeacherId = table.Column<string>(type: "TEXT", nullable: false),
                    LoginDateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LogoutDateTime = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherAttendance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeacherAttendance_Users_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Users",
                        principalColumn: "TeacherId",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "IX_TeacherAttendance_TeacherId",
                table: "TeacherAttendance",
                column: "TeacherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentAttendance");

            migrationBuilder.DropTable(
                name: "TeacherAttendance");
        }
    }
}
