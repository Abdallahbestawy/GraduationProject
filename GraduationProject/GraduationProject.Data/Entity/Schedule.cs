using GraduationProject.Data.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduationProject.Data.Entity
{
    public class Schedule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public ScheduleType ScheduleType { get; set; }
        public ScheduleDay ScheduleDay { get; set; }
        public TimeSpan TimeStart { get; set; }
        public TimeSpan EndStart { get; set; }
        public int Capacity { get; set; }
        public int? CurrentCapacity { get; set; }
        [ForeignKey("Faculty")]
        public int FacultyId { get; set; }
        public Faculty Faculty { get; set; }
        [ForeignKey("Staff")]
        public int StaffId { get; set; }
        public Staff Staff { get; set; }
        [ForeignKey("SchedulePlace")]
        public int SchedulePlaceId { get; set; }
        public SchedulePlace SchedulePlace { get; set; }
        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public Course Course { get; set; }
        [ForeignKey("AcademyYear")]
        public int AcademyYearId { get; set; }
        public AcademyYear AcademyYear { get; set; }
        [ForeignKey("ScientificDegree")]
        public int ScientificDegreeId { get; set; }
        public ScientificDegree ScientificDegree { get; set; }
        public virtual ICollection<StudentSchedule> ScheduleStudents { get; set; } = new List<StudentSchedule>();

    }
}
