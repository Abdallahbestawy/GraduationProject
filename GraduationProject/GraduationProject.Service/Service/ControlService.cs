using GraduationProject.Data.Entity;
using GraduationProject.Data.Enum;
using GraduationProject.Identity.Enum;
using GraduationProject.Identity.IService;
using GraduationProject.Mails.IService;
using GraduationProject.Mails.Models;
using GraduationProject.Repository.Repository;
using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.AcademyYearDto;
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
                if (std.Item1 == null && std.Item2 == null && !std.Item3)
                    return Response<bool>.BadRequest("This semseter doesn't have students");
                if (!std.Item3 && std.Item1 != null)
                {
                    await _unitOfWork.StudentSemesters.AddRangeAsync(std.Item1);
                    int result = await _unitOfWork.SaveAsync();
                    if (result > 0)
                        return Response<bool>.Success(true, "The semeseter ended successfully");
                }
                else
                {
                    if (std.Item1 == null && std.Item2 == null && std.Item3)
                        return Response<bool>.BadRequest("This semseter doesn't have students");
                    if (std.Item2 != null)
                    {
                        await _unitOfWork.StudentSemesters.UpdateRangeAsync(std.Item2);
                        if (std.Item1 != null)
                        {
                            await _unitOfWork.StudentSemesters.AddRangeAsync(std.Item1);
                        }
                        int result = await _unitOfWork.SaveAsync();
                        if (result > 0)
                            return Response<bool>.Success(true, "The semeseter ended successfully");
                    }
                }

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
                    ClassName = "ControlService",
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
                    ClassName = "ControlService",
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
                    ClassName = "ControlService",
                    MethodName = "AddControlMembersAsync",
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
                    ClassName = "ControlService",
                    MethodName = "AddControlMembersAsync",
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
                    ClassName = "ControlService",
                    MethodName = "GetAllControlMembersAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<List<GetAllStaffsDto>>.ServerError("Error occured while retrieving ControlMembers",
                     "An unexpected error occurred while retrieving ControlMembers. Please try again later.");
            }
        }

        public async Task<Response<List<GetAllSemesterActiveDto>>> GetAllSemesterActiveAsync(int academyYearId)
        {
            try
            {
                var distinctSemesters = await _unitOfWork.StudentSemesters.GetAllSemesterActiveAsync(academyYearId);
                if (distinctSemesters == null)
                    return Response<List<GetAllSemesterActiveDto>>.NoContent("No active semesters are exist");

                var semesters = distinctSemesters.Select(se => new GetAllSemesterActiveDto
                {
                    SemesterId = se.ScientificDegreeId,
                    SemesterName = $"{se.ScientificDegree.Parent.Name} - {se.ScientificDegree.Name} {se.AcademyYear.Start.Year}/{se.AcademyYear.End.Year}"
                }).ToList();

                return Response<List<GetAllSemesterActiveDto>>.Success(semesters, "Active semesters are retrieved successfully").WithCount();
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "ControlService",
                    MethodName = "GetAllSemesterActiveAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<List<GetAllSemesterActiveDto>>.ServerError("Error occured while retrieving active semesters",
                     "An unexpected error occurred while retrieving active semesters. Please try again later.");
            }
        }

        public async Task<Response<GetStudentsSemesterResultDto>> GetStudentsSemesterResultAsync(int semesterId, int acedemyYearId)
        {
            try
            {
                SqlParameter pSemesterId = new SqlParameter("@ScientificDegreeId", semesterId);
                SqlParameter pAcedemyYearId = new SqlParameter("@AcademyYearId", acedemyYearId);
                var getStudentInSemesters = await _unitOfWork.GetStudentsSemesterResultModels.CallStoredProcedureAsync(
                        "EXECUTE SpGetStudentsSemesterResult", pAcedemyYearId, pSemesterId);

                if (!getStudentInSemesters.Any())
                    return Response<GetStudentsSemesterResultDto>.NoContent("No results are exist");

                GetStudentsSemesterResultDto getStudentsSemesterResultDtos = new GetStudentsSemesterResultDto
                {
                    SemesterName = $"{getStudentInSemesters.FirstOrDefault().SemesterName} - {getStudentInSemesters.FirstOrDefault().BandName}",
                    AcademyYearName = getStudentInSemesters.FirstOrDefault().AcademyYear,
                    studentsDetiels = getStudentInSemesters.DistinctBy(student => student.StudentName).Select(student =>
                         new StudentsDetielsDto
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
                         }).ToList()
                };

                return Response<GetStudentsSemesterResultDto>.Success(getStudentsSemesterResultDtos, "Students semester result are retrieved successfully")
                    .WithCount(getStudentsSemesterResultDtos.studentsDetiels.Count);
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "ControlService",
                    MethodName = "GetStudentsSemesterResultAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<GetStudentsSemesterResultDto>.ServerError("Error occured while retrieving students semester result",
                     "An unexpected error occurred while retrieving students semester result. Please try again later.");
            }
        }

        public async Task<Response<GetStudentInSemesterResultDto>> GetStudentInSemesterResulAsync(int studentSemesterId)
        {
            try
            {
                SqlParameter pStudentSemesterId = new SqlParameter("@StudentSemesterId", studentSemesterId);
                var getStudentInSemesters = await _unitOfWork.GetStudentInSemesterResultModels.CallStoredProcedureAsync(
                        "EXECUTE SpGetStudentInSemesterResult", pStudentSemesterId);

                if (getStudentInSemesters == null || !getStudentInSemesters.Any())
                    return Response<GetStudentInSemesterResultDto>.NoContent("No results are exist");

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

                return Response<GetStudentInSemesterResultDto>.Success(getStudentsSemesterResultDto, "Student's results are retrieved successfully")
                    .WithCount();
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "ControlService",
                    MethodName = "GetStudentInSemesterResulAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<GetStudentInSemesterResultDto>.ServerError("Error occured while retrieving student's result",
                     "An unexpected error occurred while retrieving student's result. Please try again later.");
            }
        }

        public async Task<Response<GetAllStudentInCourseResultDto>> GetAllStudentInCourseResultAsync(int semesterId, int acedemyYearId, int courseId)
        {
            try
            {
                SqlParameter pSemesterId = new SqlParameter("@ScientificDegreeId", semesterId);
                SqlParameter pAcedemyYearId = new SqlParameter("@AcademyYearId", acedemyYearId);
                SqlParameter pCourseId = new SqlParameter("@CourseId", courseId);
                var getStudentInCourse = await _unitOfWork.GetAllStudentInCourseResultModels.CallStoredProcedureAsync(
                        "EXECUTE SpGetAllStudentInCourseResult", pAcedemyYearId, pSemesterId, pCourseId);

                if (getStudentInCourse == null || !getStudentInCourse.Any())
                    return Response<GetAllStudentInCourseResultDto>.NoContent();

                var getAllStudentInCourseResultDto = new GetAllStudentInCourseResultDto
                {
                    CourseCode = getStudentInCourse.FirstOrDefault().CourseCode,
                    CourseName = getStudentInCourse.FirstOrDefault().CourseName,
                    NumberOfPoints = getStudentInCourse.FirstOrDefault().NumberOfPoints,
                    CourseStudentCourseDetiles = getStudentInCourse.DistinctBy(s => s.StudentName).Select(s => new CourseStudentCourseDetilesDto
                    {
                        StudentName = s.StudentName,
                        StudentCode = s.StudentCode,
                        CourseStatus = s.CourseStatus,
                        CourseChar = s.CourseChar,
                        CourseDegree = s.CourseDegree,
                        CourseDegreeDetiles = getStudentInCourse.Where(course => course.CourseName == s.CourseName && course.StudentName == s.StudentName).Select(course => new CourseDegreeDetielsDto
                        {
                            AssessMethodsName = course.AssessMethodsName,
                            Degree = course.Degree
                        }).ToList()
                    }).ToList()
                };
                return Response<GetAllStudentInCourseResultDto>.Success(getAllStudentInCourseResultDto, "Students results are retrieved successfully")
                    .WithCount();
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "ControlService",
                    MethodName = "GetAllStudentInCourseResultAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<GetAllStudentInCourseResultDto>.ServerError("Error occured while retrieving students results",
                     "An unexpected error occurred while retrieving students results. Please try again later.");
            }
        }

        public async Task<Response<List<GetAllAcdemyYearGraduatesDto>>> GetAllAcdemyYearGraduatesAsync()
        {
            try
            {
                var distinctSemesters = await _unitOfWork.StudentSemesters.GetAllAcdemyYearGraduatesAsync();
                if (distinctSemesters == null)
                    return Response<List<GetAllAcdemyYearGraduatesDto>>.NoContent("No AcdemyYear Graduates are exist");
                int number = 0;

                var acdemeyGraduates = distinctSemesters
                         .Select((se, index) => new GetAllAcdemyYearGraduatesDto
                         {
                             AcdemyYearId = se.AcademyYearId,
                             NumberGraduate = index + 1,
                             NameGraduate = $"{se.AcademyYear.Start.Year} / {se.AcademyYear.End.Year}"
                         })
                         .ToList();

                return Response<List<GetAllAcdemyYearGraduatesDto>>.Success(acdemeyGraduates, "AcdemyYear Graduates are retrieved successfully").WithCount();
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "ControlService",
                    MethodName = "GetAllAcdemyYearGraduatesAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<List<GetAllAcdemyYearGraduatesDto>>.ServerError("Error occured while retrieving AcdemyYear Graduates",
                     "An unexpected error occurred while retrieving AcdemyYear Graduates. Please try again later.");
            }
        }

        public async Task<Response<GetGraduateStudentsByAcademyYearIdDto>> GetGraduateStudentsByAcademyYearIdAsync(int acedemyYearId)
        {
            try
            {
                SqlParameter pAcedemyYearId = new SqlParameter("@AcademyYearId", acedemyYearId);
                var getGraduateStudent = await _unitOfWork.GetGraduateStudentsByAcademyYearIdModels.CallStoredProcedureAsync(
                        "EXECUTE SpGetGraduateStudentsByAcademyYearId", pAcedemyYearId);
                if (!getGraduateStudent.Any())
                {
                    return Response<GetGraduateStudentsByAcademyYearIdDto>.NoContent("No Graduate Student are exist");
                }
                GetGraduateStudentsByAcademyYearIdDto getGraduateStudentsByAcademyYearIdDto = new GetGraduateStudentsByAcademyYearIdDto
                {
                    AcademyYearName = getGraduateStudent.FirstOrDefault().AcademyYear,
                    GraduateStudentDetiels = getGraduateStudent.Select(sd => new GraduateStudentDetielsDto
                    {
                        StudentName = sd.StudentName,
                        StudentCode = sd.StudentCode,
                        Percentage = sd.PercentageTotal,
                        Char = sd.CharTotal
                    }).ToList()
                };
                return Response<GetGraduateStudentsByAcademyYearIdDto>.Success(getGraduateStudentsByAcademyYearIdDto, "Graduate Students are retrieved successfully")
                        .WithCount();
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "ControlService",
                    MethodName = "GetGraduateStudentsByAcademyYearIdAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<GetGraduateStudentsByAcademyYearIdDto>.ServerError("Error occured while retrieving Graduate Students",
                     "An unexpected error occurred while retrieving Graduate Students. Please try again later.");
            }
        }
    }
}
