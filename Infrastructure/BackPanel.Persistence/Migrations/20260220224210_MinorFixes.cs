using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackPanel.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MinorFixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Admins");

            migrationBuilder.DropColumn(
                name: "PasswordSalt",
                table: "Admins");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Admins",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "Admins",
                newName: "PhoneNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Admins",
                newName: "Username");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Admins",
                newName: "Phone");

            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordHash",
                table: "Admins",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordSalt",
                table: "Admins",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}
