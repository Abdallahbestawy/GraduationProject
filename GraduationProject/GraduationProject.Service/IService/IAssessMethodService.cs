using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.AssessMethodDto;
using System.Security.Claims;

namespace GraduationProject.Service.IService
{
    public interface IAssessMethodService
    {
        Task<Response<int>> AddAssessMethodAsync(AssessMethodDto addAssessMethodDto, ClaimsPrincipal user);
        Task<Response<int>> UpdateAssessMethodAsync(AssessMethodDto updateAssessMethodDto, ClaimsPrincipal user);
        Task<Response<int>> DeleteAssessMethodAsync(int assessMethodId, ClaimsPrincipal user);
        Task<Response<AssessMethodDto>> GetAssessMethodByIdAsync(int assessMethodId);
        Task<Response<IQueryable<GetAssessMethodDto>>> GetAssessMethodAsync(int facultyId);
    }
}
