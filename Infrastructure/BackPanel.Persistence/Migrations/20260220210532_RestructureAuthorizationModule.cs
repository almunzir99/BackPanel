using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackPanel.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RestructureAuthorizationModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admins_Roles_RoleId",
                table: "Admins");

            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Permissions_AdminsPermissionsId",
                table: "Roles");

            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Permissions_CompanyInfosPermissionsId",
                table: "Roles");

            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Permissions_MessagesPermissionsId",
                table: "Roles");

            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Permissions_RolesPermissionsId",
                table: "Roles");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropIndex(
                name: "IX_Roles_AdminsPermissionsId",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Roles_CompanyInfosPermissionsId",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Roles_MessagesPermissionsId",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Roles_RolesPermissionsId",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Admins_RoleId",
                table: "Admins");

            migrationBuilder.DropColumn(
                name: "AdminsPermissionsId",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CompanyInfosPermissionsId",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "MessagesPermissionsId",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "RolesPermissionsId",
                table: "Roles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AdminsPermissionsId",
                table: "Roles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompanyInfosPermissionsId",
                table: "Roles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MessagesPermissionsId",
                table: "Roles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RolesPermissionsId",
                table: "Roles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Create = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Delete = table.Column<bool>(type: "bit", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Read = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Update = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Roles_AdminsPermissionsId",
                table: "Roles",
                column: "AdminsPermissionsId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_CompanyInfosPermissionsId",
                table: "Roles",
                column: "CompanyInfosPermissionsId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_MessagesPermissionsId",
                table: "Roles",
                column: "MessagesPermissionsId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_RolesPermissionsId",
                table: "Roles",
                column: "RolesPermissionsId");

            migrationBuilder.CreateIndex(
                name: "IX_Admins_RoleId",
                table: "Admins",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Admins_Roles_RoleId",
                table: "Admins",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Permissions_AdminsPermissionsId",
                table: "Roles",
                column: "AdminsPermissionsId",
                principalTable: "Permissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Permissions_CompanyInfosPermissionsId",
                table: "Roles",
                column: "CompanyInfosPermissionsId",
                principalTable: "Permissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Permissions_MessagesPermissionsId",
                table: "Roles",
                column: "MessagesPermissionsId",
                principalTable: "Permissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Permissions_RolesPermissionsId",
                table: "Roles",
                column: "RolesPermissionsId",
                principalTable: "Permissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
