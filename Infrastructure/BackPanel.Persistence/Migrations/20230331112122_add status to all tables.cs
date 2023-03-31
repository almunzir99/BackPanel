using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackPanel.Persistence.Migrations
{
    public partial class addstatustoalltables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Roles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Permissions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Notifications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Messages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Admins",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Activity",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastUpdate", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2023, 3, 31, 14, 21, 22, 195, DateTimeKind.Local).AddTicks(1245), new DateTime(2023, 3, 31, 14, 21, 22, 195, DateTimeKind.Local).AddTicks(1255), new byte[] { 11, 74, 97, 131, 123, 123, 118, 84, 111, 165, 172, 247, 173, 242, 74, 29, 159, 255, 169, 214, 47, 119, 202, 252, 143, 76, 125, 103, 113, 4, 253, 188, 210, 215, 151, 213, 59, 171, 168, 112, 24, 90, 66, 117, 193, 14, 16, 254, 8, 71, 43, 198, 114, 97, 51, 30, 81, 55, 156, 62, 126, 151, 133, 40 }, new byte[] { 35, 0, 207, 154, 113, 185, 134, 50, 44, 65, 89, 13, 150, 116, 131, 123, 147, 216, 224, 176, 137, 228, 124, 44, 133, 58, 39, 145, 200, 246, 66, 243, 46, 225, 88, 200, 236, 34, 176, 125, 151, 171, 151, 228, 247, 69, 77, 180, 30, 178, 229, 210, 160, 76, 181, 186, 130, 213, 55, 75, 10, 21, 124, 106, 42, 31, 137, 252, 36, 99, 64, 40, 190, 253, 9, 195, 157, 149, 222, 197, 59, 173, 44, 27, 239, 242, 155, 193, 250, 36, 213, 162, 128, 143, 70, 193, 217, 88, 219, 109, 208, 248, 109, 116, 107, 218, 52, 141, 202, 93, 9, 134, 141, 98, 201, 95, 190, 125, 106, 183, 67, 16, 84, 143, 19, 86, 178, 181 } });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Admins");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Activity");

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastUpdate", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2022, 9, 16, 21, 47, 12, 379, DateTimeKind.Local).AddTicks(5766), new DateTime(2022, 9, 16, 21, 47, 12, 379, DateTimeKind.Local).AddTicks(5780), new byte[] { 179, 237, 243, 120, 78, 21, 84, 8, 99, 52, 230, 232, 193, 22, 148, 144, 20, 211, 81, 74, 101, 188, 54, 120, 15, 8, 69, 120, 67, 255, 189, 5, 32, 252, 189, 113, 18, 144, 123, 54, 120, 104, 232, 204, 17, 242, 153, 83, 107, 34, 20, 153, 173, 4, 153, 199, 96, 240, 209, 233, 88, 203, 164, 162 }, new byte[] { 225, 202, 147, 146, 30, 181, 199, 164, 142, 182, 69, 126, 92, 152, 62, 83, 251, 132, 123, 86, 149, 40, 100, 41, 214, 48, 139, 171, 131, 251, 175, 161, 252, 96, 176, 214, 46, 106, 217, 62, 173, 204, 225, 193, 85, 56, 118, 45, 116, 240, 156, 17, 18, 42, 191, 100, 246, 139, 55, 149, 188, 61, 110, 110, 251, 230, 7, 115, 173, 30, 119, 205, 21, 203, 8, 96, 92, 121, 115, 11, 230, 93, 83, 177, 16, 227, 56, 202, 78, 238, 154, 245, 119, 142, 198, 17, 132, 92, 42, 20, 172, 181, 52, 165, 224, 95, 132, 205, 120, 54, 244, 255, 217, 224, 54, 229, 52, 77, 7, 48, 209, 114, 114, 250, 237, 19, 120, 62 } });
        }
    }
}
