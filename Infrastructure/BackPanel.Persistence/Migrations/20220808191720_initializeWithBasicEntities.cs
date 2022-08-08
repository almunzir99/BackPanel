using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackPanel.Persistence.Migrations
{
    public partial class initializeWithBasicEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Create = table.Column<bool>(type: "bit", nullable: false),
                    Read = table.Column<bool>(type: "bit", nullable: false),
                    Update = table.Column<bool>(type: "bit", nullable: false),
                    Delete = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MessagesPermissionsId = table.Column<int>(type: "int", nullable: true),
                    AdminsPermissionsId = table.Column<int>(type: "int", nullable: true),
                    RolesPermissionsId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Roles_Permissions_AdminsPermissionsId",
                        column: x => x.AdminsPermissionsId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Roles_Permissions_MessagesPermissionsId",
                        column: x => x.MessagesPermissionsId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Roles_Permissions_RolesPermissionsId",
                        column: x => x.RolesPermissionsId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsManager = table.Column<bool>(type: "bit", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    Username = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Photo = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Activity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    EffectedTable = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EffectedRowId = table.Column<int>(type: "int", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AdminId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Activity_Users_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Module = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Read = table.Column<bool>(type: "bit", nullable: false),
                    GroupedItem = table.Column<int>(type: "int", nullable: true),
                    AdminId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Email", "Image", "IsManager", "LastUpdate", "PasswordHash", "PasswordSalt", "Phone", "Photo", "RoleId", "Username" },
                values: new object[] { 1, new DateTime(2022, 8, 8, 21, 17, 20, 37, DateTimeKind.Local).AddTicks(5239), null, "almunzir99@gmail.com", null, true, new DateTime(2022, 8, 8, 21, 17, 20, 37, DateTimeKind.Local).AddTicks(5247), new byte[] { 34, 244, 156, 255, 137, 230, 196, 12, 114, 19, 40, 206, 4, 66, 220, 94, 91, 222, 196, 89, 207, 46, 158, 220, 106, 134, 194, 139, 77, 117, 246, 26, 120, 160, 85, 48, 4, 205, 221, 166, 150, 39, 31, 192, 0, 165, 133, 233, 248, 112, 237, 8, 147, 114, 239, 119, 9, 182, 187, 41, 243, 237, 161, 122 }, new byte[] { 53, 50, 8, 78, 88, 33, 101, 194, 31, 243, 110, 9, 230, 61, 205, 181, 143, 24, 12, 203, 55, 92, 102, 161, 228, 132, 19, 220, 65, 214, 55, 142, 77, 54, 25, 62, 62, 129, 93, 77, 243, 180, 140, 158, 96, 174, 23, 238, 64, 197, 230, 55, 166, 67, 158, 22, 253, 201, 30, 115, 165, 179, 230, 76, 112, 14, 108, 95, 204, 75, 192, 224, 195, 251, 108, 219, 45, 74, 162, 29, 166, 4, 209, 217, 59, 113, 90, 86, 92, 112, 233, 217, 190, 226, 223, 169, 64, 181, 151, 180, 73, 27, 232, 59, 145, 11, 112, 242, 116, 21, 23, 89, 107, 255, 167, 41, 151, 107, 20, 215, 218, 196, 220, 95, 180, 193, 209, 79 }, "249128647019", null, null, "almunzir99" });

            migrationBuilder.CreateIndex(
                name: "IX_Activity_AdminId",
                table: "Activity",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_AdminId",
                table: "Notifications",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_AdminsPermissionsId",
                table: "Roles",
                column: "AdminsPermissionsId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_MessagesPermissionsId",
                table: "Roles",
                column: "MessagesPermissionsId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_RolesPermissionsId",
                table: "Roles",
                column: "RolesPermissionsId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Title",
                table: "Roles",
                column: "Title",
                unique: true,
                filter: "[Title] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Activity");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Permissions");
        }
    }
}
