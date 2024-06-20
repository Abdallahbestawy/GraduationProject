using GraduationProject.Data.Enum;

namespace GraduationProject.Data.Models
{
    public class GetSchedulesForStaffByUserIdModel
    {
        public int SchedulesId { get; set; }
        public ScheduleDay ScheduleDay { get; set; }
        public ScheduleType ScheduleType { get; set; }
        public string FacultysName { get; set; }
        public string NameEnglish { get; set; }
        public string AcademyYear { get; set; }
        public string SchedulePlacesName { get; set; }
        public string Timing { get; set; }
        public string CoursesName { get; set; }
        public string CoursesCode { get; set; }
        public string ScientificDegreesName { get; set; }
        public string BandName { get; set; }
    }
}

