using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.BandDto;
using System.Security.Claims;

namespace GraduationProject.Service.IService
{
    public interface IBandService
    {
        Task<Response<int>> AddBandAsync(BandDto addBandDto, ClaimsPrincipal user);
        Task<Response<int>> UpdateBandAsync(BandDto updateBandDto);
        Task<Response<int>> DeleteBandAsync(int bandId);
        Task<Response<BandDto>> GetBandByIdAsync(int bandId);
        Task<Response<IQueryable<BandDto>>> GetBandAsync();
        Task<Response<IQueryable<GetBandDto>>> GetBandByFacultyIdAsync(int facultyId);
    }
}
