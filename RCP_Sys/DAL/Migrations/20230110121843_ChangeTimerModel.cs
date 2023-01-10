using Microsoft.EntityFrameworkCore.Migrations;

namespace RCP_Sys.Migrations
{
    public partial class ChangeTimerModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Times",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Project",
                table: "Times",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Times");

            migrationBuilder.DropColumn(
                name: "Project",
                table: "Times");
        }
    }
}
