using Microsoft.EntityFrameworkCore.Migrations;

namespace MountainWeb.Migrations
{
    public partial class TaskNameToRemind : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TaskName",
                table: "Reminds",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaskName",
                table: "Reminds");
        }
    }
}
