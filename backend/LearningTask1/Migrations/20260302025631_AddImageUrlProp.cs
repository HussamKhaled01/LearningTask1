using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LearningTask1.Migrations
{
    /// <inheritdoc />
    public partial class AddImageUrlProp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BusinessCards",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "BusinessCards",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "BusinessCards",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "BusinessCards",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "BusinessCards",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "BusinessCards",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "BusinessCards",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "BusinessCards");

            migrationBuilder.InsertData(
                table: "BusinessCards",
                columns: new[] { "Id", "Address", "DOB", "Email", "Gender", "Name", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, "123 Main St", new DateOnly(1990, 1, 1), "john@example.com", "Male", "John Doe", "1234567890" },
                    { 2, "456 Oak Ave", new DateOnly(1992, 5, 10), "jane@example.com", "Female", "Jane Smith", "0987654321" },
                    { 3, "789 Pine Rd", new DateOnly(1985, 3, 15), "michael@example.com", "Male", "Michael Brown", "1112223333" },
                    { 4, "321 Cedar Ln", new DateOnly(1995, 7, 22), "emily@example.com", "Female", "Emily White", "4445556666" },
                    { 5, "654 Maple St", new DateOnly(1988, 11, 30), "david@example.com", "Male", "David Green", "7778889999" },
                    { 6, "987 Birch Ave", new DateOnly(1993, 2, 5), "sophia@example.com", "Female", "Sophia Blue", "2223334444" }
                });
        }
    }
}
