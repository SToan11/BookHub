using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookHub.API.Migrations
{
    /// <inheritdoc />
    public partial class book2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
