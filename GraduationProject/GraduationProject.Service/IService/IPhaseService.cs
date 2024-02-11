using GraduationProject.Service.DataTransferObject.PhaseDto;

namespace GraduationProject.Service.IService
{
    public interface IPhaseService
    {
        Task AddPhaseAsync(PhaseDto addPhaseDto);
        Task UpdatePhaseAsync(PhaseDto updatePhaseDto);
        Task DeletePhaseAsync(int PhaseId);
        Task<PhaseDto> GetPhaseByIdAsync(int PhaseId);
        Task<IQueryable<PhaseDto>> GetPhaseAsync();
    }
}
