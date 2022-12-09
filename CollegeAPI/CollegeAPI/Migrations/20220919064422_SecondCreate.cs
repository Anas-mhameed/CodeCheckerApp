using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CollegeAPI.Migrations
{
    public partial class SecondCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Users",
                newName: "UserID");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Courses",
                newName: "TeacherID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Users",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "TeacherID",
                table: "Courses",
                newName: "UserID");
        }
    }
}
