using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CollegeAPI.Migrations
{
    public partial class FourthInitail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Enrolment",
                table: "Enrolment");

            migrationBuilder.RenameTable(
                name: "Enrolment",
                newName: "Enrolments");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Enrolments",
                table: "Enrolments",
                columns: new[] { "CourseNum", "UserID" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Enrolments",
                table: "Enrolments");

            migrationBuilder.RenameTable(
                name: "Enrolments",
                newName: "Enrolment");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Enrolment",
                table: "Enrolment",
                columns: new[] { "CourseNum", "UserID" });
        }
    }
}
