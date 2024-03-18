using GraduationProject.Service.DataTransferObject.LocationsDto;

namespace GraduationProject.Service.IService
{
    public interface ILocationsService
    {
        Task<bool> AddCountryAsync(CountryDto AddCountryDto);
        Task<List<CountryDto>> GetCountryAsync();
        Task<CountryDto> GetCountryByIdAsync(int countryId);
        Task<bool> UpdateCountryAsync(CountryDto updateCountryDto);
        Task<bool> AddGovernorateAsync(List<GovernorateDto> AddGovernorateDto);
        Task<List<GovernorateDto>> GetGovernorateAsync();
        Task<bool> AddCityAsync(List<CityDto> AddGCityDto);
        Task<List<CityDto>> GetCityAsync();


    }
}
