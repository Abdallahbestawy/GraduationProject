using GraduationProject.Data.Enum;

namespace GraduationProject.Data.Models
{
    public class GetAllStudentsModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string NameArabic { get; set; }
        public string NameEnglish { get; set; }
        public string Email { get; set; }
        public Gender Gender { get; set; }
        public Nationality Nationality { get; set; }
        public Religion Religion { get; set; }
    }
}
