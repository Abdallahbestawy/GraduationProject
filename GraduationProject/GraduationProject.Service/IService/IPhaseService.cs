using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.PhaseDto;

namespace GraduationProject.Service.IService
{
    public interface IPhaseService
    {
        Task<Response<int>> AddPhaseAsync(PhaseDto addPhaseDto);
        Task<Response<int>> UpdatePhaseAsync(PhaseDto updatePhaseDto);
        Task<Response<int>> DeletePhaseAsync(int PhaseId);
        Task<Response<PhaseDto>> GetPhaseByIdAsync(int PhaseId);
        Task<Response<IQueryable<PhaseDto>>> GetPhaseAsync();
        Task<Response<IQueryable<GetPhaseDto>>> GetPhaseByFacultyIdAsync(int facultyId);
    }
}
