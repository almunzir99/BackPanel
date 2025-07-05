using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackPanel.Persistence.Migrations
{
    public partial class SeedDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "Username", "Email", "Phone", "CreatedAt", "LastUpdate", "PasswordHash", "PasswordSalt", "IsManager","Status" },
                values: new object[] { "Admin", "admin@gmail.com", "0128647019", new DateTime(2025, 1, 22, 21, 34, 16, 833, DateTimeKind.Local).AddTicks(5804), new DateTime(2025, 1, 22, 21, 34, 16, 833, DateTimeKind.Local).AddTicks(5810), new byte[] { 155, 162, 113, 132, 212, 231, 74, 130, 173, 165, 145, 136, 174, 120, 76, 187, 209, 4, 11, 63, 98, 207, 13, 77, 242, 47, 206, 18, 128, 120, 183, 192, 201, 226, 55, 179, 85, 48, 134, 54, 230, 210, 40, 26, 95, 144, 171, 109, 119, 255, 78, 166, 82, 223, 82, 231, 28, 111, 36, 78, 112, 59, 54, 195 }, new byte[] { 118, 6, 196, 243, 10, 133, 28, 22, 216, 207, 222, 95, 110, 212, 114, 193, 162, 139, 4, 83, 217, 247, 234, 131, 79, 115, 26, 20, 34, 215, 213, 37, 179, 156, 47, 191, 149, 103, 45, 93, 229, 5, 189, 121, 163, 202, 137, 83, 75, 222, 225, 198, 197, 156, 78, 134, 226, 40, 222, 26, 41, 40, 7, 173, 152, 97, 203, 103, 63, 57, 118, 92, 254, 107, 146, 176, 198, 133, 221, 64, 88, 238, 179, 231, 159, 177, 56, 32, 229, 141, 200, 153, 107, 78, 227, 86, 178, 126, 71, 231, 112, 170, 13, 33, 63, 106, 166, 132, 244, 70, 144, 18, 146, 119, 224, 83, 56, 155, 62, 198, 15, 75, 228, 5, 29, 24, 135, 206 }, true,1 });

            migrationBuilder.Sql(@"
                INSERT INTO [dbo].[Image]
                           ([Path]
                           ,[Status]
                           ,[CreatedAt]
                           ,[LastUpdate])
                     VALUES
                           ('images/logo.png'
                      ,1
                      ,'1/1/2023'
                      ,'1/1/2023')
                GO");
            migrationBuilder.Sql(@"
                                    INSERT INTO [dbo].[CompanyInfos]
                                               ([CompanyName]
                                               ,[Address]
                                               ,[LogoId]
                                               ,[Email]
                                               ,[PhoneNumber]
                                               ,[Fax]
                                               ,[AboutUs]
                                               ,[Status]
                                               ,[CreatedAt]
                                               ,[LastUpdate])
                                         VALUES
                                               ('Company1'
                                               ,'Company1'
                                               ,1
                                               ,'Company1@gmail.com'
                                               ,'0123456789'
                                               ,'0123456789'
                                               ,''
                                               ,1
                                               ,'1/1/2023'
                                               ,'1/1/2023')
                                    GO


");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
