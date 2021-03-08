using Microsoft.EntityFrameworkCore.Migrations;

namespace MountainWeb.Data.Migrations
{
    public partial class addEventLogToDadabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "eventLogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(nullable: true),
                    EventType = table.Column<int>(nullable: true)

                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_eventLogs", x => x.Id);
                }); ;
        }


        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropTable(
                name: "eventLogs");


        }
    }
}
