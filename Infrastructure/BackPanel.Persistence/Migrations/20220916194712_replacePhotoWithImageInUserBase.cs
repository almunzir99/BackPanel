using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackPanel.Persistence.Migrations
{
    public partial class ReplacePhotoWithImageInUserBase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Photo",
                table: "Admins");

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastUpdate", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2022, 9, 16, 21, 47, 12, 379, DateTimeKind.Local).AddTicks(5766), new DateTime(2022, 9, 16, 21, 47, 12, 379, DateTimeKind.Local).AddTicks(5780), new byte[] { 179, 237, 243, 120, 78, 21, 84, 8, 99, 52, 230, 232, 193, 22, 148, 144, 20, 211, 81, 74, 101, 188, 54, 120, 15, 8, 69, 120, 67, 255, 189, 5, 32, 252, 189, 113, 18, 144, 123, 54, 120, 104, 232, 204, 17, 242, 153, 83, 107, 34, 20, 153, 173, 4, 153, 199, 96, 240, 209, 233, 88, 203, 164, 162 }, new byte[] { 225, 202, 147, 146, 30, 181, 199, 164, 142, 182, 69, 126, 92, 152, 62, 83, 251, 132, 123, 86, 149, 40, 100, 41, 214, 48, 139, 171, 131, 251, 175, 161, 252, 96, 176, 214, 46, 106, 217, 62, 173, 204, 225, 193, 85, 56, 118, 45, 116, 240, 156, 17, 18, 42, 191, 100, 246, 139, 55, 149, 188, 61, 110, 110, 251, 230, 7, 115, 173, 30, 119, 205, 21, 203, 8, 96, 92, 121, 115, 11, 230, 93, 83, 177, 16, 227, 56, 202, 78, 238, 154, 245, 119, 142, 198, 17, 132, 92, 42, 20, 172, 181, 52, 165, 224, 95, 132, 205, 120, 54, 244, 255, 217, 224, 54, 229, 52, 77, 7, 48, 209, 114, 114, 250, 237, 19, 120, 62 } });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Photo",
                table: "Admins",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastUpdate", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2022, 9, 16, 17, 58, 37, 244, DateTimeKind.Local).AddTicks(986), new DateTime(2022, 9, 16, 17, 58, 37, 244, DateTimeKind.Local).AddTicks(997), new byte[] { 198, 87, 129, 6, 178, 28, 50, 125, 189, 23, 124, 209, 207, 148, 99, 229, 18, 38, 62, 32, 92, 126, 218, 77, 129, 128, 242, 112, 45, 201, 174, 37, 222, 175, 43, 200, 175, 176, 172, 51, 137, 142, 96, 220, 12, 47, 255, 153, 79, 100, 64, 159, 114, 14, 67, 41, 191, 148, 252, 57, 175, 206, 74, 124 }, new byte[] { 172, 199, 252, 17, 162, 127, 86, 2, 18, 134, 251, 181, 191, 38, 119, 191, 156, 99, 235, 196, 209, 96, 160, 195, 253, 45, 71, 6, 33, 11, 80, 87, 225, 27, 209, 174, 41, 142, 70, 121, 186, 12, 22, 121, 34, 204, 179, 101, 223, 176, 5, 181, 132, 10, 235, 123, 224, 213, 94, 35, 14, 118, 18, 134, 218, 178, 214, 194, 241, 202, 252, 97, 5, 87, 239, 252, 191, 69, 240, 243, 135, 200, 207, 187, 115, 115, 191, 80, 194, 90, 9, 53, 169, 124, 106, 22, 134, 95, 229, 221, 134, 232, 17, 31, 154, 69, 17, 20, 68, 15, 207, 232, 9, 146, 243, 229, 242, 169, 123, 134, 193, 117, 87, 27, 150, 41, 13, 174 } });
        }
    }
}
