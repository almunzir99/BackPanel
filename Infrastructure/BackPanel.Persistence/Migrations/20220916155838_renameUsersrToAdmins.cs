using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackPanel.Persistence.Migrations
{
    public partial class RenameUsersrToAdmins : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activity_Users_AdminId",
                table: "Activity");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Users_AdminId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "Admins");

            migrationBuilder.RenameIndex(
                name: "IX_Users_RoleId",
                newName: "IX_Admins_RoleId",
                table: "Admins");

            migrationBuilder.RenameIndex(
                name: "IX_Users_Email",
                newName: "IX_Admins_Email",
                table: "Admins");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Admins",
                table: "Admins",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastUpdate", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2022, 9, 16, 17, 58, 37, 244, DateTimeKind.Local).AddTicks(986), new DateTime(2022, 9, 16, 17, 58, 37, 244, DateTimeKind.Local).AddTicks(997), new byte[] { 198, 87, 129, 6, 178, 28, 50, 125, 189, 23, 124, 209, 207, 148, 99, 229, 18, 38, 62, 32, 92, 126, 218, 77, 129, 128, 242, 112, 45, 201, 174, 37, 222, 175, 43, 200, 175, 176, 172, 51, 137, 142, 96, 220, 12, 47, 255, 153, 79, 100, 64, 159, 114, 14, 67, 41, 191, 148, 252, 57, 175, 206, 74, 124 }, new byte[] { 172, 199, 252, 17, 162, 127, 86, 2, 18, 134, 251, 181, 191, 38, 119, 191, 156, 99, 235, 196, 209, 96, 160, 195, 253, 45, 71, 6, 33, 11, 80, 87, 225, 27, 209, 174, 41, 142, 70, 121, 186, 12, 22, 121, 34, 204, 179, 101, 223, 176, 5, 181, 132, 10, 235, 123, 224, 213, 94, 35, 14, 118, 18, 134, 218, 178, 214, 194, 241, 202, 252, 97, 5, 87, 239, 252, 191, 69, 240, 243, 135, 200, 207, 187, 115, 115, 191, 80, 194, 90, 9, 53, 169, 124, 106, 22, 134, 95, 229, 221, 134, 232, 17, 31, 154, 69, 17, 20, 68, 15, 207, 232, 9, 146, 243, 229, 242, 169, 123, 134, 193, 117, 87, 27, 150, 41, 13, 174 } });

            migrationBuilder.AddForeignKey(
                name: "FK_Activity_Admins_AdminId",
                table: "Activity",
                column: "AdminId",
                principalTable: "Admins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Admins_Roles_RoleId",
                table: "Admins",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Admins_AdminId",
                table: "Notifications",
                column: "AdminId",
                principalTable: "Admins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activity_Admins_AdminId",
                table: "Activity");

            migrationBuilder.DropForeignKey(
                name: "FK_Admins_Roles_RoleId",
                table: "Admins");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Admins_AdminId",
                table: "Notifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Admins",
                table: "Admins");

            migrationBuilder.RenameTable(
                name: "Admins",
                newName: "Users");

            migrationBuilder.RenameIndex(
                name: "IX_Admins_RoleId",
                newName: "IX_Users_RoleId",
                table: "Users");

            migrationBuilder.RenameIndex(
                name: "IX_Admins_Email",
                newName: "IX_Users_Email",
                table: "Users");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastUpdate", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2022, 9, 13, 22, 18, 24, 508, DateTimeKind.Local).AddTicks(3376), new DateTime(2022, 9, 13, 22, 18, 24, 508, DateTimeKind.Local).AddTicks(3386), new byte[] { 57, 223, 110, 167, 248, 149, 136, 97, 44, 170, 146, 220, 167, 97, 128, 176, 96, 247, 28, 110, 81, 151, 62, 13, 13, 76, 70, 154, 10, 68, 51, 190, 169, 179, 102, 76, 250, 24, 28, 49, 157, 7, 213, 228, 160, 247, 253, 152, 169, 173, 78, 187, 181, 53, 101, 127, 51, 109, 225, 240, 186, 155, 107, 229 }, new byte[] { 127, 49, 43, 176, 94, 104, 142, 139, 77, 24, 37, 136, 249, 103, 42, 10, 15, 132, 194, 83, 195, 72, 118, 247, 85, 27, 202, 39, 152, 32, 102, 141, 46, 96, 31, 16, 153, 173, 243, 34, 203, 169, 244, 107, 73, 191, 44, 63, 243, 250, 81, 84, 45, 214, 251, 116, 82, 138, 251, 40, 192, 156, 210, 243, 67, 64, 251, 106, 68, 117, 164, 21, 210, 251, 38, 106, 139, 169, 253, 161, 89, 153, 237, 196, 137, 155, 176, 102, 140, 118, 180, 228, 251, 56, 143, 206, 44, 13, 36, 203, 116, 186, 196, 142, 187, 205, 196, 14, 149, 168, 5, 238, 172, 40, 12, 107, 172, 102, 64, 6, 162, 241, 44, 112, 157, 121, 187, 129 } });

            migrationBuilder.AddForeignKey(
                name: "FK_Activity_Users_AdminId",
                table: "Activity",
                column: "AdminId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Users_AdminId",
                table: "Notifications",
                column: "AdminId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
