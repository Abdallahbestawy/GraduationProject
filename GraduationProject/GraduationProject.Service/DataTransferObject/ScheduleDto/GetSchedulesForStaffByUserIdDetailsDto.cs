using GraduationProject.Data.Enum;

namespace GraduationProject.Service.DataTransferObject.ScheduleDto
{
    public class GetSchedulesForStaffByUserIdDetailsDto
    {
        public int SchedulesId { get; set; }
        public ScheduleDay ScheduleDay { get; set; }
        public ScheduleType ScheduleType { get; set; }
        public string Timing { get; set; }
        public string CoursesName { get; set; }
        public string CoursesCode { get; set; }
        public string SchedulePlacesName { get; set; }
        public string ScientificDegreesName { get; set; }
    }
}
