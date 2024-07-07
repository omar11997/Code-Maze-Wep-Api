using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ultmate_Book.Migrations
{
    /// <inheritdoc />
    public partial class intializeRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "22ef195e-78dd-4637-8f69-8bd501fc6f05", null, "Administrator", "ADMINISTRATOR" },
                    { "32d5ee9b-cf39-4872-ab5a-e449f737456b", null, "Manager", "MANAGER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "22ef195e-78dd-4637-8f69-8bd501fc6f05");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "32d5ee9b-cf39-4872-ab5a-e449f737456b");
        }
    }
}
