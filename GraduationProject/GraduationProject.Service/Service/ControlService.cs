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

        public async Task<Response<GetAllSemesterCurrentDto>> GetAllSemesterCurrentAsync()
        {
            try
            {
                var semesters = await _unitOfWork.StudentSemesters.GetAllSemesterCurrentAsync();

                if (semesters == null)
                    return Response<GetAllSemesterCurrentDto>.NoContent("No semesters are exist");
                var startDate = semesters.FirstOrDefault().AcademyYear.Start;
                var endDate = semesters.FirstOrDefault().AcademyYear.End;
                GetAllSemesterCurrentDto getAllSemesterCurrentDtos = new GetAllSemesterCurrentDto
                {
                    AcademyYearName = $"{startDate.Day}/{startDate.Month}/{startDate.Year} - {endDate.Day}/{endDate.Month}/{endDate.Year}"
                };

                getAllSemesterCurrentDtos.semesterName = semesters.Select(semester => new GetSemesterNameDto
                {
                    Id = semester.ScientificDegreeId,
                    Name = $"{semester.ScientificDegree.Parent.Name} - {semester.ScientificDegree.Name}"
                }).ToList();


                return Response<GetAllSemesterCurrentDto>.Success(getAllSemesterCurrentDtos, "Semesters are retrieved successfully").WithCount();
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
                return Response<GetAllSemesterCurrentDto>.ServerError("Error occured while retrieving semesters",
                     "An unexpected error occurred while retrieving semesters. Please try again later.");
            }
        }

        // the message value in the badRequest need to be specified
        public async Task<Response<bool>> EndSemesterAsync(int semesterId)
        {
            try
            {
                var std = await _unitOfWork.StudentSemesters.EndSemesterAsync(semesterId);
                if (std == null)
                    return Response<bool>.BadRequest("This semseter doesn't have students");
                await _unitOfWork.StudentSemesters.AddRangeAsync(std);
                int result = await _unitOfWork.SaveAsync();
                if (result > 0)
                    return Response<bool>.Success(true, "The semeseter ended successfully");

                return Response<bool>.ServerError("Error occured while ending semester",
                     "An unexpected error occurred while ending semester. Please try again later.");
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "ControlService",
                    MethodName = "EndSemesterAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<bool>.ServerError("Error occured while ending semester",
                     "An unexpected error occurred while ending semester. Please try again later.");
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
                    ClassName = "ControlMembersService",
                    MethodName = "AddControlMembersAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<int>.ServerError("Error occured while adding ControlMembers",
                     "An unexpected error occurred while adding ControlMembers. Please try again later.");
            }

            if (string.IsNullOrEmpty(userId))
                return Response<int>.ServerError("Error occured while adding ControlMembers",
                     "An unexpected error occurred while adding ControlMembers. Please try again later.");

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
                    ClassName = "ControlMembersService",
                    MethodName = "AddControlMembersAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                await _accountService.DeleteUser(userId);
                return Response<int>.ServerError("Error occured while adding ControlMembers",
                     "An unexpected error occurred while adding ControlMembers. Please try again later.");
            }

            int ControlMemberId = newaddControlMembersDto.Id;
            QualificationData newQualificationDataStudent = new QualificationData
            {
                StaffId = ControlMemberId,
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
                    ClassName = "ControlMembersService",
                    MethodName = "ControlMembersAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                await _unitOfWork.Staffs.Delete(newaddControlMembersDto);
                await _accountService.DeleteUser(userId);
                return Response<int>.ServerError("Error occured while adding ControlMembers",
                     "An unexpected error occurred while adding ControlMembers. Please try again later.");
            }
            try
            {
                if (addControlMembersDto.PhoneNumbers != null)
                {
                    List<Phone> phones = addControlMembersDto.PhoneNumbers.Select(ph =>
                        new Phone
                        {
                            StaffId = ControlMemberId,
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
                    ClassName = "ControlMembersService",
                    MethodName = "ControlMembersAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                await _unitOfWork.QualificationDatas.Delete(newQualificationDataStudent);
                await _unitOfWork.Staffs.Delete(newaddControlMembersDto);
                await _accountService.DeleteUser(userId);
                return Response<int>.ServerError("Error occured while adding ControlMembers",
                     "An unexpected error occurred while adding ControlMembers. Please try again later.");
            }

            return Response<int>.Created("ControlMembers added successfully");
        }

        public async Task<Response<List<GetAllStaffsDto>>> GetAllControlMembersAsync()
        {
            try
            {
                var userType = UserType.ControlMembers;
                SqlParameter pUserType = new SqlParameter("@UserType", userType);
                var controlMembers = await _unitOfWork.GetAllModels.CallStoredProcedureAsync("EXECUTE SpGetAllStaffs", pUserType);

                if (!controlMembers.Any())
                    return Response<List<GetAllStaffsDto>>.NoContent("No ControlMembers are exist");

                List<GetAllStaffsDto> result = controlMembers.Select(controlMember => new GetAllStaffsDto
                {
                    StaffId = controlMember.Id,
                    UserId = controlMember.UserId,
                    Nationality = Enum.GetName(typeof(Nationality), controlMember.Nationality),
                    StaffNameArbic = controlMember.NameArabic,
                    StaffNameEnglish = controlMember.NameEnglish,
                    Gender = Enum.GetName(typeof(Gender), controlMember.Gender),
                    Religion = Enum.GetName(typeof(Religion), controlMember.Religion),
                    Email = controlMember.Email
                }).ToList();

                return Response<List<GetAllStaffsDto>>.Success(result, "ControlMembers retrieved successfully").WithCount();
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "ControlMembersService",
                    MethodName = "GetAllControlMembersAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<List<GetAllStaffsDto>>.ServerError("Error occured while retrieving ControlMembers",
                     "An unexpected error occurred while retrieving ControlMembers. Please try again later.");
            }
        }

        public async Task<List<GetAllSemesterActiveDto>> GetAllSemesterActiveAsync(int academyYearId)
        {
            var distinctSemesters = await _unitOfWork.StudentSemesters.GetAllSemesterActiveAsync(academyYearId);
            if (distinctSemesters == null)
            {
                return null;
            }
            var semesters = distinctSemesters.Select(se => new GetAllSemesterActiveDto
            {
                SemesterId = se.ScientificDegreeId,
                SemesterName = $"{se.ScientificDegree.Parent.Name} - {se.ScientificDegree.Name} {se.AcademyYear.Start.Year}/{se.AcademyYear.End.Year}"
            }).ToList();
            return semesters;
        }

        public async Task<List<GetStudentsSemesterResultDto>> GetStudentsSemesterResultAsync(int semesterId, int acedemyYearId)
        {
            SqlParameter pSemesterId = new SqlParameter("@ScientificDegreeId", semesterId);
            SqlParameter pAcedemyYearId = new SqlParameter("@AcademyYearId", acedemyYearId);
            var getStudentInSemesters = await _unitOfWork.GetStudentsSemesterResultModels.CallStoredProcedureAsync(
                    "EXECUTE SpGetStudentsSemesterResult", pAcedemyYearId, pSemesterId);
            if (getStudentInSemesters == null || !getStudentInSemesters.Any())
            {
                return null;
            }
            List<GetStudentsSemesterResultDto> getStudentsSemesterResultDtos = new List<GetStudentsSemesterResultDto>();
            getStudentsSemesterResultDtos = getStudentInSemesters.DistinctBy(student => student.StudentName).Select(student =>
                new GetStudentsSemesterResultDto
                {
                    StudentCode = student.StudentCode,
                    StudentName = student.StudentName,
                    StudentSemesterPercentage = student.StudentSemesterPercentage,
                    StudentSemesterChar = student.StudentSemesterChar,
                    StudentCumulativePercentage = student.StudentCumulativePercentage,
                    StudentCumulativeChar = student.StudentCumulativeChar,
                    StudentSemesterStatus = student.SemesterStatus,
                    StudentCourseDetiles = getStudentInSemesters.Where(course => course.StudentName == student.StudentName).DistinctBy(course => course.CourseName).Select(course => new StudentCourseDetilesDto
                    {
                        CourseCode = course.CourseCode,
                        CourseName = course.CourseName,
                        CourseDegree = course.CourseDegree,
                        CourseChar = course.CourseChar,
                        CourseStatus = course.CourseStatus,
                        NumberOfPoints = course.NumberOfPoints,
                        CourseDegreeDetiles = getStudentInSemesters.Where(detiles => detiles.CourseName == course.CourseName && detiles.StudentName == student.StudentName).Select(detiles => new CourseDegreeDetielsDto
                        {
                            AssessMethodsName = detiles.Name,
                            Degree = detiles.Degree,
                        }).ToList()
                    }).ToList()
                }).ToList();
            return getStudentsSemesterResultDtos;
        }

        public async Task<GetStudentInSemesterResultDto> GetStudentInSemesterResulAsync(int studentSemesterId)
        {
            SqlParameter pStudentSemesterId = new SqlParameter("@StudentSemesterId", studentSemesterId);
            var getStudentInSemesters = await _unitOfWork.GetStudentInSemesterResultModels.CallStoredProcedureAsync(
                    "EXECUTE SpGetStudentInSemesterResult", pStudentSemesterId);
            if (getStudentInSemesters == null || !getStudentInSemesters.Any())
            {
                return null;
            }
            GetStudentInSemesterResultDto getStudentsSemesterResultDto = new GetStudentInSemesterResultDto
            {
                StudentName = getStudentInSemesters.FirstOrDefault().StudentName,
                StudentCode = getStudentInSemesters.FirstOrDefault().StudentCode,
                StudentSemesterPercentage = getStudentInSemesters.FirstOrDefault().StudentSemesterPercentage,
                StudentSemesterChar = getStudentInSemesters.FirstOrDefault().StudentCumulativeChar,
                StudentCumulativePercentage = getStudentInSemesters.FirstOrDefault().StudentCumulativePercentage,
                StudentCumulativeChar = getStudentInSemesters.FirstOrDefault().StudentCumulativeChar,
                StudentSemesterStatus = getStudentInSemesters.FirstOrDefault().SemesterStatus,
                StudentCourseDetiles = getStudentInSemesters.DistinctBy(course => course.CourseName).Select(course => new StudentCourseDetilesDto
                {
                    CourseName = course.CourseName,
                    CourseCode = course.CourseCode,
                    CourseChar = course.CourseChar,
                    CourseDegree = course.CourseDegree,
                    CourseStatus = course.CourseStatus,
                    NumberOfPoints = course.NumberOfPoints,
                    CourseDegreeDetiles = getStudentInSemesters.Where(detiels => detiels.CourseName == course.CourseName).Select(detiels => new CourseDegreeDetielsDto
                    {
                        AssessMethodsName = detiels.AssessMethodsName,
                        Degree = detiels.Degree
                    }).ToList(),
                }).ToList()

            };

            return getStudentsSemesterResultDto;
        }
    }
}
