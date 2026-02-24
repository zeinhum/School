using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolResultSystem.Web.Migrations
{
    /// <inheritdoc />
    public partial class exalistadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClassSubject",
                columns: table => new
                {
                    tabId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClassId = table.Column<int>(type: "INTEGER", nullable: false),
                    SCode = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassSubject", x => x.tabId);
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

            migrationBuilder.CreateIndex(
                name: "IX_Exams_ExamId",
                table: "Exams",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassSubject_ClassId",
                table: "ClassSubject",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassSubject_SCode",
                table: "ClassSubject",
                column: "SCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_ExamList_ExamId",
                table: "Exams",
                column: "ExamId",
                principalTable: "ExamList",
                principalColumn: "ExamId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exams_ExamList_ExamId",
                table: "Exams");

            migrationBuilder.DropTable(
                name: "ClassSubject");

            migrationBuilder.DropTable(
                name: "ExamList");

            migrationBuilder.DropIndex(
                name: "IX_Exams_ExamId",
                table: "Exams");
        }
    }
}
