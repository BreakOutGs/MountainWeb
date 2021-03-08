using Microsoft.EntityFrameworkCore.Migrations;

namespace MountainWeb.Data.Migrations
{
    public partial class addPriorityToDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "UserTask",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Priority",
                table: "UserTask");
        }
    }
}
