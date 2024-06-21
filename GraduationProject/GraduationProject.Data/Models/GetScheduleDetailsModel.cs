using GraduationProject.Data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Data.Models
{
    public class GetScheduleDetailsModel
    {
        public int SchedulesId { get; set; }
        public ScheduleType ScheduleType { get; set; }
        public ScheduleDay ScheduleDay { get; set; }
        public string Timing { get; set; }
        public int Capacity { get; set; }
        public string FacultyName { get; set; }
        public string StaffName { get; set; }
        public string SchedulePlacesName { get; set; }
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        public string AcademyYear { get; set; }
        public string ScientificDegreesName { get; set; }
        public string BandName { get; set; }
    }
}
