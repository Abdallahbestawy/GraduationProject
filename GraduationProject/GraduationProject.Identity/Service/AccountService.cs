using GraduationProject.Identity.IService;
using GraduationProject.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace GraduationProject.Identity.Service
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public AccountService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<string> AddStudentAccount(string NameArabic, string NameEnglish, string NationalID, string Email, string Password)
        {
            ApplicationUser user = new ApplicationUser();
            user.NameArabic = NameArabic;
            user.NameEnglish = NameEnglish;
            user.NationalID = NationalID;
            user.Email = Email;
            user.UserName = NationalID;
            user.UserType = Enum.UserType.Student;

            IdentityResult result = await _userManager.CreateAsync(user, Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Student");
                return user.Id; // Return the generated UserId
            }
            else
            {
                return null; // Return null or handle the failure case accordingly
            }
        }
        public async Task<string> AddStaffAccount(string NameArabic, string NameEnglish, string NationalID, string Email, string Password)
        {
            ApplicationUser user = new ApplicationUser();
            user.NameArabic = NameArabic;
            user.NameEnglish = NameEnglish;
            user.NationalID = NationalID;
            user.Email = Email;
            user.UserName = NationalID;
            user.UserType = Enum.UserType.Staff;

            IdentityResult result = await _userManager.CreateAsync(user, Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Staff");
                return user.Id; // Return the generated UserId
            }
            else
            {
                return null; // Return null or handle the failure case accordingly
            }
        }

        public async Task<string> AddAdministrationAccount(string NameArabic, string NameEnglish, string NationalID, string Email, string Password)
        {
            ApplicationUser user = new ApplicationUser();
            user.NameArabic = NameArabic;
            user.NameEnglish = NameEnglish;
            user.NationalID = NationalID;
            user.Email = Email;
            user.UserName = NationalID;
            user.UserType = Enum.UserType.Administration;

            IdentityResult result = await _userManager.CreateAsync(user, Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Administration");
                return user.Id; // Return the generated UserId
            }
            else
            {
                return null; // Return null or handle the failure case accordingly
            }
        }

        public async Task<string> AddTeacherAccount(string NameArabic, string NameEnglish, string NationalID, string Email, string Password)
        {
            ApplicationUser user = new ApplicationUser();
            user.NameArabic = NameArabic;
            user.NameEnglish = NameEnglish;
            user.NationalID = NationalID;
            user.Email = Email;
            user.UserName = NationalID;
            user.UserType = Enum.UserType.Teacher;

            IdentityResult result = await _userManager.CreateAsync(user, Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Teacher");
                return user.Id; // Return the generated UserId
            }
            else
            {
                return null; // Return null or handle the failure case accordingly
            }
        }

        public async Task<string> AddTeacherAssistantAccount(string NameArabic, string NameEnglish, string NationalID, string Email, string Password)
        {
            ApplicationUser user = new ApplicationUser();
            user.NameArabic = NameArabic;
            user.NameEnglish = NameEnglish;
            user.NationalID = NationalID;
            user.Email = Email;
            user.UserName = NationalID;
            user.UserType = Enum.UserType.TeacherAssistant;

            IdentityResult result = await _userManager.CreateAsync(user, Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "TeacherAssistant");
                return user.Id; // Return the generated UserId
            }
            else
            {
                return null; // Return null or handle the failure case accordingly
            }
        }
    }
}
