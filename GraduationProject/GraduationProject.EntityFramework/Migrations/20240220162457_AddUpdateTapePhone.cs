using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject.EntityFramework.Migrations
{
    public partial class AddUpdateTapePhone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Phones_Staffs_StaffId",
                table: "Phones");

            migrationBuilder.DropForeignKey(
                name: "FK_Phones_Students_StudentId",
                table: "Phones");

            migrationBuilder.AlterColumn<int>(
                name: "StudentId",
                table: "Phones",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "StaffId",
                table: "Phones",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Phones",
                type: "nvarchar(11)",
                maxLength: 11,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");


            migrationBuilder.AddForeignKey(
                name: "FK_Phones_Staffs_StaffId",
                table: "Phones",
                column: "StaffId",
                principalTable: "Staffs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Phones_Students_StudentId",
                table: "Phones",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Phones_Staffs_StaffId",
                table: "Phones");

            migrationBuilder.DropForeignKey(
                name: "FK_Phones_Students_StudentId",
                table: "Phones");

            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.AlterColumn<int>(
                name: "StudentId",
                table: "Phones",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "StaffId",
                table: "Phones",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Phones",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(11)",
                oldMaxLength: 11);

            migrationBuilder.AddForeignKey(
                name: "FK_Phones_Staffs_StaffId",
                table: "Phones",
                column: "StaffId",
                principalTable: "Staffs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Phones_Students_StudentId",
                table: "Phones",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
