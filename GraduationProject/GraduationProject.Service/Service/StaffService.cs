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
    public class StaffService : IStaffService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IAccountService _accountService;
        private readonly ICourseService _courseService;
        private readonly IMailService _mailService;

        public StaffService(UnitOfWork unitOfWork, IAccountService accountService, ICourseService courseService, IMailService mailService)
        {
            _unitOfWork = unitOfWork;
            _accountService = accountService;
            _courseService = courseService;
            _mailService = mailService;
        }

        public async Task<Response<int>> AddStAffAsync(AddStaffDto addSaffDto)
        {
            string userId = "";
            try
            {
                userId = await _accountService.AddStaffAccount(addSaffDto.NameArabic, addSaffDto.NameEnglish,
                    addSaffDto.NationalID, addSaffDto.Email, addSaffDto.Password);
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "StaffService",
                    MethodName = "AddStAffAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<int>.ServerError("Error occured while adding staff",
                         "An unexpected error occurred while adding staff. Please try again later.");
            }

            if (string.IsNullOrEmpty(userId))
                return Response<int>.ServerError("Error occured while adding staff",
                         "An unexpected error occurred while adding staff. Please try again later.");

            Staff newStaff = new Staff
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
                await _unitOfWork.Staffs.AddAsync(newStaff);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "StaffService",
                    MethodName = "AddStAffAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                await _accountService.DeleteUser(userId);
                return Response<int>.ServerError("Error occured while adding staff",
                         "An unexpected error occurred while adding staff. Please try again later.");
            }

            int staffId = newStaff.Id;
            QualificationData newQualificationDataStudent = new QualificationData
            {
                StaffId = staffId,
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
                    ClassName = "StaffService",
                    MethodName = "AddStAffAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                await _unitOfWork.Staffs.Delete(newStaff);
                await _accountService.DeleteUser(userId);
                return Response<int>.ServerError("Error occured while adding staff",
                         "An unexpected error occurred while adding staff. Please try again later.");
            }
            try
            {
                if (addSaffDto.PhoneNumbers != null)
                {
                    List<Phone> phones = addSaffDto.PhoneNumbers.Select(ph =>
                        new Phone
                        {
                            StaffId = staffId,
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
                    ClassName = "StaffService",
                    MethodName = "StaffService",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                await _unitOfWork.QualificationDatas.Delete(newQualificationDataStudent);
                await _unitOfWork.Staffs.Delete(newStaff);
                await _accountService.DeleteUser(userId);
                return Response<int>.ServerError("Error occured while adding Staff",
                     "An unexpected error occurred while adding Staff. Please try again later.");
            }
            return Response<int>.Created("Staff added successfully");

        }

        public async Task<Response<int>> AddStaffSemesterAsync(AddStaffSemesterDto addStaffSemesterDto)
        {
            try
            {
                StaffSemester newStaffSemester = new StaffSemester
                {
                    StaffId = addStaffSemesterDto.StaffId,
                    CourseId = addStaffSemesterDto.CourseId,
                    AcademyYearId = addStaffSemesterDto.AcademyYearId
                };
                await _unitOfWork.StaffSemesters.AddAsync(newStaffSemester);
                var result = await _unitOfWork.SaveAsync();

                if (result > 0)
                    return Response<int>.Created("Staff assigned to course in semester successfully");

                return Response<int>.ServerError("Error occured while assigning to course in semester",
                    "An unexpected error occurred while assigning to course in semester. Please try again later.");
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "StaffService",
                    MethodName = "AddStaffSemesterAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<int>.ServerError("Error occured while assigning to course in semester",
                     "An unexpected error occurred while assigning to course in semester. Please try again later.");
            }
        }

        public async Task<Response<GetCourseStaffSemesterDto>> GetCourseStaffSemesterAsync(int satffId)
        {
            try
            {
                var staffSemesters = await _unitOfWork.StaffSemesters
                   .FindWithIncludeIQueryableAsync(d => d.AcademyYear, c => c.Course);
                if (staffSemesters == null)
                    return Response<GetCourseStaffSemesterDto>.BadRequest("This staff doesn't exist");

                var results = staffSemesters
                    .Where(dc => dc.AcademyYear.IsCurrent && dc.StaffId == satffId)
                    .ToList();

                if (!results.Any())
                    return Response<GetCourseStaffSemesterDto>.NoContent("This staff doesn't have courses");

                var staffSemesterDto = new GetCourseStaffSemesterDto
                {
                    StaffId = results.First().StaffId,
                    AcademyYearId = results.First().AcademyYearId,
                    CourseDoctorDtos = results.Select(result => new CourseDoctorDto
                    {
                        CourseId = result.Course.Id,
                        CourseName = result.Course.Name
                    }).ToList()
                };

                return Response<GetCourseStaffSemesterDto>.Success(staffSemesterDto, "Staff's courses retrieved successfully")
                    .WithCount(staffSemesterDto.CourseDoctorDtos.Count());
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "StaffService",
                    MethodName = "GetCourseStaffSemesterAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<GetCourseStaffSemesterDto>.ServerError("Error occured while retrieving staff's courses",
                     "An unexpected error occurred while retrieving staff's courses. Please try again later.");
            }
        }

        public async Task<Response<GetStaffDetailsByUserIdDto>> GetStaffByUserIdAsync(string userId)
        {
            try
            {
                SqlParameter pUserId = new SqlParameter("@UserId", userId);
                var getStaff = await _unitOfWork.GetStaffDetailsByUserIdModels.CallStoredProcedureAsync(
                    "EXECUTE SpGetStaffDetailsByUserId", pUserId);

                if (getStaff.Any())
                {
                    GetStaffDetailsByUserIdDto getStaffDetailsByUserIdDto = new GetStaffDetailsByUserIdDto
                    {
                        NameArabic = getStaff.FirstOrDefault()?.NameArabic,
                        NameEnglish = getStaff.FirstOrDefault()?.NameEnglish,
                        NationalID = getStaff.FirstOrDefault()?.NationalID,
                        Email = getStaff.FirstOrDefault()?.Email,
                        StaffId = getStaff.FirstOrDefault()?.Id ?? 0,
                        StaffAddress = getStaff.FirstOrDefault()?.StaffAddress,
                        DateOfBirth = getStaff.FirstOrDefault()?.DateOfBirth,
                        Gender = Enum.GetName(typeof(Gender), getStaff.FirstOrDefault()?.Gender),
                        Nationality = Enum.GetName(typeof(Nationality), getStaff.FirstOrDefault()?.Nationality),
                        PlaceOfBirth = getStaff.FirstOrDefault()?.PlaceOfBirth,
                        PostalCode = getStaff.FirstOrDefault()?.PostalCode,
                        ReleasePlace = getStaff.FirstOrDefault()?.ReleasePlace,
                        Religion = Enum.GetName(typeof(Religion), getStaff.FirstOrDefault()?.Religion),
                        PreQualification = getStaff.FirstOrDefault()?.PreQualification,
                        QualificationYear = getStaff.FirstOrDefault()?.QualificationYear,
                        SeatNumber = getStaff.FirstOrDefault()?.SeatNumber ?? 0,
                        Degree = getStaff.FirstOrDefault()?.Degree ?? 0.0m,
                    };
                    if (getStaff.Any(s => !string.IsNullOrEmpty(s.StaffPhoneNumber)))
                    {
                        getStaffDetailsByUserIdDto.GetPhoneStaffDtos = getStaff
                            .Where(s => !string.IsNullOrEmpty(s.StaffPhoneNumber))
                            .Select(s => new GetPhoneSafftDto
                            {
                                StaffPhoneNumber = s.StaffPhoneNumber,
                                PhoneType = Enum.GetName(typeof(PhoneType), s.PhoneType)
                            })
                            .ToList();
                    }
                    return Response<GetStaffDetailsByUserIdDto>.Success(getStaffDetailsByUserIdDto, "Staff data retrieved successfully")
                        .WithCount();
                }
                else
                {
                    return Response<GetStaffDetailsByUserIdDto>.NoContent("This Staff doesn't exist");
                }
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "StaffService",
                    MethodName = "GetStaffByUserIdAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<GetStaffDetailsByUserIdDto>.ServerError("Error occured while retrieving Staff's data",
                    "An unexpected error occurred while retrieving Staff's data. Please try again later.");
            }
        }

        public async Task<Response<List<GetAllStaffsDto>>> GetAllStaffsAsync()
        {
            try
            {
                var userType = UserType.Staff;
                SqlParameter pUserType = new SqlParameter("@UserType", userType);
                var staffs = await _unitOfWork.GetAllModels.CallStoredProcedureAsync("EXECUTE SpGetAllStaffs", pUserType);
                if (!staffs.Any())
                    return Response<List<GetAllStaffsDto>>.NoContent("No staffs are exist");

                List<GetAllStaffsDto> result = staffs.Select(staff => new GetAllStaffsDto
                {
                    StaffId = staff.Id,
                    UserId = staff.UserId,
                    Nationality = Enum.GetName(typeof(Nationality), staff.Nationality),
                    StaffNameArbic = staff.NameArabic,
                    StaffNameEnglish = staff.NameEnglish,
                    Gender = Enum.GetName(typeof(Gender), staff.Gender),
                    Religion = Enum.GetName(typeof(Religion), staff.Religion),
                    Email = staff.Email
                }).ToList();

                return Response<List<GetAllStaffsDto>>.Success(result, "Staffs retrieved successfully").WithCount();
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "StaffService",
                    MethodName = "GetAllStaffsAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<List<GetAllStaffsDto>>.ServerError("Error occured while retrieving Staffs",
                    "An unexpected error occurred while retrieving Staffs. Please try again later.");
            }
        }

        public async Task<bool> DeleteStaffSemesterAsync(int staffSemesterId)
        {
            var oldStaffSemester = await _unitOfWork.StaffSemesters.GetByIdAsync(staffSemesterId);
            if (oldStaffSemester != null)
            {
                await _unitOfWork.StaffSemesters.Delete(oldStaffSemester);
                int result = await _unitOfWork.SaveAsync();
                if (result > 0)
                {
                    return true;
                }
            }
            return false;
        }

        //public async Task<GetCourseStaffSemester> Test(int staffId)
        //{

        //}

    }
}




