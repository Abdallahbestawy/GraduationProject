using GraduationProject.Service.DataTransferObject.AssessMethodDto;

namespace GraduationProject.Service.IService
{
    public interface IAssessMethodService
    {
        Task AddAssessMethodAsync(AssessMethodDto addAssessMethodDto);
        Task UpdateAssessMethodAsync(AssessMethodDto updateAssessMethodDto);
        Task DeleteAssessMethodAsync(int assessMethodId);
        Task<AssessMethodDto> GetAssessMethodByIdAsync(int assessMethodId);
        Task<IQueryable<AssessMethodDto>> GetAssessMethodAsync();
    }
}
