using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject.EntityFramework.Migrations
{
    public partial class ModifyTapleResultAndStudentSemester : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Results_AcademyYears_AcademyYearId",
                table: "Results");

            migrationBuilder.DropForeignKey(
                name: "FK_Results_Departments_DepartmentId",
                table: "Results");

            migrationBuilder.DropForeignKey(
                name: "FK_Results_ScientificDegrees_ScientificDegreeId",
                table: "Results");

            migrationBuilder.DropForeignKey(
                name: "FK_Results_Students_StudentId",
                table: "Results");

            migrationBuilder.DropIndex(
                name: "IX_Results_AcademyYearId",
                table: "Results");

            migrationBuilder.DropIndex(
                name: "IX_Results_DepartmentId",
                table: "Results");

            migrationBuilder.DropIndex(
                name: "IX_Results_ScientificDegreeId",
                table: "Results");

            migrationBuilder.DropColumn(
                name: "Char",
                table: "StudentSemesters");

            migrationBuilder.DropColumn(
                name: "Percentage",
                table: "StudentSemesters");

            migrationBuilder.DropColumn(
                name: "AcademyYearId",
                table: "Results");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Results");

            migrationBuilder.DropColumn(
                name: "ScientificDegreeId",
                table: "Results");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "Results",
                newName: "StudentSemesterId");

            migrationBuilder.RenameColumn(
                name: "GPA",
                table: "Results",
                newName: "PercentageTotal");

            migrationBuilder.RenameIndex(
                name: "IX_Results_StudentId",
                table: "Results",
                newName: "IX_Results_StudentSemesterId");

            migrationBuilder.AddColumn<string>(
                name: "Char",
                table: "Results",
                type: "nvarchar(1)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CharTotal",
                table: "Results",
                type: "nvarchar(1)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Percentage",
                table: "Results",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "ResultId",
                table: "Results",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Results_ResultId",
                table: "Results",
                column: "ResultId");

            migrationBuilder.AddForeignKey(
                name: "FK_Results_Results_ResultId",
                table: "Results",
                column: "ResultId",
                principalTable: "Results",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Results_StudentSemesters_StudentSemesterId",
                table: "Results",
                column: "StudentSemesterId",
                principalTable: "StudentSemesters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Results_Results_ResultId",
                table: "Results");

            migrationBuilder.DropForeignKey(
                name: "FK_Results_StudentSemesters_StudentSemesterId",
                table: "Results");

            migrationBuilder.DropIndex(
                name: "IX_Results_ResultId",
                table: "Results");

            migrationBuilder.DropColumn(
                name: "Char",
                table: "Results");

            migrationBuilder.DropColumn(
                name: "CharTotal",
                table: "Results");

            migrationBuilder.DropColumn(
                name: "Percentage",
                table: "Results");

            migrationBuilder.DropColumn(
                name: "ResultId",
                table: "Results");

            migrationBuilder.RenameColumn(
                name: "StudentSemesterId",
                table: "Results",
                newName: "StudentId");

            migrationBuilder.RenameColumn(
                name: "PercentageTotal",
                table: "Results",
                newName: "GPA");

            migrationBuilder.RenameIndex(
                name: "IX_Results_StudentSemesterId",
                table: "Results",
                newName: "IX_Results_StudentId");

            migrationBuilder.AddColumn<string>(
                name: "Char",
                table: "StudentSemesters",
                type: "nvarchar(1)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Percentage",
                table: "StudentSemesters",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AcademyYearId",
                table: "Results",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "Results",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ScientificDegreeId",
                table: "Results",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Results_AcademyYearId",
                table: "Results",
                column: "AcademyYearId");

            migrationBuilder.CreateIndex(
                name: "IX_Results_DepartmentId",
                table: "Results",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Results_ScientificDegreeId",
                table: "Results",
                column: "ScientificDegreeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Results_AcademyYears_AcademyYearId",
                table: "Results",
                column: "AcademyYearId",
                principalTable: "AcademyYears",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Results_Departments_DepartmentId",
                table: "Results",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Results_ScientificDegrees_ScientificDegreeId",
                table: "Results",
                column: "ScientificDegreeId",
                principalTable: "ScientificDegrees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Results_Students_StudentId",
                table: "Results",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
