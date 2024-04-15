namespace GraduationProject.Service.DataTransferObject.CourseDto
{
    public class CourseStudentsAssessMethodDto
    {

        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        public List<StudentDto> StudentDtos { get; set; }
    }

    public class StudentDto
    {
        public string StudentName { get; set; }
        public string StudentCode { get; set; }

        public List<AssesstMethodDto> AssesstMethodDtos { get; set; }
    }

    public class AssesstMethodDto
    {
        public int StudentSemesterAssessMethodId { get; set; }
        public string AssessName { get; set; }
        public decimal? AssessDegree { get; set; }
    }
}


