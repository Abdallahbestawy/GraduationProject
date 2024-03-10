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
    public class TeacherAssistantService : ITeacherAssistantService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IAccountService _accountService;
        private readonly IMailService _mailService;
        public TeacherAssistantService(UnitOfWork unitOfWork, IAccountService accountService, IMailService mailService)
        {
            _unitOfWork = unitOfWork;
            _accountService = accountService;
            _mailService = mailService;
        }
        public async Task<Response<int>> AddTeacherAssistantAsync(AddStaffDto addTeacherAssistantDto)
        {
            string userId;

            try
            {
                userId = await _accountService.AddTeacherAssistantAccount(addTeacherAssistantDto.NameArabic, addTeacherAssistantDto.NameEnglish,
                        addTeacherAssistantDto.NationalID, addTeacherAssistantDto.Email, addTeacherAssistantDto.Password);
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "TeacherAssistantService",
                    MethodName = "AddTeacherAssistantAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<int>.ServerError("Error occured while adding Teacher Assistant",
                     "An unexpected error occurred while adding Teacher Assistant. Please try again later.");
            }

            if (string.IsNullOrEmpty(userId))
                return Response<int>.ServerError("Error occured while adding Teacher Assistant",
                         "An unexpected error occurred while adding Teacher Assistant. Please try again later.");

            Staff newTeacherAssistant = new Staff
            {
                UserId = userId,
                PlaceOfBirth = addTeacherAssistantDto.PlaceOfBirth,
                Gender = addTeacherAssistantDto.Gender,
                Nationality = addTeacherAssistantDto.Nationality,
                Religion = addTeacherAssistantDto.Religion,
                DateOfBirth = addTeacherAssistantDto.DateOfBirth,
                CountryId = addTeacherAssistantDto.CountryId,
                GovernorateId = addTeacherAssistantDto.GovernorateId,
                CityId = addTeacherAssistantDto.CityId,
                Street = addTeacherAssistantDto.Street,
                PostalCode = addTeacherAssistantDto.PostalCode
            };

            try
            {
                await _unitOfWork.Staffs.AddAsync(newTeacherAssistant);
                await _unitOfWork.SaveAsync();
            }
            catch(Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "TeacherAssistantService",
                    MethodName = "AddTeacherAssistantAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                await _accountService.DeleteUser(userId);
                return Response<int>.ServerError("Error occured while adding Teacher Assistant",
                        "An unexpected error occurred while adding Teacher Assistant. Please try again later.");
            }

            int teacherAssistantId = newTeacherAssistant.Id;
            QualificationData newQualificationDataStudent = new QualificationData
            {
                StaffId = teacherAssistantId,
                PreQualification = addTeacherAssistantDto.PreQualification,
                SeatNumber = addTeacherAssistantDto.SeatNumber,
                QualificationYear = addTeacherAssistantDto.QualificationYear,
                Degree = addTeacherAssistantDto.Degree
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
                    ClassName = "TeacherAssistantService",
                    MethodName = "AddTeacherAssistantAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                await _unitOfWork.Staffs.Delete(newTeacherAssistant);
                await _accountService.DeleteUser(userId);
                return Response<int>.ServerError("Error occured while adding Teacher Assistant",
                        "An unexpected error occurred while adding Teacher Assistant. Please try again later.");
            }
            return Response<int>.Created("Teacher Assistant added successfully");
            
        }

        public async Task<Response<List<GetAllStaffsDto>>> GetAllTeacherAssistantsAsync()
        {
            try
            {
                var userType = UserType.TeacherAssistant;
                SqlParameter pUserType = new SqlParameter("@UserType", userType);
                var teacherAssistants = await _unitOfWork.GetAllModels.CallStoredProcedureAsync("EXECUTE SpGetAllStaffs", pUserType);
                if (!teacherAssistants.Any())
                    Response<List<GetAllStaffsDto>>.NoContent("No staffs are exist");

                List<GetAllStaffsDto> result = teacherAssistants.Select(teacherAssistant => new GetAllStaffsDto
                {
                    StaffId = teacherAssistant.Id,
                    UserId = teacherAssistant.UserId,
                    Nationality = Enum.GetName(typeof(Nationality), teacherAssistant.Nationality),
                    StaffNameArbic = teacherAssistant.NameArabic,
                    StaffNameEnglish = teacherAssistant.NameEnglish,
                    Gender = Enum.GetName(typeof(Gender), teacherAssistant.Gender),
                    Religion = Enum.GetName(typeof(Religion), teacherAssistant.Religion),
                    Email = teacherAssistant.Email
                }).ToList();

                return Response<List<GetAllStaffsDto>>.Success(result, "Staffs retrieved successfully").WithCount();
            }
            catch(Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "TeacherAssistantService",
                    MethodName = "GetAllTeacherAssistantsAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<List<GetAllStaffsDto>>.ServerError("Error occured while retrieving staffs",
                        "An unexpected error occurred while retrieving staffs. Please try again later.");
            }
        }
    }
}
