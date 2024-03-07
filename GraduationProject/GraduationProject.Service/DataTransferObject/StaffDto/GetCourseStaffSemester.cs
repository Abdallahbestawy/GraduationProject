namespace GraduationProject.Service.DataTransferObject.StaffDto
{
    public class GetCourseStaffSemester
    {
        public int? StaffId { get; set; }
        public int AcademyYearId { get; set; }
        public List<CourseDoctorDto>? CourseDoctorDtos { get; set; } = new List<CourseDoctorDto>();
    }
    public class CourseDoctorDto
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
    }
}
