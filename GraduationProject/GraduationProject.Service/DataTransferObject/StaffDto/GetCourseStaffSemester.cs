namespace GraduationProject.Service.DataTransferObject.StaffDto
{
    public class GetCourseStaffSemester
    {
        public int? StaffId { get; set; }
        public int AcademyYearId { get; set; }
        public List<int>? CourseIds { get; set; } = new List<int>();
    }
}
