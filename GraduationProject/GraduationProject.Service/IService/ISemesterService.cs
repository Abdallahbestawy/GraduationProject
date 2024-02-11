using GraduationProject.Service.DataTransferObject.SemesterDto;

namespace GraduationProject.Service.IService
{
    public interface ISemesterService
    {
        Task AddSemesterAsync(SemesterDto addSemesterDto);
        Task UpdateSemesterAsync(SemesterDto updateSemesterDto);
        Task DeleteSemesterAsync(int SemesterId);
        Task<SemesterDto> GetSemesterByIdAsync(int SemesterId);
        Task<IQueryable<SemesterDto>> GetSemesterAsync();
    }
}
