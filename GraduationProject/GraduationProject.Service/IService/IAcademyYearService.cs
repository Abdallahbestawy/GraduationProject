using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.AcademyYearDto;
using System.Security.Claims;

namespace GraduationProject.Service.IService
{
    public interface IAcademyYearService
    {
        Task<Response<int>> AddAcademyYearAsync(AcademyYearDto addAcademyYearDto, ClaimsPrincipal user);
        Task<Response<int>> UpdateAcademyYearAsync(AcademyYearDto updateAcademyYearDto, ClaimsPrincipal user);
        Task<Response<int>> DeleteAcademyYearAsync(int academyYearId, ClaimsPrincipal user);
        Task<Response<AcademyYearDto>> GetAcademyYearByIdAsync(int academyYearId);
        Task<Response<IQueryable<GetAcademyYearDto>>> GetAcademyYearAsync(int facultId);
        Task<Response<GetAcademyYearDto>> GetCurrentAcademyYearAsync(int facultId);
    }
}
