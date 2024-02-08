namespace GraduationProject.Identity.IService
{
    public interface IAccountService
    {
        Task<string> AddStudentAccount(string NameArabic, string NameEnglish, string NationalID, string Email, string Password);
        Task<string> AddStaffAccount(string NameArabic, string NameEnglish, string NationalID, string Email, string Password);
        Task<string> AddAdministrationAccount(string NameArabic, string NameEnglish, string NationalID, string Email, string Password);
        Task<string> AddTeacherAccount(string NameArabic, string NameEnglish, string NationalID, string Email, string Password);
        Task<string> AddTeacherAssistantAccount(string NameArabic, string NameEnglish, string NationalID, string Email, string Password);
    }
}
