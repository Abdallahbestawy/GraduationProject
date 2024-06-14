using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject.EntityFramework.Migrations
{
    public partial class AddTableFKInSchedule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ScientificDegreeId",
                table: "Schedules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_ScientificDegreeId",
                table: "Schedules",
                column: "ScientificDegreeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_ScientificDegrees_ScientificDegreeId",
                table: "Schedules",
                column: "ScientificDegreeId",
                principalTable: "ScientificDegrees",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_ScientificDegrees_ScientificDegreeId",
                table: "Schedules");

            migrationBuilder.DropIndex(
                name: "IX_Schedules_ScientificDegreeId",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "ScientificDegreeId",
                table: "Schedules");
        }
    }
}
