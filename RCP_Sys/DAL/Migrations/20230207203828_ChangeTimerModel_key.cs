using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RCP_Sys.Migrations
{
    public partial class ChangeTimerModel_key : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "StartDateTime",
                table: "Times",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateTimeJoined", "Gender", "Password" },
                values: new object[] { new DateTime(2023, 2, 7, 21, 38, 27, 875, DateTimeKind.Local).AddTicks(514), "male", "SRZBjd1cJxAnUPqSyS7yZ0Yr0cTqwR6ClXbS2Hp5eoaO8TdW" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDateTime",
                table: "Times",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateTimeJoined", "Gender", "Password" },
                values: new object[] { new DateTime(2023, 2, 4, 11, 8, 58, 822, DateTimeKind.Local).AddTicks(5472), "Male", "ZCD+ZygQt+sHS+JIJ/WscAH9e09kzWyaGxJhYK+5eOqrksmu" });
        }
    }
}
