using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RCP_Sys.Migrations
{
    public partial class AddGender : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "Users",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateTimeJoined", "Password" },
                values: new object[] { new DateTime(2023, 2, 4, 10, 53, 54, 292, DateTimeKind.Local).AddTicks(7958), "exuBHjAoku6CuNsAhBcfH7aCT+6c39Q4PAeyF5SIdoR4KhJY" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateTimeJoined", "Password" },
                values: new object[] { new DateTime(2023, 1, 10, 13, 19, 38, 835, DateTimeKind.Local).AddTicks(367), "uBuGeou9IhQUD714KfHi7X/qL5/B5aY/XXX6Vv7z4O1Yd0Aj" });
        }
    }
}
