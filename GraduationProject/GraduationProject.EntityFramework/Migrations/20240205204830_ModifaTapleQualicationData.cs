using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject.EntityFramework.Migrations
{
    public partial class ModifaTapleQualicationData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QualificationDatas_Staffs_StaffId",
                table: "QualificationDatas");

            migrationBuilder.DropForeignKey(
                name: "FK_QualificationDatas_Students_StudentId",
                table: "QualificationDatas");

            migrationBuilder.DropIndex(
                name: "IX_QualificationDatas_StudentId",
                table: "QualificationDatas");

            migrationBuilder.AlterColumn<int>(
                name: "StudentId",
                table: "QualificationDatas",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "StaffId",
                table: "QualificationDatas",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_QualificationDatas_StudentId",
                table: "QualificationDatas",
                column: "StudentId",
                unique: true,
                filter: "[StudentId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_QualificationDatas_Staffs_StaffId",
                table: "QualificationDatas",
                column: "StaffId",
                principalTable: "Staffs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QualificationDatas_Students_StudentId",
                table: "QualificationDatas",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QualificationDatas_Staffs_StaffId",
                table: "QualificationDatas");

            migrationBuilder.DropForeignKey(
                name: "FK_QualificationDatas_Students_StudentId",
                table: "QualificationDatas");

            migrationBuilder.DropIndex(
                name: "IX_QualificationDatas_StudentId",
                table: "QualificationDatas");

            migrationBuilder.AlterColumn<int>(
                name: "StudentId",
                table: "QualificationDatas",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "StaffId",
                table: "QualificationDatas",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_QualificationDatas_StudentId",
                table: "QualificationDatas",
                column: "StudentId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_QualificationDatas_Staffs_StaffId",
                table: "QualificationDatas",
                column: "StaffId",
                principalTable: "Staffs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QualificationDatas_Students_StudentId",
                table: "QualificationDatas",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
