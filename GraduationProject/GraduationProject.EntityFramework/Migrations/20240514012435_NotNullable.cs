using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject.EntityFramework.Migrations
{
    public partial class NotNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Facultys_FacultyId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Staffs_Facultys_FacultyId",
                table: "Staffs");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Facultys_FacultyId",
                table: "Students");

            migrationBuilder.AlterColumn<int>(
                name: "FacultyId",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FacultyId",
                table: "Staffs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);


            migrationBuilder.AlterColumn<int>(
                name: "FacultyId",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Facultys_FacultyId",
                table: "Courses",
                column: "FacultyId",
                principalTable: "Facultys",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Staffs_Facultys_FacultyId",
                table: "Staffs",
                column: "FacultyId",
                principalTable: "Facultys",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Facultys_FacultyId",
                table: "Students",
                column: "FacultyId",
                principalTable: "Facultys",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Facultys_FacultyId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Staffs_Facultys_FacultyId",
                table: "Staffs");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Facultys_FacultyId",
                table: "Students");

            migrationBuilder.AlterColumn<int>(
                name: "FacultyId",
                table: "Students",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "FacultyId",
                table: "Staffs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "FacultyId",
                table: "Courses",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Facultys_FacultyId",
                table: "Courses",
                column: "FacultyId",
                principalTable: "Facultys",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Staffs_Facultys_FacultyId",
                table: "Staffs",
                column: "FacultyId",
                principalTable: "Facultys",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Facultys_FacultyId",
                table: "Students",
                column: "FacultyId",
                principalTable: "Facultys",
                principalColumn: "Id");
        }
    }
}
