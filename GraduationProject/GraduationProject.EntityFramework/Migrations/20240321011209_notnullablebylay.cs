using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject.EntityFramework.Migrations
{
    public partial class notnullablebylay : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScientificDegrees_Bylaws_BylawId",
                table: "ScientificDegrees");

            migrationBuilder.AlterColumn<int>(
                name: "BylawId",
                table: "ScientificDegrees",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ScientificDegrees_Bylaws_BylawId",
                table: "ScientificDegrees",
                column: "BylawId",
                principalTable: "Bylaws",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScientificDegrees_Bylaws_BylawId",
                table: "ScientificDegrees");

            migrationBuilder.AlterColumn<int>(
                name: "BylawId",
                table: "ScientificDegrees",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ScientificDegrees_Bylaws_BylawId",
                table: "ScientificDegrees",
                column: "BylawId",
                principalTable: "Bylaws",
                principalColumn: "Id");
        }
    }
}
