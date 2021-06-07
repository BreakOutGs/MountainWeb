using Microsoft.EntityFrameworkCore.Migrations;

namespace MountainWeb.Migrations
{
    public partial class AddCurrentAimId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentAim",
                table: "WorkspaceSettings",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentAim",
                table: "WorkspaceSettings");
        }
    }
}
