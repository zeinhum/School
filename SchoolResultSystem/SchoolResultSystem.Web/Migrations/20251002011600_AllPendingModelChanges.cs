using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolResultSystem.Web.Migrations
{
    /// <inheritdoc />
    public partial class AllPendingModelChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CST_Users_UserId",
                table: "CST");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "CST",
                newName: "TeacherId");

            migrationBuilder.RenameIndex(
                name: "IX_CST_UserId",
                table: "CST",
                newName: "IX_CST_TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_CST_Users_TeacherId",
                table: "CST",
                column: "TeacherId",
                principalTable: "Users",
                principalColumn: "TeacherId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CST_Users_TeacherId",
                table: "CST");

            migrationBuilder.RenameColumn(
                name: "TeacherId",
                table: "CST",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_CST_TeacherId",
                table: "CST",
                newName: "IX_CST_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CST_Users_UserId",
                table: "CST",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "TeacherId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
