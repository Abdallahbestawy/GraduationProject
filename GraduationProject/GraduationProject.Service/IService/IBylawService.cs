using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.BylawDto;
using System.Security.Claims;

namespace GraduationProject.Service.IService
{
    public interface IBylawService
    {
        Task<Response<int>> AddBylawAsync(BylawDto addBylawDto, ClaimsPrincipal user);
        Task<Response<int>> UpdateBylawAsync(BylawDto updateBylawDto, ClaimsPrincipal user);
        Task<Response<int>> DeleteBylawAsync(int BylawId, ClaimsPrincipal user);
        Task<Response<int>> DeleteEstimatesAsync(int estimatesId, ClaimsPrincipal user);
        Task<Response<int>> DeleteEstimatesCourseAsync(int estimatesCourseId, ClaimsPrincipal user);
        Task<Response<BylawDto>> GetBylawByIdAsync(int BylawId);
        Task<Response<IQueryable<BylawDto>>> GetBylawAsync();
        Task<Response<IQueryable<GetBylawDto>>> GetBylawByFacultyIdAsync(int facultyId);
    }
}
