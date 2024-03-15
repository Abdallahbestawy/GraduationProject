using GraduationProject.Mails.IService;
using GraduationProject.Mails.Models;
using GraduationProject.Repository.IRepository;
using GraduationProject.Repository.Repository;
using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.CityDto;
using GraduationProject.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Service.Service
{
    public class CityService : ICityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMailService _mailService;
        public CityService(UnitOfWork unitOfWork, IMailService mailService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mailService = mailService;
        }
        public async Task<Response<List<CityDto>>> GetCitiesByGovernorateId(int governorateId)
        {
            try
            {
                if(await _unitOfWork.Governorates.GetByIdAsync(governorateId) == null)
                    return Response<List<CityDto>>.BadRequest("This Governorate doesn't exist");

                var cities = await _unitOfWork.Cities.GetEntityByPropertyAsync(city=>city.GovernorateId == governorateId);

                if (!cities.Any())
                    return Response<List<CityDto>>.NoContent("No Cities are exist in this governorate");

                List<CityDto> result = cities.Select(city => new CityDto
                {
                    Id = city.Id,
                    Name = city.Name,
                }).ToList();
                return Response<List<CityDto>>.Success(result,"Cities retrieved successfully").WithCount();
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "CityService",
                    MethodName = "GetCitiesByGovernorateId",
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
