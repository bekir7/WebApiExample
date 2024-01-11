using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class mig3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8bb66a4f-7bea-4daf-89a6-991b8bafd0fa");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ce8ad758-7221-4f6d-ac52-68c7be667ff9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fcbe113c-1c0a-4bfa-b991-6530b9c5f6d2");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0cae62e5-1aed-4726-a699-5558fa71f5ca", null, "Admin", "ADMIN" },
                    { "82c97171-9b49-4d29-a136-db7f94c2dc0e", null, "Editor", "EDITOR" },
                    { "95728f4b-df56-4645-92ea-7b5a76c260be", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0cae62e5-1aed-4726-a699-5558fa71f5ca");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "82c97171-9b49-4d29-a136-db7f94c2dc0e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "95728f4b-df56-4645-92ea-7b5a76c260be");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "8bb66a4f-7bea-4daf-89a6-991b8bafd0fa", null, "User", "USER" },
                    { "ce8ad758-7221-4f6d-ac52-68c7be667ff9", null, "Editor", "EDITOR" },
                    { "fcbe113c-1c0a-4bfa-b991-6530b9c5f6d2", null, "Admin", "ADMIN" }
                });
        }
    }
}
