using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BtkAkademi.WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddRefreshTokenFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9fabb4bf-aa25-4f6d-98d4-9690560ea03d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dbd0375a-9261-418e-8d41-63fb41e0cec1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e7fbcfe9-f388-407c-a7f2-0df11f1ebf11");

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpireTime",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2a4cf219-b375-4283-a622-ee550eb8e339", null, "User", "USER" },
                    { "5ec83e99-bbe1-4c80-8f36-bcd7867e1836", null, "Editor", "EDITOR" },
                    { "85693b34-54c9-4f61-897d-a8390ccc7614", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2a4cf219-b375-4283-a622-ee550eb8e339");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5ec83e99-bbe1-4c80-8f36-bcd7867e1836");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "85693b34-54c9-4f61-897d-a8390ccc7614");

            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpireTime",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "9fabb4bf-aa25-4f6d-98d4-9690560ea03d", null, "User", "USER" },
                    { "dbd0375a-9261-418e-8d41-63fb41e0cec1", null, "Editor", "EDITOR" },
                    { "e7fbcfe9-f388-407c-a7f2-0df11f1ebf11", null, "Admin", "ADMIN" }
                });
        }
    }
}
