using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject.EntityFramework.Migrations
{
    public partial class ModifyTapleScientificDegree : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SuccessPercentageCourse",
                table: "ScientificDegrees");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "SuccessPercentageCourse",
                table: "ScientificDegrees",
                type: "decimal(18,2)",
                nullable: true);
        }
    }
}
