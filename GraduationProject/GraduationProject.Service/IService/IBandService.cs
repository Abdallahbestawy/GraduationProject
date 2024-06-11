using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.BandDto;
using System.Security.Claims;

namespace GraduationProject.Service.IService
{
    public interface IBandService
    {
        Task<Response<int>> AddBandAsync(BandDto addBandDto, ClaimsPrincipal user);
        Task<Response<int>> UpdateBandAsync(BandDto updateBandDto, ClaimsPrincipal user);
        Task<Response<int>> DeleteBandAsync(int bandId, ClaimsPrincipal user);
        Task<Response<BandDto>> GetBandByIdAsync(int bandId);
        Task<Response<IQueryable<BandDto>>> GetBandAsync();
        Task<Response<IQueryable<GetBandDto>>> GetBandByFacultyIdAsync(int facultyId);
    }
}
