using GraduationProject.Data.Entity;
using GraduationProject.Data.Enum;
using GraduationProject.Identity.Enum;
using GraduationProject.Identity.IService;
using GraduationProject.Mails.IService;
using GraduationProject.Mails.Models;
using GraduationProject.Repository.Repository;
using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.StaffDto;
using GraduationProject.Service.IService;
using Microsoft.Data.SqlClient;

namespace GraduationProject.Service.Service
{
    public class AdministrationService : IAdministrationService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IAccountService _accountService;
        private readonly IMailService _mailService;

        public AdministrationService(UnitOfWork unitOfWork, IAccountService accountService, IMailService mailService)
        {
            _unitOfWork = unitOfWork;
            _accountService = accountService;
            _mailService = mailService;
        }

        public async Task<Response<int>> AddAdministrationAsync(AddStaffDto addSaffDto)
        {
            string userId = "";

            try
            {
                userId = await _accountService.AddAdministrationAccount(addSaffDto.NameArabic, addSaffDto.NameEnglish,
                       addSaffDto.NationalID, addSaffDto.Email, addSaffDto.Password);
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "AdministrationService",
                    MethodName = "AddAdministrationAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<int>.ServerError("Error occured while adding AddAdministration",
                     "An unexpected error occurred while adding AddAdministration. Please try again later.");
            }

            if (!string.IsNullOrEmpty(userId))
                return Response<int>.ServerError("Error occured while adding AddAdministration",
                     "An unexpected error occurred while adding AddAdministration. Please try again later.");

            Staff newAdministration = new Staff
            {
                UserId = userId,
                PlaceOfBirth = addSaffDto.PlaceOfBirth,
                Gender = addSaffDto.Gender,
                Nationality = addSaffDto.Nationality,
                Religion = addSaffDto.Religion,
                DateOfBirth = addSaffDto.DateOfBirth,
                CountryId = addSaffDto.CountryId,
                GovernorateId = addSaffDto.GovernorateId,
                CityId = addSaffDto.CityId,
                Street = addSaffDto.Street,
                PostalCode = addSaffDto.PostalCode
            };

            try
            {
                await _unitOfWork.Staffs.AddAsync(newAdministration);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "AdministrationService",
                    MethodName = "AddAdministrationAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                await _accountService.DeleteUser(userId);
                return Response<int>.ServerError("Error occured while adding AddAdministration",
                     "An unexpected error occurred while adding AddAdministration. Please try again later.");
            }

            int AdministrationId = newAdministration.Id;
            QualificationData newQualificationDataStudent = new QualificationData
            {
                StaffId = AdministrationId,
                PreQualification = addSaffDto.PreQualification,
                SeatNumber = addSaffDto.SeatNumber,
                QualificationYear = addSaffDto.QualificationYear,
                Degree = addSaffDto.Degree
            };

            try
            {
                await _unitOfWork.QualificationDatas.AddAsync(newQualificationDataStudent);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "AdministrationService",
                    MethodName = "AddAdministrationAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                await _unitOfWork.Staffs.Delete(newAdministration);
                await _accountService.DeleteUser(userId);
                return Response<int>.ServerError("Error occured while adding AddAdministration",
                     "An unexpected error occurred while adding AddAdministration. Please try again later.");
            }

            return Response<int>.Created("AddAdministration added successfully");
        }

        public async Task<Response<List<GetAllStaffsDto>>> GetAllAdministrationsAsync()
        {
            try
            {
                var userType = UserType.Administration;
                SqlParameter pUserType = new SqlParameter("@UserType", userType);
                var administrations = await _unitOfWork.GetAllModels.CallStoredProcedureAsync("EXECUTE SpGetAllStaffs", pUserType);

                if (!administrations.Any())
                    return Response<List<GetAllStaffsDto>>.NoContent("No AddAdministrations are exist");

                List<GetAllStaffsDto> result = administrations.Select(administration => new GetAllStaffsDto
                {
                    StaffId = administration.Id,
                    UserId = administration.UserId,
                    Nationality = Enum.GetName(typeof(Nationality), administration.Nationality),
                    StaffNameArbic = administration.NameArabic,
                    StaffNameEnglish = administration.NameEnglish,
                    Gender = Enum.GetName(typeof(Gender), administration.Gender),
                    Religion = Enum.GetName(typeof(Religion), administration.Religion),
                    Email = administration.Email
                }).ToList();

                return Response<List<GetAllStaffsDto>>.Success(result, "AddAdministrations retrieved successfully").WithCount();
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "AdministrationService",
                    MethodName = "GetAllAdministrationsAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<List<GetAllStaffsDto>>.ServerError("Error occured while retrieving AddAdministrations",
                     "An unexpected error occurred while retrieving AddAdministrations. Please try again later.");
            }
        }
    }
}
