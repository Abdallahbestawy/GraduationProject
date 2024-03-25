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
    }
}
