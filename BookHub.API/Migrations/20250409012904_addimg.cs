using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookHub.API.Migrations
{
    /// <inheritdoc />
    public partial class addimg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: new Guid("944cd84b-8146-4354-9b77-d537359f8138"));

            migrationBuilder.DeleteData(
                table: "Staffs",
                keyColumn: "Id",
                keyValue: new Guid("bf253009-81dc-4582-8f45-92ca2b5532e6"));

            migrationBuilder.DeleteData(
                table: "Staffs",
                keyColumn: "Id",
                keyValue: new Guid("e1801928-ab7e-46a4-89ac-36acbb66f6b7"));

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Products");

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "Address", "Email", "FullName", "PasswordHash", "PhoneNumber", "Username" },
                values: new object[] { new Guid("944cd84b-8146-4354-9b77-d537359f8138"), "123 Le Loi, TP.HCM", "toankh@gmail.com", "Nguyen Van A", "U0YS9dlbVt5Avq7BCOZv7YkIBAp91E5utP31PPurS0c=", "0900000003", "customer01" });

            migrationBuilder.InsertData(
                table: "Staffs",
                columns: new[] { "Id", "Email", "PasswordHash", "PhoneNumber", "Role", "Username" },
                values: new object[,]
                {
                    { new Guid("bf253009-81dc-4582-8f45-92ca2b5532e6"), "toanadmin@gmail.com", "U0YS9dlbVt5Avq7BCOZv7YkIBAp91E5utP31PPurS0c=", "0900000001", "owner", "owner01" },
                    { new Guid("e1801928-ab7e-46a4-89ac-36acbb66f6b7"), "toanstaff@gmail.com", "U0YS9dlbVt5Avq7BCOZv7YkIBAp91E5utP31PPurS0c=", "0900000002", "employee", "employee01" }
                });
        }
    }
}
