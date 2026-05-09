using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication3.Migrations
{
    /// <inheritdoc />
    public partial class AddCarProfilePhotoUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoUrl",
                table: "CarProfiles",
                type: "nvarchar(2048)",
                maxLength: 2048,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 5, 2, 19, 27, 38, 82, DateTimeKind.Utc).AddTicks(3739), "a4+JtrpJ1dKRSJ5okmIow6U3nA9P3Ow54//SXIMvkFyWJt2X" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 5, 2, 19, 27, 38, 84, DateTimeKind.Utc).AddTicks(5116), "oQnaWgywlzmoGZwwf9n7RlScrvW+DN4rgHmlkubeu2i1wey3" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "PasswordHash", "Role", "Username" },
                values: new object[] { new DateTime(2026, 5, 2, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(8063), "tDhk97oG1yIO+3eLLdUEyk91zmSucYhhUN4k0+hxMboUn9DW", "Employee", "employee" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoUrl",
                table: "CarProfiles");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 4, 28, 15, 50, 43, 332, DateTimeKind.Utc).AddTicks(8122), "9XdKXC++IPPDQs7F773bZsBaGIr3WO79IqIKbyR8Fv0MDJTa" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 4, 28, 15, 50, 43, 334, DateTimeKind.Utc).AddTicks(3076), "/yUi1cQPSZnu3uWtnBsfOov6q4r7IFgrbhg0CxkW4euyPpYe" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "PasswordHash", "Role", "Username" },
                values: new object[] { new DateTime(2026, 4, 28, 15, 50, 43, 335, DateTimeKind.Utc).AddTicks(7964), "r3XYi3XBVVb1ijSC0Dqw4Vxh7MhnTujhmyVu0Uz8T4B5pnJW", "Employee", "employee" });
        }
    }
}
