using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject.EntityFramework.Migrations
{
    public partial class AddMinDegreeNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.AddColumn<decimal>(
                name: "MinDegree",
                table: "Courses",
                type: "decimal(18,2)",
                nullable: true);




        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {






            migrationBuilder.DropColumn(
                name: "MinDegree",
                table: "Courses");

        }
    }
}
