using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject.EntityFramework.Migrations
{
    public partial class ModifaTaplescientifidegree : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScientificDegrees_Bands_BandId",
                table: "ScientificDegrees");

            migrationBuilder.DropForeignKey(
                name: "FK_ScientificDegrees_Bylaws_BylawId",
                table: "ScientificDegrees");

            migrationBuilder.DropForeignKey(
                name: "FK_ScientificDegrees_ExamRoles_ExamRoleId",
                table: "ScientificDegrees");

            migrationBuilder.DropForeignKey(
                name: "FK_ScientificDegrees_phases_PhaseId",
                table: "ScientificDegrees");

            migrationBuilder.DropForeignKey(
                name: "FK_ScientificDegrees_Semesters_SemesterId",
                table: "ScientificDegrees");

            migrationBuilder.AlterColumn<int>(
                name: "SemesterId",
                table: "ScientificDegrees",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "PhaseId",
                table: "ScientificDegrees",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ExamRoleId",
                table: "ScientificDegrees",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "BylawId",
                table: "ScientificDegrees",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "BandId",
                table: "ScientificDegrees",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ScientificDegrees_Bands_BandId",
                table: "ScientificDegrees",
                column: "BandId",
                principalTable: "Bands",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ScientificDegrees_Bylaws_BylawId",
                table: "ScientificDegrees",
                column: "BylawId",
                principalTable: "Bylaws",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ScientificDegrees_ExamRoles_ExamRoleId",
                table: "ScientificDegrees",
                column: "ExamRoleId",
                principalTable: "ExamRoles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ScientificDegrees_phases_PhaseId",
                table: "ScientificDegrees",
                column: "PhaseId",
                principalTable: "phases",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ScientificDegrees_Semesters_SemesterId",
                table: "ScientificDegrees",
                column: "SemesterId",
                principalTable: "Semesters",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScientificDegrees_Bands_BandId",
                table: "ScientificDegrees");

            migrationBuilder.DropForeignKey(
                name: "FK_ScientificDegrees_Bylaws_BylawId",
                table: "ScientificDegrees");

            migrationBuilder.DropForeignKey(
                name: "FK_ScientificDegrees_ExamRoles_ExamRoleId",
                table: "ScientificDegrees");

            migrationBuilder.DropForeignKey(
                name: "FK_ScientificDegrees_phases_PhaseId",
                table: "ScientificDegrees");

            migrationBuilder.DropForeignKey(
                name: "FK_ScientificDegrees_Semesters_SemesterId",
                table: "ScientificDegrees");

            migrationBuilder.AlterColumn<int>(
                name: "SemesterId",
                table: "ScientificDegrees",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PhaseId",
                table: "ScientificDegrees",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ExamRoleId",
                table: "ScientificDegrees",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BylawId",
                table: "ScientificDegrees",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BandId",
                table: "ScientificDegrees",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ScientificDegrees_Bands_BandId",
                table: "ScientificDegrees",
                column: "BandId",
                principalTable: "Bands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScientificDegrees_Bylaws_BylawId",
                table: "ScientificDegrees",
                column: "BylawId",
                principalTable: "Bylaws",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScientificDegrees_ExamRoles_ExamRoleId",
                table: "ScientificDegrees",
                column: "ExamRoleId",
                principalTable: "ExamRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScientificDegrees_phases_PhaseId",
                table: "ScientificDegrees",
                column: "PhaseId",
                principalTable: "phases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScientificDegrees_Semesters_SemesterId",
                table: "ScientificDegrees",
                column: "SemesterId",
                principalTable: "Semesters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
