using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RCP_Sys.Migrations
{
    public partial class AddImageUsermodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageToByte",
                table: "Users",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateTimeJoined", "Password" },
                values: new object[] { new DateTime(2023, 2, 26, 18, 19, 38, 301, DateTimeKind.Local).AddTicks(6559), "EaZUysL/MZYgTvWxQx/dhc9wdOt9807XVdvaWF0PLd+4ctuc" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ImageToByte",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateTimeJoined", "Password" },
                values: new object[] { new DateTime(2023, 2, 19, 11, 3, 18, 454, DateTimeKind.Local).AddTicks(8698), "OMPE/gJQgF3lg6EzWTzITQvGC/UUL7lL40bCWeDZLYN14tyB" });
        }
    }
}
