using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CollegeAPI.Migrations
{
    public partial class FOneInitail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Averages",
                columns: table => new
                {
                    StudentID = table.Column<int>(type: "int", nullable: false),
                    courseNum = table.Column<int>(type: "int", nullable: false),
                    average = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Averages", x => new { x.StudentID, x.courseNum });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Averages");
        }
    }
}
