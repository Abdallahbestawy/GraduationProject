namespace GraduationProject.Service.DataTransferObject.ScheduleDto
{
    public class GetSchedulesForStaffByUserIdDto
    {
        public string FacultysName { get; set; }
        public string NameEnglish { get; set; }
        public string AcademyYearName { get; set; }
        public List<GetSchedulesForStaffByUserIdDetailsDto> getSchedulesForStaffByUserIdDetails { get; set; } = new List<GetSchedulesForStaffByUserIdDetailsDto>();


    }

}
