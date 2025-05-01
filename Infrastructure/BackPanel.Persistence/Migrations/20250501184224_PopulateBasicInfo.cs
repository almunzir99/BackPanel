using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackPanel.Persistence.Migrations
{
    public partial class PopulateBasicInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Check if there are any Admins before inserting
            migrationBuilder.Sql(@"
IF NOT EXISTS (SELECT 1 FROM Admins)
BEGIN
    INSERT INTO Admins
        (Username, Email, Phone, CreatedAt, LastUpdate, PasswordHash, PasswordSalt, IsManager)
    VALUES
        (
            'Admin',
            'admin@gmail.com',
            '0128647019',
            '2025-01-22T21:34:16.833',
            '2025-01-22T21:34:16.833',
            0x9BA27184D4E74A82ADA59188AE784CBBD1040B3F62CF0D4DF22FCE128078B7C0C9E237B355308636E6D2281A5F90AB6D77FF4EA652DF52E71C6F244E703B36C3,
            0x7606C4F30A851C16D8CFDE5F6ED472C1A28B0453D9F7EA834F731A1422D7D525B39C2FBF95672D5DE505BD79A3CA89534BDEE1C6C59C4E86E228DE1A292807AD9861CB673F39765CFE6B92B0C685DD4058EEB3E79FB13820E58DC8996B4EE356B27E47E770AA0D213F6AA684F44690129277E053389B3EC60F4BE4051D1887CE,
            1
        )
END
");

            // Check if there are any rows in the Image table before inserting
            migrationBuilder.Sql(@"
IF NOT EXISTS (SELECT 1 FROM [dbo].[Image])
BEGIN
    INSERT INTO [dbo].[Image]
        ([Path], [Status], [CreatedAt], [LastUpdate])
    VALUES
        ('images/logo.png', 1, '2023-01-01', '2023-01-01')
END
");

            // Check if there are any rows in the CompanyInfos table before inserting
            migrationBuilder.Sql(@"
IF NOT EXISTS (SELECT 1 FROM [dbo].[CompanyInfos])
BEGIN
    INSERT INTO [dbo].[CompanyInfos]
        ([CompanyName], [Address], [LogoId], [Email], [PhoneNumber], [Fax], [AboutUs], [Status], [CreatedAt], [LastUpdate])
    VALUES
        ('Comapany', 'CompanyName', 1, 'email@gmail.com', '0128647019', '0128647019', '', 1, '2023-01-01', '2023-01-01')
END
");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
