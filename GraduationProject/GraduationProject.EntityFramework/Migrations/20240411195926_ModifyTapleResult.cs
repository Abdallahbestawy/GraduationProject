using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject.EntityFramework.Migrations
{
    public partial class ModifyTapleResult : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Results_Results_ResultId",
                table: "Results");

            migrationBuilder.DropIndex(
                name: "IX_Results_ResultId",
                table: "Results");

            migrationBuilder.DropColumn(
                name: "ResultId",
                table: "Results");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
