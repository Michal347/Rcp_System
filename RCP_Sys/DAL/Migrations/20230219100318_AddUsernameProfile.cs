using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RCP_Sys.Migrations
{
    public partial class AddUsernameProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Picture",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateTimeJoined", "Password" },
                values: new object[] { new DateTime(2023, 2, 19, 11, 3, 18, 454, DateTimeKind.Local).AddTicks(8698), "OMPE/gJQgF3lg6EzWTzITQvGC/UUL7lL40bCWeDZLYN14tyB" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Username",
                table: "Picture");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateTimeJoined", "Password" },
                values: new object[] { new DateTime(2023, 2, 19, 8, 46, 42, 754, DateTimeKind.Local).AddTicks(4536), "ZtI55h/TSOcC9hGq8GthK3QhzuTDJdmF/XqNkgg2wH0AlXBw" });
        }
    }
}
