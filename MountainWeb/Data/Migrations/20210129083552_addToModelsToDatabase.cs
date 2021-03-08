using Microsoft.EntityFrameworkCore.Migrations;

namespace MountainWeb.Data.Migrations
{
    public partial class addToModelsToDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Aim_AspNetUsers_ApplicationUserId",
                table: "Aim");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "UserTask");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Aim",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Aim",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Aim_AspNetUsers_ApplicationUserId",
                table: "Aim",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Aim_AspNetUsers_ApplicationUserId",
                table: "Aim");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Aim");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "UserTask",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Aim",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddForeignKey(
                name: "FK_Aim_AspNetUsers_ApplicationUserId",
                table: "Aim",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
