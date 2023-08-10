using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BtkAkademi.WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddRolesToDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
