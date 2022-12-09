using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CollegeAPI.Migrations
{
    public partial class NinethCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Enrolments",
                newName: "EnrolmentID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EnrolmentID",
                table: "Enrolments",
                newName: "ID");
        }
    }
}
