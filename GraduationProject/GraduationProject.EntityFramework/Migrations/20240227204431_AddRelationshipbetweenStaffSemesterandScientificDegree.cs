using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject.EntityFramework.Migrations
{
    public partial class AddRelationshipbetweenStaffSemesterandScientificDegree : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ScientificDegreeId",
                table: "StaffSemesters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_StaffSemesters_ScientificDegreeId",
                table: "StaffSemesters",
                column: "ScientificDegreeId");

            migrationBuilder.AddForeignKey(
                name: "FK_StaffSemesters_ScientificDegrees_ScientificDegreeId",
                table: "StaffSemesters",
                column: "ScientificDegreeId",
                principalTable: "ScientificDegrees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StaffSemesters_ScientificDegrees_ScientificDegreeId",
                table: "StaffSemesters");

            migrationBuilder.DropIndex(
                name: "IX_StaffSemesters_ScientificDegreeId",
                table: "StaffSemesters");

            migrationBuilder.DropColumn(
                name: "ScientificDegreeId",
                table: "StaffSemesters");
        }
    }
}
