using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerSide.Migrations
{
    public partial class SecondCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "JsonFile",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RoleFile",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JsonFile",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "RoleFile",
                table: "Tasks");
        }
    }
}
