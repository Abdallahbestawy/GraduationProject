using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject.EntityFramework.Migrations
{
    public partial class AddTableSchedule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Schedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScheduleType = table.Column<int>(type: "int", nullable: false),
                    ScheduleDay = table.Column<int>(type: "int", nullable: false),
                    TimeStart = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndStart = table.Column<TimeSpan>(type: "time", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    FacultyId = table.Column<int>(type: "int", nullable: false),
                    StaffId = table.Column<int>(type: "int", nullable: false),
                    SchedulePlaceId = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    AcademyYearId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Schedules_AcademyYears_AcademyYearId",
                        column: x => x.AcademyYearId,
                        principalTable: "AcademyYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Schedules_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Schedules_Facultys_FacultyId",
                        column: x => x.FacultyId,
                        principalTable: "Facultys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Schedules_SchedulePlaces_SchedulePlaceId",
                        column: x => x.SchedulePlaceId,
                        principalTable: "SchedulePlaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Schedules_Staffs_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Staffs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_AcademyYearId",
                table: "Schedules",
                column: "AcademyYearId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_CourseId",
                table: "Schedules",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_FacultyId",
                table: "Schedules",
                column: "FacultyId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_SchedulePlaceId",
                table: "Schedules",
                column: "SchedulePlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_StaffId",
                table: "Schedules",
                column: "StaffId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Schedules");
        }
    }
}
