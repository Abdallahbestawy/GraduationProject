using GraduationProject.Mails.IService;
using GraduationProject.Mails.Models;
using GraduationProject.Repository.IRepository;
using GraduationProject.Repository.Repository;
using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.CityDto;
using GraduationProject.Service.DataTransferObject.GovernorateDto;
using GraduationProject.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Service.Service
{
    public class GovernorateService : IGovernorateService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMailService _mailService;

        public GovernorateService(UnitOfWork unitOfWork, IMailService mailService)
        {
            _unitOfWork = unitOfWork;
            _mailService = mailService;
        }

        public async Task<Response<List<GovernorateDto>>> GetByCountyId(int countyId)
        {
            try
            {
                if (await _unitOfWork.Countries.GetByIdAsync(countyId) == null)
                    return Response<List<GovernorateDto>>.BadRequest("This country doesn't exist");

                var governorates = await _unitOfWork.Governorates.GetEntityByPropertyAsync(gov => gov.CountryId == countyId);

                if (!governorates.Any())
                    return Response<List<GovernorateDto>>.NoContent("No governorates are exist");

                List<GovernorateDto> result = governorates.Select(g => new GovernorateDto
                {
                    Id = g.Id,
                    Name = g.Name,
                }).ToList();

                return Response<List<GovernorateDto>>.Success(result, "Governorates retrieved successfully").WithCount();
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "GovernorateService",
                    MethodName = "GetByCountyId",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<List<GovernorateDto>>.ServerError("Error occured while retrieving governorates",
                    "An unexpected error occurred while retrieving governorates. Please try again later.");
            }
        }
    }
}
