using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LearningTask1.Migrations
{
    /// <inheritdoc />
    public partial class AddBusinessCardTblAndSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BusinessCards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DOB = table.Column<DateOnly>(type: "date", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessCards", x => x.Id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BusinessCards");
        }
    }
}
