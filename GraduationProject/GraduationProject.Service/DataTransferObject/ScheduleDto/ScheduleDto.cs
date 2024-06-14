namespace GraduationProject.Service.DataTransferObject.ScheduleDto
{
    public class ScheduleDto
    {
        public int SemesterDegreeId { get; set; }
        public int AcademyYearId { get; set; }
        public List<ScheduleDetailsDto> ScheduleDetails { get; set; } = new List<ScheduleDetailsDto>();
    }
}
