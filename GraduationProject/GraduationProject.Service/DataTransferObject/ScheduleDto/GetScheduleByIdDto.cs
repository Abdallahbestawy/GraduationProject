using GraduationProject.Data.Enum;

namespace GraduationProject.Service.DataTransferObject.ScheduleDto
{
    public class GetScheduleByIdDto
    {
        public int Id { get; set; }
        public ScheduleType ScheduleType { get; set; }
        public ScheduleDay ScheduleDay { get; set; }
        public int StartHour { get; set; }
        public int StartMinute { get; set; }
        public int EndHour { get; set; }
        public int EndMinute { get; set; }
        public int Capacity { get; set; }
        public int FacultyId { get; set; }

        public int StaffId { get; set; }
        public int SchedulePlaceId { get; set; }
        public int CourseId { get; set; }
        public int AcademyYearId { get; set; }
        public int ScientificDegreeId { get; set; }
    }
}
