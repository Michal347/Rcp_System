using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RCP_Sys.Migrations
{
    public partial class SeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "DateTimeJoined", "Email", "IsUserAdmin", "Name", "Password", "Surname", "Username" },
                values: new object[] { 1, new DateTime(2023, 1, 10, 13, 19, 38, 835, DateTimeKind.Local).AddTicks(367), "admin@gmail.com", true, "Admin", "uBuGeou9IhQUD714KfHi7X/qL5/B5aY/XXX6Vv7z4O1Yd0Aj", "Admin", "Admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
