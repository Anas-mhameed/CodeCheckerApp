using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CollegeAPI.Migrations
{
    public partial class FifthCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Enrolments",
                table: "Enrolments");

            migrationBuilder.AlterColumn<int>(
                name: "EnrolmentID",
                table: "Enrolments",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Enrolments",
                table: "Enrolments",
                column: "EnrolmentID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Enrolments",
                table: "Enrolments");

            migrationBuilder.AlterColumn<int>(
                name: "EnrolmentID",
                table: "Enrolments",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Enrolments",
                table: "Enrolments",
                columns: new[] { "CourseNum", "UserID" });
        }
    }
}
