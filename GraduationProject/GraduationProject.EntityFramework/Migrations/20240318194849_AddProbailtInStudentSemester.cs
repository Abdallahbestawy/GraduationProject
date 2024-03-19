using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject.EntityFramework.Migrations
{
    public partial class AddProbailtInStudentSemester : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Char",
                table: "StudentSemesters",
                type: "nvarchar(1)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Passing",
                table: "StudentSemesters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "Percentage",
                table: "StudentSemesters",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Total",
                table: "StudentSemesters",
                type: "decimal(18,2)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Char",
                table: "StudentSemesters");

            migrationBuilder.DropColumn(
                name: "Passing",
                table: "StudentSemesters");

            migrationBuilder.DropColumn(
                name: "Percentage",
                table: "StudentSemesters");

            migrationBuilder.DropColumn(
                name: "Total",
                table: "StudentSemesters");
        }
    }
}
