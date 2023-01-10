using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RCP_Sys.Migrations
{
    public partial class addAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "DateTimeJoined", "Email", "IsUserAdmin", "Name", "Password", "Surname", "Username" },
                values: new object[] { 1, new DateTime(2023, 1, 10, 12, 12, 57, 909, DateTimeKind.Local).AddTicks(4247), "admin@gmail.com", true, "Admin", "lgCvIqTtx1WDjy1qWm4lr7GbSwmWTVJcWPhR+SgqQXMEkfo9", "Admin", "Admin" });
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
