using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackPanel.Persistence.Migrations
{
    public partial class AddCompanyInfoPermissions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyInfosPermissionsId",
                table: "Roles",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastUpdate", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2023, 6, 6, 13, 28, 9, 379, DateTimeKind.Local).AddTicks(6410), new DateTime(2023, 6, 6, 13, 28, 9, 379, DateTimeKind.Local).AddTicks(6430), new byte[] { 113, 246, 85, 65, 199, 180, 72, 45, 136, 54, 221, 182, 122, 53, 99, 73, 163, 242, 214, 57, 91, 54, 74, 230, 145, 228, 42, 170, 39, 159, 43, 151, 31, 222, 167, 240, 49, 232, 236, 111, 109, 116, 1, 58, 179, 242, 111, 86, 211, 59, 218, 105, 167, 155, 87, 17, 114, 83, 185, 135, 82, 93, 171, 133 }, new byte[] { 29, 184, 222, 240, 159, 75, 196, 155, 251, 73, 132, 248, 0, 75, 206, 248, 42, 198, 155, 248, 185, 222, 249, 65, 223, 139, 53, 220, 204, 9, 134, 41, 114, 32, 31, 64, 131, 111, 124, 12, 159, 196, 50, 176, 222, 8, 125, 170, 249, 69, 197, 254, 99, 49, 245, 202, 156, 212, 187, 230, 224, 17, 148, 160, 209, 184, 153, 33, 111, 162, 132, 35, 64, 232, 165, 67, 216, 168, 73, 8, 141, 255, 146, 29, 142, 21, 59, 2, 8, 59, 20, 199, 43, 87, 31, 214, 212, 10, 178, 251, 9, 133, 161, 250, 230, 36, 41, 206, 151, 40, 136, 124, 59, 104, 105, 227, 135, 216, 13, 95, 154, 2, 96, 176, 60, 155, 197, 47 } });

            migrationBuilder.CreateIndex(
                name: "IX_Roles_CompanyInfosPermissionsId",
                table: "Roles",
                column: "CompanyInfosPermissionsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Permissions_CompanyInfosPermissionsId",
                table: "Roles",
                column: "CompanyInfosPermissionsId",
                principalTable: "Permissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Permissions_CompanyInfosPermissionsId",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Roles_CompanyInfosPermissionsId",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CompanyInfosPermissionsId",
                table: "Roles");

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastUpdate", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2023, 6, 6, 13, 28, 6, 81, DateTimeKind.Local).AddTicks(5710), new DateTime(2023, 6, 6, 13, 28, 6, 81, DateTimeKind.Local).AddTicks(5730), new byte[] { 226, 104, 177, 124, 158, 57, 149, 29, 126, 217, 44, 126, 121, 189, 51, 46, 233, 240, 244, 101, 216, 110, 205, 239, 45, 145, 120, 234, 127, 110, 225, 100, 206, 124, 156, 250, 125, 165, 96, 231, 220, 70, 1, 28, 150, 96, 124, 64, 220, 54, 184, 245, 214, 186, 205, 10, 21, 25, 74, 67, 182, 135, 148, 115 }, new byte[] { 167, 250, 80, 114, 35, 129, 99, 98, 119, 147, 33, 108, 58, 192, 136, 112, 96, 248, 170, 53, 146, 52, 219, 2, 226, 146, 227, 219, 73, 39, 238, 56, 32, 91, 33, 208, 199, 219, 185, 127, 232, 167, 137, 240, 146, 30, 109, 136, 249, 235, 208, 104, 104, 219, 173, 72, 75, 49, 57, 116, 108, 119, 247, 73, 105, 13, 110, 47, 226, 49, 67, 186, 4, 88, 253, 26, 141, 112, 141, 63, 79, 12, 151, 164, 96, 72, 99, 224, 212, 202, 195, 83, 117, 80, 42, 192, 241, 78, 8, 34, 85, 166, 140, 105, 47, 23, 23, 190, 68, 68, 141, 200, 182, 108, 68, 244, 180, 118, 182, 21, 15, 165, 70, 86, 11, 176, 28, 230 } });
        }
    }
}
