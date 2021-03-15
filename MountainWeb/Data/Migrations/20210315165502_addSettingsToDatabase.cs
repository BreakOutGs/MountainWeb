using Microsoft.EntityFrameworkCore.Migrations;

namespace MountainWeb.Data.Migrations
{
    public partial class addSettingsToDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "aimSettings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Expanded = table.Column<bool>(nullable: false),
                    AimId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aimSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_aimSettings_Aim_AimId",
                        column: x => x.AimId,
                        principalTable: "Aim",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "taskListSettings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Expanded = table.Column<bool>(nullable: false),
                    TaskListId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_taskListSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_taskListSettings_TaskList_TaskListId",
                        column: x => x.TaskListId,
                        principalTable: "TaskList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "userTaskSettings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserTaskId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userTaskSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_userTaskSettings_UserTask_UserTaskId",
                        column: x => x.UserTaskId,
                        principalTable: "UserTask",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_aimSettings_AimId",
                table: "aimSettings",
                column: "AimId");

            migrationBuilder.CreateIndex(
                name: "IX_taskListSettings_TaskListId",
                table: "taskListSettings",
                column: "TaskListId");

            migrationBuilder.CreateIndex(
                name: "IX_userTaskSettings_UserTaskId",
                table: "userTaskSettings",
                column: "UserTaskId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "aimSettings");

            migrationBuilder.DropTable(
                name: "taskListSettings");

            migrationBuilder.DropTable(
                name: "userTaskSettings");
        }
    }
}
