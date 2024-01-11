using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class category : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "27433102-0478-458d-91d6-426bd6773127");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7148504e-6ee8-4e5b-990e-ceb66e3084ff");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a514dc91-6124-45c4-b92c-2f50e35cd2ca");

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "250fdbf5-19d9-4db8-93ce-84f3c33b8115", null, "User", "USER" },
                    { "45eab0aa-81d6-4f63-8bf6-183ace3a2ff2", null, "Admin", "ADMIN" },
                    { "47d83c1b-822b-4b35-b9e1-2b674663766d", null, "Editor", "EDITOR" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "CategoryName" },
                values: new object[,]
                {
                    { 1, "PC Science" },
                    { 2, "Anan" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "250fdbf5-19d9-4db8-93ce-84f3c33b8115");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "45eab0aa-81d6-4f63-8bf6-183ace3a2ff2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "47d83c1b-822b-4b35-b9e1-2b674663766d");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "27433102-0478-458d-91d6-426bd6773127", null, "Editor", "EDITOR" },
                    { "7148504e-6ee8-4e5b-990e-ceb66e3084ff", null, "User", "USER" },
                    { "a514dc91-6124-45c4-b92c-2f50e35cd2ca", null, "Admin", "ADMIN" }
                });
        }
    }
}
