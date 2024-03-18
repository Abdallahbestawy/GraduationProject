using GraduationProject.Data.Entity;
using GraduationProject.Repository.IRepository;
using GraduationProject.Repository.Repository;
using GraduationProject.Service.DataTransferObject.LocationsDto;
using GraduationProject.Service.IService;

namespace GraduationProject.Service.Service
{
    public class LocationsService : ILocationsService
    {
        private readonly IUnitOfWork _unitOfWork;
        public LocationsService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }
        public async Task<bool> AddCountryAsync(CountryDto AddCountryDto)
        {
            try
            {
                Country newCountry = new Country
                {
                    Name = AddCountryDto.Name,
                };
                await _unitOfWork.Countries.AddAsync(newCountry);
                int result = await _unitOfWork.SaveAsync();
                if (result > 0)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }



        public async Task<List<CountryDto>> GetCountryAsync()
        {
            try
            {
                var countries = await _unitOfWork.Countries.GetAll();
                if (countries == null || !countries.Any())
                {
                    return null;
                }
                var result = countries.Select(country => new CountryDto
                {
                    Id = country.Id,
                    Name = country.Name
                }).ToList();
                return result;
            }
            catch
            {
                return null;
            }
        }

        public async Task<CountryDto> GetCountryByIdAsync(int countryId)
        {
            try
            {
                var country = await _unitOfWork.Countries.GetByIdAsync(countryId);
                if (country == null)
                {
                    return null;
                }
                CountryDto countryDto = new CountryDto
                {
                    Id = country.Id,
                    Name = country.Name
                };
                return countryDto;
            }
            catch
            {
                return null;
            }
        }



        public async Task<bool> UpdateCountryAsync(CountryDto updateCountryDto)
        {
            try
            {
                var existingCountry = await _unitOfWork.Countries.GetByIdAsync(updateCountryDto.Id);
                if (existingCountry == null)
                {
                    return false;
                }
                existingCountry.Name = updateCountryDto.Name;
                await _unitOfWork.Countries.Update(existingCountry);
                int result = await _unitOfWork.SaveAsync();
                if (result > 0)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> AddGovernorateAsync(List<GovernorateDto> AddGovernorateDto)
        {
            try
            {
                var newGovernorats = AddGovernorateDto.Select(g => new Governorate
                {
                    Name = g.Name,
                    CountryId = g.CountryId,
                }).ToList();
                await _unitOfWork.Governorates.AddRangeAsync(newGovernorats);
                int result = await _unitOfWork.SaveAsync();
                if (result > 0)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }

        }
        public async Task<List<GovernorateDto>> GetGovernorateAsync()
        {
            try
            {
                var governorates = await _unitOfWork.Governorates.GetAll();
                if (governorates == null || !governorates.Any())
                {
                    return null;
                }
                var governoratesDto = governorates.Select(g => new GovernorateDto
                {
                    Id = g.Id,
                    Name = g.Name,
                }).ToList();
                return governoratesDto;
            }
            catch
            {
                return null;
            }
        }
        public async Task<bool> AddCityAsync(List<CityDto> AddGCityDto)
        {
            try
            {
                var newCitys = AddGCityDto.Select(g => new City
                {
                    Name = g.Name,
                    GovernorateId = g.GovernorateId,
                }).ToList();
                await _unitOfWork.Cities.AddRangeAsync(newCitys);
                int result = await _unitOfWork.SaveAsync();
                if (result > 0)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        public async Task<List<CityDto>> GetCityAsync()
        {
            try
            {
                var citys = await _unitOfWork.Cities.GetAll();
                if (citys == null || !citys.Any())
                {
                    return null;
                }
                var citysDto = citys.Select(g => new CityDto
                {
                    Id = g.Id,
                    Name = g.Name,
                    GovernorateId = g.GovernorateId,
                }).ToList();
                return citysDto;
            }
            catch
            {
                return null;
            }
        }
    }
}
