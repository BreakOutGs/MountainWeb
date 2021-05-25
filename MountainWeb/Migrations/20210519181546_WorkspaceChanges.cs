using Microsoft.EntityFrameworkCore.Migrations;

namespace MountainWeb.Migrations
{
    public partial class WorkspaceChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentWorkspaceId",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentWorkspaceId",
                table: "AspNetUsers");
        }
    }
}
