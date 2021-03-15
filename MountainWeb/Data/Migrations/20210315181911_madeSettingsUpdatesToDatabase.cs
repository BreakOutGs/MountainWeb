using Microsoft.EntityFrameworkCore.Migrations;

namespace MountainWeb.Data.Migrations
{
    public partial class madeSettingsUpdatesToDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_userTaskSettings_UserTaskId",
                table: "userTaskSettings");

            migrationBuilder.DropIndex(
                name: "IX_taskListSettings_TaskListId",
                table: "taskListSettings");

            migrationBuilder.DropIndex(
                name: "IX_aimSettings_AimId",
                table: "aimSettings");

            migrationBuilder.CreateIndex(
                name: "IX_userTaskSettings_UserTaskId",
                table: "userTaskSettings",
                column: "UserTaskId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_taskListSettings_TaskListId",
                table: "taskListSettings",
                column: "TaskListId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_aimSettings_AimId",
                table: "aimSettings",
                column: "AimId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_userTaskSettings_UserTaskId",
                table: "userTaskSettings");

            migrationBuilder.DropIndex(
                name: "IX_taskListSettings_TaskListId",
                table: "taskListSettings");

            migrationBuilder.DropIndex(
                name: "IX_aimSettings_AimId",
                table: "aimSettings");

            migrationBuilder.CreateIndex(
                name: "IX_userTaskSettings_UserTaskId",
                table: "userTaskSettings",
                column: "UserTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_taskListSettings_TaskListId",
                table: "taskListSettings",
                column: "TaskListId");

            migrationBuilder.CreateIndex(
                name: "IX_aimSettings_AimId",
                table: "aimSettings",
                column: "AimId");
        }
    }
}
