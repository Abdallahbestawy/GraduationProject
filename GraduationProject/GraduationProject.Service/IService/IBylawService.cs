using GraduationProject.Service.DataTransferObject.BylawDto;

namespace GraduationProject.Service.IService
{
    public interface IBylawService
    {
        Task AddBylawAsync(BylawDto addBylawDto);
        Task UpdateBylawAsync(BylawDto updateBylawDto);
        Task DeleteBylawAsync(int BylawId);
        Task<BylawDto> GetBylawByIdAsync(int BylawId);
        Task<IQueryable<BylawDto>> GetBylawAsync();
    }
}
