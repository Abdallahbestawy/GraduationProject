using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject.Identity.Migrations
{
    public partial class SeedingRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //Admin role
            migrationBuilder.Sql("INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'478cbdcb-b896-4d94-8d26-34683f5a275a', N'Admin', N'ADMIN', N'2542e710-ffa2-49d3-bcc6-fa96e934a1c8')");
            //Administration role
            migrationBuilder.Sql("INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'0d2515d8-c3ee-4fda-b5ea-df5b61c23328', N'Administration', N'ADMINISTRATION', N'404e4e75-a4fe-445f-9928-f5f7f3016461')");
            //Teacher role
            migrationBuilder.Sql("INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'107c0982-db64-4e4d-b043-195766f9fd3d', N'Teacher', N'TEACHER', N'b12b049d-25c8-4d51-81a9-ede6cdc8c366')");
            //TeacherAssistant role
            migrationBuilder.Sql("INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'4994c349-2b14-4de0-9262-5e711fd4ee20', N'TeacherAssistant', N'TEACHERASSISTANT', N'9d8a790f-9694-433a-9569-4c952a51af45')");
            //Student role
            migrationBuilder.Sql("INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'586b6801-aef2-4267-9fde-1303450fffc0', N'Student', N'STUDENT', N'78aacd30-4617-4e9b-b7bb-44d2a9492bea')");
            //Staff role
            migrationBuilder.Sql("INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'5f21122b-56f2-4911-a74d-f3cc3cbe9ddf', N'Staff', N'STAFF', N'b9c8d88b-839a-4507-9c11-7078701246a7')");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [dbo].[AspNetRoles] WHERE [Name] = 'Admin'");
            migrationBuilder.Sql("DELETE FROM [dbo].[AspNetRoles] WHERE [Name] = 'Administration'");
            migrationBuilder.Sql("DELETE FROM [dbo].[AspNetRoles] WHERE [Name] = 'Teacher'");
            migrationBuilder.Sql("DELETE FROM [dbo].[AspNetRoles] WHERE [Name] = 'TeacherAssistant'");
            migrationBuilder.Sql("DELETE FROM [dbo].[AspNetRoles] WHERE [Name] = 'Student'");
            migrationBuilder.Sql("DELETE FROM [dbo].[AspNetRoles] WHERE [Name] = 'Staff'");
        }
    }
}
