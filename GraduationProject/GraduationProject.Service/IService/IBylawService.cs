using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.BylawDto;

namespace GraduationProject.Service.IService
{
    public interface IBylawService
    {
        Task AddBylawAsync(BylawDto addBylawDto);
        Task<Response<int>> UpdateBylawAsync(BylawDto updateBylawDto);
        Task<Response<int>> DeleteBylawAsync(int BylawId);
        Task<Response<BylawDto>> GetBylawByIdAsync(int BylawId);
        Task<Response<IQueryable<BylawDto>>> GetBylawAsync();
    }
}
