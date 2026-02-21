using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackPanel.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAdminAddUserNotificationRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activity_Admins_AdminId",
                table: "Activity");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Admins_AdminId",
                table: "Notifications");

            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Activity",
                table: "Activity");

            migrationBuilder.DropColumn(
                name: "LegacyAdminId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LegacyRoleId",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "Activity",
                newName: "Activities");

            migrationBuilder.RenameColumn(
                name: "AdminId",
                table: "Notifications",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_AdminId",
                table: "Notifications",
                newName: "IX_Notifications_UserId");

            migrationBuilder.RenameColumn(
                name: "AdminId",
                table: "Activities",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Activity_AdminId",
                table: "Activities",
                newName: "IX_Activities_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Activities",
                table: "Activities",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_AspNetUsers_UserId",
                table: "Activities",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_AspNetUsers_UserId",
                table: "Notifications",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_AspNetUsers_UserId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_AspNetUsers_UserId",
                table: "Notifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Activities",
                table: "Activities");

            migrationBuilder.RenameTable(
                name: "Activities",
                newName: "Activity");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Notifications",
                newName: "AdminId");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                newName: "IX_Notifications_AdminId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Activity",
                newName: "AdminId");

            migrationBuilder.RenameIndex(
                name: "IX_Activities_UserId",
                table: "Activity",
                newName: "IX_Activity_AdminId");

            migrationBuilder.AddColumn<int>(
                name: "LegacyAdminId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LegacyRoleId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Activity",
                table: "Activity",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsManager = table.Column<bool>(type: "bit", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Admins_Email",
                table: "Admins",
                column: "Email",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Activity_Admins_AdminId",
                table: "Activity",
                column: "AdminId",
                principalTable: "Admins",
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
    }
}
