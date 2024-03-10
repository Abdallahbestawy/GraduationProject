namespace GraduationProject.Service.DataTransferObject.StaffDto
{
    public class GetAllStaffsDto
    {
        public int StaffId { get; set; }
        public string UserId { get; set; }
        public string StaffNameArbic { get; set; }
        public string StaffNameEnglish { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string Nationality { get; set; }
        public string Religion { get; set; }
    }
}
