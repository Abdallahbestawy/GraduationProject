using GraduationProject.Identity.Models;
using System.Security.Claims;

namespace GraduationProject.Identity.IService
{
    public interface IAccountService
    {
        Task<string> AddStudentAccount(string NameArabic, string NameEnglish, string NationalID, string Email, string Password);
        Task<string> AddStaffAccount(string NameArabic, string NameEnglish, string NationalID, string Email, string Password);
        Task<string> AddAdministrationAccount(string NameArabic, string NameEnglish, string NationalID, string Email, string Password);
        Task<string> AddTeacherAccount(string NameArabic, string NameEnglish, string NationalID, string Email, string Password);
        Task<string> AddTeacherAssistantAccount(string NameArabic, string NameEnglish, string NationalID, string Email, string Password);
        Task<string> AddControlMembers(string NameArabic, string NameEnglish, string NationalID, string Email, string Password);
        Task<bool> DeleteUser(string userId);
        Task<string> GetUserIdByUser(ClaimsPrincipal user);
        Task<bool> UpdateUser(string userId, string NameArabic, string NameEnglish, string NationalID);
        Task<ApplicationUser> GetUser(ClaimsPrincipal user);
        Task<ApplicationUser> GetUserByUserId(string userId);
    }

}
