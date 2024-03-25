using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.AssessMethodDto;

namespace GraduationProject.Service.IService
{
    public interface IAssessMethodService
    {
        Task<Response<int>> AddAssessMethodAsync(AssessMethodDto addAssessMethodDto);
        Task<Response<int>> UpdateAssessMethodAsync(AssessMethodDto updateAssessMethodDto);
        Task<Response<int>> DeleteAssessMethodAsync(int assessMethodId);
        Task<Response<AssessMethodDto>> GetAssessMethodByIdAsync(int assessMethodId);
        Task<Response<IQueryable<GetAssessMethodDto>>> GetAssessMethodAsync(int facultyId);
    }
}
