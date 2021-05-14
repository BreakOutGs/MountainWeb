using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace MountainWeb.Migrations
{
    public partial class addRemindToDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChangeWorkspaceVM",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeWorkspaceVM", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Remind",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateTime = table.Column<DateTime>(nullable: false),
                    MinuteInterval = table.Column<int>(nullable: false),
                    TaskId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Remind", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Remind_UserTask_TaskId",
                        column: x => x.TaskId,
                        principalTable: "UserTask",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Remind_TaskId",
                table: "Remind",
                column: "TaskId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChangeWorkspaceVM");

            migrationBuilder.DropTable(
                name: "Remind");
        }
    }
}
