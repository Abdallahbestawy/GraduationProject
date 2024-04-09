using GraduationProject.Data.Entity;
using GraduationProject.Mails.IService;
using GraduationProject.Mails.Models;
using GraduationProject.Repository.IRepository;
using GraduationProject.Repository.Repository;
using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.LocationsDto;
using GraduationProject.Service.IService;

namespace GraduationProject.Service.Service
{
    public class LocationsService : ILocationsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMailService _mailService;
        public LocationsService(UnitOfWork unitOfWork, IMailService mailService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mailService = mailService;
        }
        public async Task<Response<bool>> AddCountryAsync(CountryDto AddCountryDto)
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
                    return Response<bool>.Created("Country added successfully");

                return Response<bool>.ServerError("Error occured while adding country",
                     "An unexpected error occurred while adding country. Please try again later.");
            }
            catch(Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "LocationsService",
                    MethodName = "AddCountryAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<bool>.ServerError("Error occured while adding country",
                     "An unexpected error occurred while adding country. Please try again later.");
            }
        }

        public async Task<Response<List<CountryDto>>> GetCountryAsync()
        {
            try
            {
                var countries = await _unitOfWork.Countries.GetAll();

                if (countries == null || !countries.Any())
                    return Response<List<CountryDto>>.NoContent("No Countries are exist");

                var result = countries.Select(country => new CountryDto
                {
                    Id = country.Id,
                    Name = country.Name
                }).ToList();

                return Response<List<CountryDto>>.Success(result,"Countries are retrieved successfully").WithCount();
            }
            catch(Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "LocationsService",
                    MethodName = "GetCountryAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<List<CountryDto>>.ServerError("Error occured while retrieving countries",
                     "An unexpected error occurred while retrieving countries. Please try again later.");
            }
        }

        public async Task<Response<CountryDto>> GetCountryByIdAsync(int countryId)
        {
            try
            {
                var country = await _unitOfWork.Countries.GetByIdAsync(countryId);

                if (country == null)
                    return Response<CountryDto>.BadRequest("This country doesn't exist");
                
                CountryDto countryDto = new CountryDto
                {
                    Id = country.Id,
                    Name = country.Name
                };

                return Response<CountryDto>.Success(countryDto, "Country retrieved successfully").WithCount();
            }
            catch(Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "LocationsService",
                    MethodName = "GetCountryByIdAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<CountryDto>.ServerError("Error occured while retrieving country",
                     "An unexpected error occurred while retrieving country. Please try again later.");
            }
        }

        public async Task<Response<bool>> UpdateCountryAsync(CountryDto updateCountryDto)
        {
            try
            {
                var existingCountry = await _unitOfWork.Countries.GetByIdAsync(updateCountryDto.Id);

                if (existingCountry == null)
                    return Response<bool>.BadRequest("This country doesn't exist");

                existingCountry.Name = updateCountryDto.Name;
                await _unitOfWork.Countries.Update(existingCountry);
                int result = await _unitOfWork.SaveAsync();

                if (result > 0)
                    return Response<bool>.Updated("County updated successfully");

                return Response<bool>.ServerError("Error occured while updating country",
                     "An unexpected error occurred while updating country. Please try again later.");
            }
            catch(Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "LocationsService",
                    MethodName = "UpdateCountryAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<bool>.ServerError("Error occured while updating country",
                     "An unexpected error occurred while updating country. Please try again later.");
            }
        }

        public async Task<Response<bool>> AddGovernorateAsync(List<GovernorateDto> AddGovernorateDto)
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
                    return Response<bool>.Created("Governorate added successfully");

                return Response<bool>.ServerError("Error occured while adding governorate",
                     "An unexpected error occurred while adding governorate. Please try again later.");
            }
            catch(Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "LocationsService",
                    MethodName = "AddGovernorateAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<bool>.ServerError("Error occured while adding governorate",
                     "An unexpected error occurred while adding governorate. Please try again later.");
            }

        }

        public async Task<Response<List<GovernorateDto>>> GetGovernorateCountryIdAsync(int CountryId)
        {
            try
            {
                var governorates = await _unitOfWork.Governorates.GetEntityByPropertyAsync(gov => gov.CountryId == CountryId);

                if (governorates == null || !governorates.Any())
                    return Response<List<GovernorateDto>>.NoContent("No Governorates are exist");

                var governoratesDto = governorates.Select(g => new GovernorateDto
                {
                    Id = g.Id,
                    Name = g.Name,
                }).ToList();

                return Response<List<GovernorateDto>>.Success(governoratesDto, "Governorates retrieved successfully").WithCount();
            }
            catch(Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "LocationsService",
                    MethodName = "GetGovernorateCountryIdAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<List<GovernorateDto>>.ServerError("Error occured while retrieving governorates",
                     "An unexpected error occurred while retrieving governorates. Please try again later.");
            }
        }

        public async Task<Response<bool>> AddCityAsync(List<CityDto> AddGCityDto)
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
                    return Response<bool>.Created("City added successfully");

                return Response<bool>.ServerError("Error occured while adding city",
                     "An unexpected error occurred while adding city. Please try again later.");
            }
            catch(Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "LocationsService",
                    MethodName = "AddCityAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<bool>.ServerError("Error occured while adding city",
                     "An unexpected error occurred while adding city. Please try again later.");
            }
        }

        public async Task<Response<List<CityDto>>> GetCityByGovernorateIdAsync(int governorateId)
        {
            try
            {
                var citys = await _unitOfWork.Cities.GetEntityByPropertyAsync(city=>city.GovernorateId == governorateId);
                if (citys == null || !citys.Any())
                    return Response<List<CityDto>>.NoContent("No cities are exist");

                var citysDto = citys.Select(g => new CityDto
                {
                    Id = g.Id,
                    Name = g.Name,
                    GovernorateId = g.GovernorateId,
                }).ToList();

                return Response<List<CityDto>>.Success(citysDto,"Cities retrieved successfully").WithCount();
            }
            catch( Exception ex )
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "LocationsService",
                    MethodName = "GetCityByGovernorateIdAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<List<CityDto>>.ServerError("Error occured while retrieving cities",
                     "An unexpected error occurred while retrieving cities. Please try again later.");
            }
        }
    }
}
