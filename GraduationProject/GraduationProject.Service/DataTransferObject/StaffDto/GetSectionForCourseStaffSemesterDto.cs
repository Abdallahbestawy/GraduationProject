using GraduationProject.Data.Enum;

namespace GraduationProject.Service.DataTransferObject.StaffDto
{
    public class GetSectionForCourseStaffSemesterDto
    {
        public int SectionId { get; set; }
        public ScheduleDay ScheduleDay { get; set; }
        public string SectionTiming { get; set; }
    }
}
