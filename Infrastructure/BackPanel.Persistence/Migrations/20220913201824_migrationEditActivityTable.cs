using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackPanel.Persistence.Migrations
{
    public partial class migrationEditActivityTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Activity");

            migrationBuilder.AlterColumn<int>(
                name: "AdminId",
                table: "Activity",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdate",
                table: "Activity",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastUpdate", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2022, 9, 13, 22, 18, 24, 508, DateTimeKind.Local).AddTicks(3376), new DateTime(2022, 9, 13, 22, 18, 24, 508, DateTimeKind.Local).AddTicks(3386), new byte[] { 57, 223, 110, 167, 248, 149, 136, 97, 44, 170, 146, 220, 167, 97, 128, 176, 96, 247, 28, 110, 81, 151, 62, 13, 13, 76, 70, 154, 10, 68, 51, 190, 169, 179, 102, 76, 250, 24, 28, 49, 157, 7, 213, 228, 160, 247, 253, 152, 169, 173, 78, 187, 181, 53, 101, 127, 51, 109, 225, 240, 186, 155, 107, 229 }, new byte[] { 127, 49, 43, 176, 94, 104, 142, 139, 77, 24, 37, 136, 249, 103, 42, 10, 15, 132, 194, 83, 195, 72, 118, 247, 85, 27, 202, 39, 152, 32, 102, 141, 46, 96, 31, 16, 153, 173, 243, 34, 203, 169, 244, 107, 73, 191, 44, 63, 243, 250, 81, 84, 45, 214, 251, 116, 82, 138, 251, 40, 192, 156, 210, 243, 67, 64, 251, 106, 68, 117, 164, 21, 210, 251, 38, 106, 139, 169, 253, 161, 89, 153, 237, 196, 137, 155, 176, 102, 140, 118, 180, 228, 251, 56, 143, 206, 44, 13, 36, 203, 116, 186, 196, 142, 187, 205, 196, 14, 149, 168, 5, 238, 172, 40, 12, 107, 172, 102, 64, 6, 162, 241, 44, 112, 157, 121, 187, 129 } });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastUpdate",
                table: "Activity");

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Roles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Permissions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Notifications",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Messages",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AdminId",
                table: "Activity",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Activity",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastUpdate", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2022, 8, 8, 21, 17, 20, 37, DateTimeKind.Local).AddTicks(5239), new DateTime(2022, 8, 8, 21, 17, 20, 37, DateTimeKind.Local).AddTicks(5247), new byte[] { 34, 244, 156, 255, 137, 230, 196, 12, 114, 19, 40, 206, 4, 66, 220, 94, 91, 222, 196, 89, 207, 46, 158, 220, 106, 134, 194, 139, 77, 117, 246, 26, 120, 160, 85, 48, 4, 205, 221, 166, 150, 39, 31, 192, 0, 165, 133, 233, 248, 112, 237, 8, 147, 114, 239, 119, 9, 182, 187, 41, 243, 237, 161, 122 }, new byte[] { 53, 50, 8, 78, 88, 33, 101, 194, 31, 243, 110, 9, 230, 61, 205, 181, 143, 24, 12, 203, 55, 92, 102, 161, 228, 132, 19, 220, 65, 214, 55, 142, 77, 54, 25, 62, 62, 129, 93, 77, 243, 180, 140, 158, 96, 174, 23, 238, 64, 197, 230, 55, 166, 67, 158, 22, 253, 201, 30, 115, 165, 179, 230, 76, 112, 14, 108, 95, 204, 75, 192, 224, 195, 251, 108, 219, 45, 74, 162, 29, 166, 4, 209, 217, 59, 113, 90, 86, 92, 112, 233, 217, 190, 226, 223, 169, 64, 181, 151, 180, 73, 27, 232, 59, 145, 11, 112, 242, 116, 21, 23, 89, 107, 255, 167, 41, 151, 107, 20, 215, 218, 196, 220, 95, 180, 193, 209, 79 } });
        }
    }
}
