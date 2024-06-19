using GraduationProject.Data.Enum;

namespace GraduationProject.Service.DataTransferObject.ScheduleDto
{
    public class GetStudentScheduleByUserIdDto
    {
        public ScheduleDay ScheduleDay { get; set; }
        public ScheduleType ScheduleType { get; set; }
        public string SchedulePlacesName { get; set; }
        public string Timing { get; set; }
        public string CoursesName { get; set; }
        public string CoursesCode { get; set; }
        public string NameDoctor { get; set; }
    }
}
