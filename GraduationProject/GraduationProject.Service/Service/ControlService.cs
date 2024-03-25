using GraduationProject.Data.Entity;
using GraduationProject.Data.Enum;
using GraduationProject.Identity.Enum;
using GraduationProject.Identity.IService;
using GraduationProject.Mails.IService;
using GraduationProject.Mails.Models;
using GraduationProject.Repository.Repository;
using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.SemesterDto;
using GraduationProject.Service.DataTransferObject.StaffDto;
using GraduationProject.Service.IService;
using Microsoft.Data.SqlClient;

namespace GraduationProject.Service.Service
{
    public class ControlService : IControlService
    {

        private readonly UnitOfWork _unitOfWork;
        private readonly IMailService _mailService;
        private readonly IAccountService _accountService;


        public ControlService(UnitOfWork unitOfWork, IMailService mailService, IAccountService accountService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mailService = mailService;
            _accountService = accountService;

        }

        public async Task<Response<bool>> RaisingGradesSemesterAsync(int semesterId)
        {
            try
            {
                bool result = await _unitOfWork.StudentSemesters.RaisingGradesSemesterAsync(semesterId);

                if (!result)
                    return Response<bool>.ServerError("Error occured while raising semester's grades",
                     "An unexpected error occurred while raising semester's grades. Please try again later.");

                await _unitOfWork.SaveAsync();
                return Response<bool>.Success(result, "Raising semester's grades success");
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "ControlService",
                    MethodName = "RaisingGradesSemesterAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<bool>.ServerError("Error occured while raising semester's grades",
                     "An unexpected error occurred while raising semester's grades. Please try again later.");
            }
        }

        public async Task<Response<bool>> RaisingGradesCourseAsync(int courseId)
        {
            try
            {
                bool result = await _unitOfWork.StudentSemesters.RaisingGradesCourseAsync(courseId);

                if (!result)
                    return Response<bool>.ServerError("Error occured while raising course's grades",
                     "An unexpected error occurred while raising course's grades. Please try again later.");

                await _unitOfWork.SaveAsync();
                return Response<bool>.Success(result, "Raising course's grades success");
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "ControlService",
                    MethodName = "RaisingGradesCourseAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<bool>.ServerError("Error occured while raising course's grades",
                     "An unexpected error occurred while raising course's grades. Please try again later.");
            }

        }

        public async Task<Response<List<GetAllSemesterCurrentDto>>> GetAllSemesterCurrentAsync()
        {
            try
            {
                var semesters = await _unitOfWork.StudentSemesters.GetAllSemesterCurrentAsync();

                if (semesters == null)
                    return Response<List<GetAllSemesterCurrentDto>>.NoContent("No semesters are exist");

                var getAllSemesterCurrentDtos = semesters.Select(semester => new GetAllSemesterCurrentDto
                {
                    Id = semester.ScientificDegreeId,
                    Name = semester.ScientificDegree.Name
                }).ToList();

                return Response<List<GetAllSemesterCurrentDto>>.Success(getAllSemesterCurrentDtos, "Semesters are retrieved successfully").WithCount();
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "ControlService",
                    MethodName = "GetAllSemesterCurrentAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<List<GetAllSemesterCurrentDto>>.ServerError("Error occured while retrieving semesters",
                     "An unexpected error occurred while retrieving semesters. Please try again later.");
            }
        }
        public async Task<bool> EndSemesterAsync(int semesterId)
        {
            try
            {
                var std = await _unitOfWork.StudentSemesters.EndSemesterAsync(semesterId);
                if (std == null)
                {
                    return false;
                }
                await _unitOfWork.StudentSemesters.AddRangeAsync(std);
                int result = await _unitOfWork.SaveAsync();
                if (result > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<Response<int>> AddControlMembersAsync(AddStaffDto addControlMembersDto)
        {
            string userId = "";

            try
            {
                userId = await _accountService.AddControlMembers(addControlMembersDto.NameArabic, addControlMembersDto.NameEnglish,
                       addControlMembersDto.NationalID, addControlMembersDto.Email, addControlMembersDto.Password);
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

            Staff newaddControlMembersDto = new Staff
            {
                UserId = userId,
                PlaceOfBirth = addControlMembersDto.PlaceOfBirth,
                Gender = addControlMembersDto.Gender,
                Nationality = addControlMembersDto.Nationality,
                Religion = addControlMembersDto.Religion,
                DateOfBirth = addControlMembersDto.DateOfBirth,
                CountryId = addControlMembersDto.CountryId,
                GovernorateId = addControlMembersDto.GovernorateId,
                CityId = addControlMembersDto.CityId,
                Street = addControlMembersDto.Street,
                PostalCode = addControlMembersDto.PostalCode
            };

            try
            {
                await _unitOfWork.Staffs.AddAsync(newaddControlMembersDto);
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

            int AdministrationId = newaddControlMembersDto.Id;
            QualificationData newQualificationDataStudent = new QualificationData
            {
                StaffId = AdministrationId,
                PreQualification = addControlMembersDto.PreQualification,
                SeatNumber = addControlMembersDto.SeatNumber,
                QualificationYear = addControlMembersDto.QualificationYear,
                Degree = addControlMembersDto.Degree
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
                await _unitOfWork.Staffs.Delete(newaddControlMembersDto);
                await _accountService.DeleteUser(userId);
                return Response<int>.ServerError("Error occured while adding AddAdministration",
                     "An unexpected error occurred while adding AddAdministration. Please try again later.");
            }

            return Response<int>.Created("AddAdministration added successfully");
        }

        public async Task<Response<List<GetAllStaffsDto>>> GetAllControlMembersAsync()
        {
            try
            {
                var userType = UserType.ControlMembers;
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
        //public async Task Test()
        //{
        //    var students = await _unitOfWork.StudentSemesters.GetTheCurrentSemesterWithStudents();

        //    var mappedResult = students.Select(group => new SemesterStudentsDTO
        //    {
        //        ScientificDegreeId = (int)group.GetType().GetProperty("ScientificDegreeId").GetValue(group, null),
        //        Students = (List<StudentSemester>)group.GetType().GetProperty("Students").GetValue(group, null)
        //    }).ToList();

        //}
    }
}
