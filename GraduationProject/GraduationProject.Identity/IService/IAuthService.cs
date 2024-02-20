using GraduationProject.Identity.Models;

namespace GraduationProject.Identity.IService
{
    public interface IAuthService
    {
        Task<AuthModel> LoginAsync(LoginUserModel loginUserModel);
        Task<AuthModel> RefreshTokenAsync(string token);
        Task<bool> RevokeTokenAsync(string token);
    }
}
