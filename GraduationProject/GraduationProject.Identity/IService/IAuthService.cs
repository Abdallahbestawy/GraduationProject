using GraduationProject.Identity.Models;
using GraduationProject.ResponseHandler.Model;

namespace GraduationProject.Identity.IService
{
    public interface IAuthService
    {
        Task<AuthModel> LoginAsync(LoginUserModel loginUserModel);
        Task<AuthModel> RefreshTokenAsync(string token);
        Task<bool> RevokeTokenAsync(string token);
        Task<Response<bool>> ForgotPassword(ForgotPasswordModel forgotPasswordModel, string baseURL);
        Task<Response<bool>> ResetPassword(ResetPasswordModel resetPasswordModel);
        Task<Response<bool>> ChangeUserRolesAsync(UserRolesDto model);
        Task<Response<UserRolesDto>> GetUserRolesAsync(string userId);
        Task<Response<bool>> ChangePassword(ChangePasswordModel changePasswordModel, ApplicationUser user);
        Task Logout();
    }
}
