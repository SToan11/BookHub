using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookHub.API.Migrations
{
    /// <inheritdoc />
    public partial class addAu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: new Guid("022d0c0c-f267-4919-97ae-fa4b22c0f4a5"));

            migrationBuilder.DeleteData(
                table: "Staffs",
                keyColumn: "Id",
                keyValue: new Guid("66ae076c-ec73-4196-92a4-7eab123100f0"));

            migrationBuilder.DeleteData(
                table: "Staffs",
                keyColumn: "Id",
                keyValue: new Guid("a33e4901-28a2-4160-b617-f1369c484d34"));

            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "Address", "Email", "FullName", "PasswordHash", "PhoneNumber", "Username" },
                values: new object[] { new Guid("c8ec5386-fb73-4d63-8bdd-9fdfb93cc9f0"), "123 Le Loi, TP.HCM", "toankh@gmail.com", "Nguyen Van A", "U0YS9dlbVt5Avq7BCOZv7YkIBAp91E5utP31PPurS0c=", "0900000003", "customer01" });

            migrationBuilder.InsertData(
                table: "Staffs",
                columns: new[] { "Id", "Email", "PasswordHash", "PhoneNumber", "Role", "Username" },
                values: new object[,]
                {
                    { new Guid("0eb631d4-beec-45e4-a7b3-c3017671fefd"), "toanadmin@gmail.com", "U0YS9dlbVt5Avq7BCOZv7YkIBAp91E5utP31PPurS0c=", "0900000001", "owner", "owner01" },
                    { new Guid("16485d52-5779-485e-90b4-c77900c74e94"), "toanstaff@gmail.com", "U0YS9dlbVt5Avq7BCOZv7YkIBAp91E5utP31PPurS0c=", "0900000002", "employee", "employee01" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: new Guid("c8ec5386-fb73-4d63-8bdd-9fdfb93cc9f0"));

            migrationBuilder.DeleteData(
                table: "Staffs",
                keyColumn: "Id",
                keyValue: new Guid("0eb631d4-beec-45e4-a7b3-c3017671fefd"));

            migrationBuilder.DeleteData(
                table: "Staffs",
                keyColumn: "Id",
                keyValue: new Guid("16485d52-5779-485e-90b4-c77900c74e94"));

            migrationBuilder.DropColumn(
                name: "Author",
                table: "Products");

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "Address", "Email", "FullName", "PasswordHash", "PhoneNumber", "Username" },
                values: new object[] { new Guid("022d0c0c-f267-4919-97ae-fa4b22c0f4a5"), "123 Le Loi, TP.HCM", "toankh@gmail.com", "Nguyen Van A", "U0YS9dlbVt5Avq7BCOZv7YkIBAp91E5utP31PPurS0c=", "0900000003", "customer01" });

            migrationBuilder.InsertData(
                table: "Staffs",
                columns: new[] { "Id", "Email", "PasswordHash", "PhoneNumber", "Role", "Username" },
                values: new object[,]
                {
                    { new Guid("66ae076c-ec73-4196-92a4-7eab123100f0"), "toanstaff@gmail.com", "U0YS9dlbVt5Avq7BCOZv7YkIBAp91E5utP31PPurS0c=", "0900000002", "employee", "employee01" },
                    { new Guid("a33e4901-28a2-4160-b617-f1369c484d34"), "toanadmin@gmail.com", "U0YS9dlbVt5Avq7BCOZv7YkIBAp91E5utP31PPurS0c=", "0900000001", "owner", "owner01" }
                });
        }
    }
}
