using GraduationProject.Data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Service.DataTransferObject.ScheduleDto
{
    public class GetScheduleDetailsDto
    {
        public string FacultyName { get; set; }
        public string AcademyYearName { get; set; }
        public string ScientificDegreesName { get; set; }
        public List<GetScheduleDetailsInfoDto> GetScheduleDetailsInfo { get; set; } = new List<GetScheduleDetailsInfoDto>();
    }
    public class GetScheduleDetailsInfoDto
    {
        public ScheduleType ScheduleType { get; set; }
        public ScheduleDay ScheduleDay { get; set; }
        public string Timing { get; set; }
        public int Capacity { get; set; }
        public string StaffName { get; set; }
        public string SchedulePlacesName { get; set; }
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
    }
}
