using GraduationProject.Service.DataTransferObject.BandDto;

namespace GraduationProject.Service.IService
{
    public interface IBandService
    {
        Task AddBandAsync(BandDto addBandDto);
        Task UpdateBandAsync(BandDto updateBandDto);
        Task DeleteBandAsync(int bandId);
        Task<BandDto> GetBandByIdAsync(int bandId);
        Task<IQueryable<BandDto>> GetBandAsync();
    }
}
