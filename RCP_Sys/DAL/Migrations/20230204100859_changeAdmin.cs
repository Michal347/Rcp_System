using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RCP_Sys.Migrations
{
    public partial class changeAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateTimeJoined", "Gender", "Password" },
                values: new object[] { new DateTime(2023, 2, 4, 11, 8, 58, 822, DateTimeKind.Local).AddTicks(5472), "Male", "ZCD+ZygQt+sHS+JIJ/WscAH9e09kzWyaGxJhYK+5eOqrksmu" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateTimeJoined", "Gender", "Password" },
                values: new object[] { new DateTime(2023, 2, 4, 10, 53, 54, 292, DateTimeKind.Local).AddTicks(7958), null, "exuBHjAoku6CuNsAhBcfH7aCT+6c39Q4PAeyF5SIdoR4KhJY" });
        }
    }
}
