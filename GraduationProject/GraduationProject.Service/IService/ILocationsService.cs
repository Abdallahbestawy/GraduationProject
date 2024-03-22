using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.LocationsDto;

namespace GraduationProject.Service.IService
{
    public interface ILocationsService
    {
        Task<Response<bool>> AddCountryAsync(CountryDto AddCountryDto);
        Task<Response<List<CountryDto>>> GetCountryAsync();
        Task<Response<CountryDto>> GetCountryByIdAsync(int countryId);
        Task<Response<bool>> UpdateCountryAsync(CountryDto updateCountryDto);
        Task<Response<bool>> AddGovernorateAsync(List<GovernorateDto> AddGovernorateDto);
        Task<Response<List<GovernorateDto>>> GetGovernorateAsync();
        Task<Response<bool>> AddCityAsync(List<CityDto> AddGCityDto);
        Task<Response<List<CityDto>>> GetCityAsync();


    }
}
