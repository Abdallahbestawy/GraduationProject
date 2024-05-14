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

    public class TeacherService : ITeacherService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IAccountService _accountService;
        private readonly IMailService _mailService;

        public TeacherService(UnitOfWork unitOfWork, IAccountService accountService, IMailService mailService)
        {
            _unitOfWork = unitOfWork;
            _accountService = accountService;
            _mailService = mailService;
        }

        public async Task<Response<int>> AddTeacheAsync(AddStaffDto addSaffDto)
        {
            string userId = "";

            try
            {
                userId = await _accountService.AddTeacherAccount(addSaffDto.NameArabic, addSaffDto.NameEnglish,
                                  addSaffDto.NationalID, addSaffDto.Email, addSaffDto.Password);
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "TeacherService",
                    MethodName = "AddTeacheAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                await _accountService.DeleteUser(userId);
                return Response<int>.ServerError("Error occured while adding Teacher",
                         "An unexpected error occurred while adding Teacher. Please try again later.");
            }

            if (string.IsNullOrEmpty(userId))
                return Response<int>.ServerError("Error occured while adding Teacher",
                         "An unexpected error occurred while adding Teacher. Please try again later.");

            Staff newTeacher = new Staff
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
                FacultyId = addSaffDto.FacultyId,
                PostalCode = addSaffDto.PostalCode
            };

            try
            {
                await _unitOfWork.Staffs.AddAsync(newTeacher);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "TeacherService",
                    MethodName = "AddTeacheAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                await _accountService.DeleteUser(userId);
                return Response<int>.ServerError("Error occured while adding Teacher",
                         "An unexpected error occurred while adding Teacher. Please try again later.");
            }

            int teacherId = newTeacher.Id;
            QualificationData newQualificationDataStudent = new QualificationData
            {
                StaffId = teacherId,
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
                    ClassName = "TeacherService",
                    MethodName = "AddTeacheAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                await _unitOfWork.Staffs.Delete(newTeacher);
                await _accountService.DeleteUser(userId);
                return Response<int>.ServerError("Error occured while adding Teacher",
                         "An unexpected error occurred while adding Teacher. Please try again later.");
            }
            try
            {
                if (addSaffDto.PhoneNumbers != null)
                {
                    List<Phone> phones = addSaffDto.PhoneNumbers.Select(ph =>
                        new Phone
                        {
                            StaffId = teacherId,
                            PhoneNumber = ph.PhoneNumber,
                            Type = ph.Type,
                        }).ToList();

                    await _unitOfWork.Phones.AddRangeAsync(phones);
                    await _unitOfWork.SaveAsync();
                }
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "TeacherService",
                    MethodName = "AddTeacheAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                await _unitOfWork.QualificationDatas.Delete(newQualificationDataStudent);
                await _unitOfWork.Staffs.Delete(newTeacher);
                await _accountService.DeleteUser(userId);
                return Response<int>.ServerError("Error occured while adding Teacher",
                     "An unexpected error occurred while adding Teacher. Please try again later.");
            }
            return Response<int>.Created("Teacher added successfully");

        }

        public async Task<Response<List<GetAllStaffsDto>>> GetAllTeachersAsync(int FacultyId)
        {
            try
            {
                var userType = UserType.Teacher;
                SqlParameter pUserType = new SqlParameter("@UserType", userType);
                SqlParameter pFacultyId = new SqlParameter("@FacultyId", FacultyId);
                var teachers = await _unitOfWork.GetAllModels.CallStoredProcedureAsync("EXECUTE SpGetAllStaffs", pUserType, pFacultyId);

                if (!teachers.Any())
                    return Response<List<GetAllStaffsDto>>.NoContent("No Teachers are exist");

                List<GetAllStaffsDto> result = teachers.Select(teacher => new GetAllStaffsDto
                {
                    StaffId = teacher.Id,
                    UserId = teacher.UserId,
                    Nationality = Enum.GetName(typeof(Nationality), teacher.Nationality),
                    StaffNameArbic = teacher.NameArabic,
                    StaffNameEnglish = teacher.NameEnglish,
                    Gender = Enum.GetName(typeof(Gender), teacher.Gender),
                    Religion = Enum.GetName(typeof(Religion), teacher.Religion),
                    Email = teacher.Email
                }).ToList();

                return Response<List<GetAllStaffsDto>>.Success(result, "Teachers retrieved successfully").WithCount();
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "TeacherService",
                    MethodName = "GetAllTeachersAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<List<GetAllStaffsDto>>.ServerError("Error occured while adding Teacher",
                         "An unexpected error occurred while adding Teacher. Please try again later.");
            }
        }
    }
}
