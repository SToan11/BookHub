using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookHub.API.Migrations
{
    /// <inheritdoc />
    public partial class AddProductCategoryRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_CategoryId",
                table: "Products");

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: new Guid("56896c5f-2158-4679-ac4b-59d040e9f9fc"));

            migrationBuilder.DeleteData(
                table: "Staffs",
                keyColumn: "Id",
                keyValue: new Guid("0a5e0e34-1c19-4296-b0d8-2c15e2e2a8fa"));

            migrationBuilder.DeleteData(
                table: "Staffs",
                keyColumn: "Id",
                keyValue: new Guid("8df7656f-5a8b-4c82-a67a-f752ec5988d2"));

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Products");

            migrationBuilder.CreateTable(
                name: "ProductCategory",
                columns: table => new
                {
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategory", x => new { x.ProductId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_ProductCategory_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductCategory_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "Address", "Email", "FullName", "PasswordHash", "PhoneNumber", "Username" },
                values: new object[] { new Guid("1bb3e19c-fbd2-40ed-ac9b-80e4173e40ab"), "123 Le Loi, TP.HCM", "toankh@gmail.com", "Nguyen Van A", "U0YS9dlbVt5Avq7BCOZv7YkIBAp91E5utP31PPurS0c=", "0900000003", "customer01" });

            migrationBuilder.InsertData(
                table: "Staffs",
                columns: new[] { "Id", "Email", "PasswordHash", "PhoneNumber", "Role", "Username" },
                values: new object[,]
                {
                    { new Guid("16c027f6-47cd-4705-b229-adc78ac9ca14"), "toanadmin@gmail.com", "U0YS9dlbVt5Avq7BCOZv7YkIBAp91E5utP31PPurS0c=", "0900000001", "owner", "owner01" },
                    { new Guid("99b37d24-3028-4d83-a04d-6428cc0e08ff"), "toanstaff@gmail.com", "U0YS9dlbVt5Avq7BCOZv7YkIBAp91E5utP31PPurS0c=", "0900000002", "employee", "employee01" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategory_CategoryId",
                table: "ProductCategory",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductCategory");

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: new Guid("1bb3e19c-fbd2-40ed-ac9b-80e4173e40ab"));

            migrationBuilder.DeleteData(
                table: "Staffs",
                keyColumn: "Id",
                keyValue: new Guid("16c027f6-47cd-4705-b229-adc78ac9ca14"));

            migrationBuilder.DeleteData(
                table: "Staffs",
                keyColumn: "Id",
                keyValue: new Guid("99b37d24-3028-4d83-a04d-6428cc0e08ff"));

            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                table: "Products",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "Address", "Email", "FullName", "PasswordHash", "PhoneNumber", "Username" },
                values: new object[] { new Guid("56896c5f-2158-4679-ac4b-59d040e9f9fc"), "123 Le Loi, TP.HCM", "toankh@gmail.com", "Nguyen Van A", "U0YS9dlbVt5Avq7BCOZv7YkIBAp91E5utP31PPurS0c=", "0900000003", "customer01" });

            migrationBuilder.InsertData(
                table: "Staffs",
                columns: new[] { "Id", "Email", "PasswordHash", "PhoneNumber", "Role", "Username" },
                values: new object[,]
                {
                    { new Guid("0a5e0e34-1c19-4296-b0d8-2c15e2e2a8fa"), "toanstaff@gmail.com", "U0YS9dlbVt5Avq7BCOZv7YkIBAp91E5utP31PPurS0c=", "0900000002", "employee", "employee01" },
                    { new Guid("8df7656f-5a8b-4c82-a67a-f752ec5988d2"), "toanadmin@gmail.com", "U0YS9dlbVt5Avq7BCOZv7YkIBAp91E5utP31PPurS0c=", "0900000001", "owner", "owner01" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
