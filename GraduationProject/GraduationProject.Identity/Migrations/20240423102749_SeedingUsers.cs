using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject.Identity.Migrations
{
    public partial class SeedingUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string hashedPassword = PasswordHasher.HashPassword("Admin@12345!");

            migrationBuilder.Sql("INSERT INTO [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [NameArabic], [NameEnglish], [NationalID], [UserType]) " +
                                 "VALUES (N'506ce8ee-e266-420f-82a0-a5baef6690f5', N'30206063100058', N'30206063100058', N'admin@eduway.com', N'ADMIN@EDUWAY.COM', 0, N'" + hashedPassword + "', N'MQRWBECAXMENNE437USUYUTLKNUJ4T6K', N'eb8ea829-6f02-42f3-8b18-6d724bdc89ec', NULL, 0, 0, NULL, 1, 0, N'محمد عبدالله فهيم', N'Mohamed AbdAllA Fahem', N'30206063100058', 7)");

            migrationBuilder.Sql("INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES ('506ce8ee-e266-420f-82a0-a5baef6690f5', '478cbdcb-b896-4d94-8d26-34683f5a275a')");
        }


        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove the user
            migrationBuilder.Sql("DELETE FROM [dbo].[AspNetUsers] WHERE [Id] = '506ce8ee-e266-420f-82a0-a5baef6690f5'");

            // Remove the role assignment
            migrationBuilder.Sql("DELETE FROM [dbo].[AspNetUserRoles] WHERE [UserId] = '506ce8ee-e266-420f-82a0-a5baef6690f5'");
        }
    }

    public static class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            var passwordHasher = new PasswordHasher<object>();

            string hashedPassword = passwordHasher.HashPassword(null, password);

            return hashedPassword;
        }
    }
}
