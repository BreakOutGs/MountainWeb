using Microsoft.EntityFrameworkCore.Migrations;

namespace MountainWeb.Migrations
{
    public partial class Migration2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Remind_UserTask_TaskId",
                table: "Remind");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Remind",
                table: "Remind");

            migrationBuilder.RenameTable(
                name: "Remind",
                newName: "Reminds");

            migrationBuilder.RenameIndex(
                name: "IX_Remind_TaskId",
                table: "Reminds",
                newName: "IX_Reminds_TaskId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reminds",
                table: "Reminds",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reminds_UserTask_TaskId",
                table: "Reminds",
                column: "TaskId",
                principalTable: "UserTask",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reminds_UserTask_TaskId",
                table: "Reminds");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reminds",
                table: "Reminds");

            migrationBuilder.RenameTable(
                name: "Reminds",
                newName: "Remind");

            migrationBuilder.RenameIndex(
                name: "IX_Reminds_TaskId",
                table: "Remind",
                newName: "IX_Remind_TaskId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Remind",
                table: "Remind",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Remind_UserTask_TaskId",
                table: "Remind",
                column: "TaskId",
                principalTable: "UserTask",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
