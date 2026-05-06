using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApplication3.Migrations
{
    /// <inheritdoc />
    public partial class AddUserAuthentication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "PasswordHash", "Role", "Username" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 4, 28, 15, 50, 43, 332, DateTimeKind.Utc).AddTicks(8122), "9XdKXC++IPPDQs7F773bZsBaGIr3WO79IqIKbyR8Fv0MDJTa", "Admin", "admin" },
                    { 2, new DateTime(2026, 4, 28, 15, 50, 43, 334, DateTimeKind.Utc).AddTicks(3076), "/yUi1cQPSZnu3uWtnBsfOov6q4r7IFgrbhg0CxkW4euyPpYe", "User", "user" },
                    { 3, new DateTime(2026, 4, 28, 15, 50, 43, 335, DateTimeKind.Utc).AddTicks(7964), "r3XYi3XBVVb1ijSC0Dqw4Vxh7MhnTujhmyVu0Uz8T4B5pnJW", "Instructor", "instructor" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
