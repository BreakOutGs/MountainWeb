using Microsoft.EntityFrameworkCore.Migrations;

namespace MountainWeb.Data.Migrations
{
    public partial class AddAimToDataBase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Aim",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    ApplicationUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Aim_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TaskList",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    AimId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskList_Aim_AimId",
                        column: x => x.AimId,
                        principalTable: "Aim",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserTask",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    IsCompleted = table.Column<bool>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    TaskListId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTask", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTask_TaskList_TaskListId",
                        column: x => x.TaskListId,
                        principalTable: "TaskList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Aim_ApplicationUserId",
                table: "Aim",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskList_AimId",
                table: "TaskList",
                column: "AimId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTask_TaskListId",
                table: "UserTask",
                column: "TaskListId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserTask");

            migrationBuilder.DropTable(
                name: "TaskList");

            migrationBuilder.DropTable(
                name: "Aim");
        }
    }
}
