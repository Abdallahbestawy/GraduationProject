using GraduationProject.Mails.IService;
using GraduationProject.Mails.Models;
using GraduationProject.Repository.IRepository;
using GraduationProject.Repository.Repository;
using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.CountryDto;
using GraduationProject.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Service.Service
{
    public class CountryService : ICountryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMailService _mailService;

        public CountryService(UnitOfWork unitOfWork, IMailService mailService)
        {
            _unitOfWork = unitOfWork;
            _mailService = mailService;
        }

        public async Task<Response<List<CountryDto>>> GetAll()
        {
            try
            {
                var countries = await _unitOfWork.Countries.GetAll();

                if (!countries.Any())
                    return Response<List<CountryDto>>.NoContent("No countries are exist");

                List<CountryDto> result = countries.Select(country=> new CountryDto
                {
                    Id = country.Id,
                    Name = country.Name,
                }).ToList();

                return Response<List<CountryDto>>.Success(result, "Countries retrieved successfully").WithCount();
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "CountryService",
                    MethodName = "GetAll",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<List<CountryDto>>.ServerError("Error occured while retrieving countries",
                    "An unexpected error occurred while retrieving countries. Please try again later.");
            }
        }
    }
}
