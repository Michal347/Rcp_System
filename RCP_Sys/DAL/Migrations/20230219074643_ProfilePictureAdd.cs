using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RCP_Sys.Migrations
{
    public partial class ProfilePictureAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Picture",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImagePath = table.Column<string>(nullable: true),
                    ImageToByte = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Picture", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateTimeJoined", "Password" },
                values: new object[] { new DateTime(2023, 2, 19, 8, 46, 42, 754, DateTimeKind.Local).AddTicks(4536), "ZtI55h/TSOcC9hGq8GthK3QhzuTDJdmF/XqNkgg2wH0AlXBw" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Picture");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateTimeJoined", "Password" },
                values: new object[] { new DateTime(2023, 2, 7, 21, 38, 27, 875, DateTimeKind.Local).AddTicks(514), "SRZBjd1cJxAnUPqSyS7yZ0Yr0cTqwR6ClXbS2Hp5eoaO8TdW" });
        }
    }
}
