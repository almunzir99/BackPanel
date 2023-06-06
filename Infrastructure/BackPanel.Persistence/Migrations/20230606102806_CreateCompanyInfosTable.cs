using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackPanel.Persistence.Migrations
{
    public partial class CreateCompanyInfosTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Image",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table => table.PrimaryKey("PK_Image", x => x.Id));

            migrationBuilder.CreateTable(
                name: "CompanyInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LogoId = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fax = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AboutUs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyInfos_Image_LogoId",
                        column: x => x.LogoId,
                        principalTable: "Image",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastUpdate", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2023, 6, 6, 13, 28, 6, 81, DateTimeKind.Local).AddTicks(5710), new DateTime(2023, 6, 6, 13, 28, 6, 81, DateTimeKind.Local).AddTicks(5730), new byte[] { 226, 104, 177, 124, 158, 57, 149, 29, 126, 217, 44, 126, 121, 189, 51, 46, 233, 240, 244, 101, 216, 110, 205, 239, 45, 145, 120, 234, 127, 110, 225, 100, 206, 124, 156, 250, 125, 165, 96, 231, 220, 70, 1, 28, 150, 96, 124, 64, 220, 54, 184, 245, 214, 186, 205, 10, 21, 25, 74, 67, 182, 135, 148, 115 }, new byte[] { 167, 250, 80, 114, 35, 129, 99, 98, 119, 147, 33, 108, 58, 192, 136, 112, 96, 248, 170, 53, 146, 52, 219, 2, 226, 146, 227, 219, 73, 39, 238, 56, 32, 91, 33, 208, 199, 219, 185, 127, 232, 167, 137, 240, 146, 30, 109, 136, 249, 235, 208, 104, 104, 219, 173, 72, 75, 49, 57, 116, 108, 119, 247, 73, 105, 13, 110, 47, 226, 49, 67, 186, 4, 88, 253, 26, 141, 112, 141, 63, 79, 12, 151, 164, 96, 72, 99, 224, 212, 202, 195, 83, 117, 80, 42, 192, 241, 78, 8, 34, 85, 166, 140, 105, 47, 23, 23, 190, 68, 68, 141, 200, 182, 108, 68, 244, 180, 118, 182, 21, 15, 165, 70, 86, 11, 176, 28, 230 } });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyInfos_LogoId",
                table: "CompanyInfos",
                column: "LogoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyInfos");

            migrationBuilder.DropTable(
                name: "Image");

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastUpdate", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2023, 3, 31, 14, 21, 22, 195, DateTimeKind.Local).AddTicks(1245), new DateTime(2023, 3, 31, 14, 21, 22, 195, DateTimeKind.Local).AddTicks(1255), new byte[] { 11, 74, 97, 131, 123, 123, 118, 84, 111, 165, 172, 247, 173, 242, 74, 29, 159, 255, 169, 214, 47, 119, 202, 252, 143, 76, 125, 103, 113, 4, 253, 188, 210, 215, 151, 213, 59, 171, 168, 112, 24, 90, 66, 117, 193, 14, 16, 254, 8, 71, 43, 198, 114, 97, 51, 30, 81, 55, 156, 62, 126, 151, 133, 40 }, new byte[] { 35, 0, 207, 154, 113, 185, 134, 50, 44, 65, 89, 13, 150, 116, 131, 123, 147, 216, 224, 176, 137, 228, 124, 44, 133, 58, 39, 145, 200, 246, 66, 243, 46, 225, 88, 200, 236, 34, 176, 125, 151, 171, 151, 228, 247, 69, 77, 180, 30, 178, 229, 210, 160, 76, 181, 186, 130, 213, 55, 75, 10, 21, 124, 106, 42, 31, 137, 252, 36, 99, 64, 40, 190, 253, 9, 195, 157, 149, 222, 197, 59, 173, 44, 27, 239, 242, 155, 193, 250, 36, 213, 162, 128, 143, 70, 193, 217, 88, 219, 109, 208, 248, 109, 116, 107, 218, 52, 141, 202, 93, 9, 134, 141, 98, 201, 95, 190, 125, 106, 183, 67, 16, 84, 143, 19, 86, 178, 181 } });
        }
    }
}
