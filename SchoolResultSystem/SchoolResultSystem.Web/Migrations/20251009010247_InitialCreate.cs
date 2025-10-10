﻿using System;
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
                    ClassName = table.Column<string>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.ClassId);
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
                    StudentName = table.Column<string>(type: "TEXT", nullable: false),
                    D_O_B = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    AdmissionDate = table.Column<DateTime>(type: "TEXT", nullable: false),
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
                    TeacherId = table.Column<string>(type: "TEXT", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    TeacherName = table.Column<string>(type: "TEXT", nullable: false),
                    Role = table.Column<string>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.TeacherId);
                });

            migrationBuilder.CreateTable(
                name: "CS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NSN = table.Column<string>(type: "TEXT", nullable: false),
                    ClassId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CS_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "ClassId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CS_Students_NSN",
                        column: x => x.NSN,
                        principalTable: "Students",
                        principalColumn: "NSN",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Exams",
                columns: table => new
                {
                    ExamId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AcademicYear = table.Column<int>(type: "INTEGER", nullable: false),
                    ExamName = table.Column<string>(type: "TEXT", nullable: false),
                    SCode = table.Column<string>(type: "TEXT", nullable: false),
                    ThMark = table.Column<decimal>(type: "TEXT", nullable: false),
                    PrMark = table.Column<decimal>(type: "TEXT", nullable: false),
                    ThCrh = table.Column<decimal>(type: "TEXT", nullable: false),
                    PrCrh = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exams", x => x.ExamId);
                    table.ForeignKey(
                        name: "FK_Exams_Subjects_SCode",
                        column: x => x.SCode,
                        principalTable: "Subjects",
                        principalColumn: "SCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CST",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClassId = table.Column<int>(type: "INTEGER", nullable: false),
                    SCode = table.Column<string>(type: "TEXT", nullable: false),
                    TeacherId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CST", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CST_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "ClassId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CST_Subjects_SCode",
                        column: x => x.SCode,
                        principalTable: "Subjects",
                        principalColumn: "SCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CST_Users_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Users",
                        principalColumn: "TeacherId",
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
                    ThMark = table.Column<decimal>(type: "TEXT", nullable: false),
                    PrMark = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Marksheet", x => x.MarksheetId);
                    table.ForeignKey(
                        name: "FK_Marksheet_Exams_ExamId",
                        column: x => x.ExamId,
                        principalTable: "Exams",
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

            migrationBuilder.CreateIndex(
                name: "IX_CS_ClassId",
                table: "CS",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_CS_NSN",
                table: "CS",
                column: "NSN");

            migrationBuilder.CreateIndex(
                name: "IX_CST_ClassId",
                table: "CST",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_CST_SCode",
                table: "CST",
                column: "SCode");

            migrationBuilder.CreateIndex(
                name: "IX_CST_TeacherId",
                table: "CST",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Exams_SCode",
                table: "Exams",
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CS");

            migrationBuilder.DropTable(
                name: "CST");

            migrationBuilder.DropTable(
                name: "Marksheet");

            migrationBuilder.DropTable(
                name: "SchoolInfo");

            migrationBuilder.DropTable(
                name: "Classes");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Exams");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Subjects");
        }
    }
}
