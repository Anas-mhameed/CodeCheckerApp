using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CollegeAPI.Migrations
{
    public partial class ThirdCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CourseID",
                table: "Tasks",
                newName: "CourseNum");

            migrationBuilder.RenameColumn(
                name: "CourseID",
                table: "Grades",
                newName: "CourseNum");

            migrationBuilder.AddColumn<int>(
                name: "CourseNum",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Enrolment",
                columns: table => new
                {
                    CourseNum = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    EnrolmentID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enrolment", x => new { x.CourseNum, x.UserID });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Enrolment");

            migrationBuilder.DropColumn(
                name: "CourseNum",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "CourseNum",
                table: "Tasks",
                newName: "CourseID");

            migrationBuilder.RenameColumn(
                name: "CourseNum",
                table: "Grades",
                newName: "CourseID");
        }
    }
}
