namespace GraduationProject.Service.DataTransferObject.StudentDto
{
    public class GetAllStudentsDto
    {
        public int StudentId { get; set; }
        public string UserId { get; set; }
        public string StudentNameArbic { get; set; }
        public string StudentNameEnglish { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string Nationality { get; set; }
        public string Religion { get; set; }
    }
}
