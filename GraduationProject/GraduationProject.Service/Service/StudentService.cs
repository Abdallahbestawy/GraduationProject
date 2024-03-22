using GraduationProject.Data.Entity;
using GraduationProject.Data.Enum;
using GraduationProject.Identity.IService;
using GraduationProject.Mails.IService;
using GraduationProject.Mails.Models;
using GraduationProject.Repository.Repository;
using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.CourseDto;
using GraduationProject.Service.DataTransferObject.StudentDto;
using GraduationProject.Service.IService;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GraduationProject.Service.Service
{
    public class StudentService : IStudentService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IAccountService _accountService;
        private readonly ICourseService _courseService;
        private readonly IMailService _mailService;

        public StudentService(UnitOfWork unitOfWork, IAccountService accountService, ICourseService courseService, IMailService mailService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
            _courseService = courseService ?? throw new ArgumentNullException(nameof(courseService));
            _mailService = mailService;
        }

        public async Task<Response<int>> AddStudentAsync(AddStudentDto addStudentDto)
        {
            string userId = "";

            try
            {
                userId = await _accountService.AddStudentAccount(addStudentDto.NameArabic, addStudentDto.NameEnglish,
                   addStudentDto.NationalID, addStudentDto.Email, addStudentDto.Password);
            }
            catch(Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "StudentService",
                    MethodName = "AddStudentAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<int>.ServerError("Error occured while adding student",
                     "An unexpected error occurred while adding student. Please try again later.");
            }

            if (string.IsNullOrEmpty(userId))
            {
                Student newStudent = new Student
                {
                    UserId = userId,
                    PlaceOfBirth = addStudentDto.PlaceOfBirth,
                    Gender = addStudentDto.Gender,
                    Nationality = addStudentDto.Nationality,
                    Religion = addStudentDto.Religion,
                    DateOfBirth = addStudentDto.DateOfBirth,
                    CountryId = addStudentDto.CountryId,
                    GovernorateId = addStudentDto.GovernorateId,
                    CityId = addStudentDto.CityId,
                    Street = addStudentDto.Street,
                    PostalCode = addStudentDto.PostalCode
                };

                try
                {
                    await _unitOfWork.Students.AddAsync(newStudent);
                    int result = await _unitOfWork.SaveAsync();
                }
                catch(Exception ex)
                {
                    await _mailService.SendExceptionEmail(new ExceptionEmailModel
                    {
                        ClassName = "StudentService",
                        MethodName = "AddStudentAsync",
                        ErrorMessage = ex.Message,
                        StackTrace = ex.StackTrace,
                        Time = DateTime.UtcNow
                    });
                    await _accountService.DeleteUser(userId);
                    return Response<int>.ServerError("Error occured while adding student",
                         "An unexpected error occurred while adding student. Please try again later.");
                }

                int studentId = newStudent.Id;
                QualificationData newQualificationDataStudent = new QualificationData
                {
                    StudentId = studentId,
                    PreQualification = addStudentDto.PreQualification,
                    SeatNumber = addStudentDto.SeatNumber,
                    QualificationYear = addStudentDto.QualificationYear,
                    Degree = addStudentDto.Degree
                };

                try
                {
                    await _unitOfWork.QualificationDatas.AddAsync(newQualificationDataStudent);
                    await _unitOfWork.SaveAsync();
                }
                catch(Exception ex)
                {
                    await _mailService.SendExceptionEmail(new ExceptionEmailModel
                    {
                        ClassName = "StudentService",
                        MethodName = "AddStudentAsync",
                        ErrorMessage = ex.Message,
                        StackTrace = ex.StackTrace,
                        Time = DateTime.UtcNow
                    });
                    await _unitOfWork.Students.Delete(newStudent);
                    await _accountService.DeleteUser(userId);
                    return Response<int>.ServerError("Error occured while adding student",
                         "An unexpected error occurred while adding student. Please try again later.");
                }

                FamilyData FamilyDataStudent = new FamilyData
                {
                    StudentId = studentId,

                    ParentName = addStudentDto.ParentName,
                    Job = addStudentDto.ParentJob,
                    CountryId = addStudentDto.ParentCountryId,
                    GovernorateId = addStudentDto.ParentGovernorateId,
                    CityId = addStudentDto.ParentCityId,
                    Street = addStudentDto.ParentStreet
                };

                try
                {
                    await _unitOfWork.FamilyDatas.AddAsync(FamilyDataStudent);
                    await _unitOfWork.SaveAsync();
                }
                catch(Exception ex)
                {
                    await _mailService.SendExceptionEmail(new ExceptionEmailModel
                    {
                        ClassName = "StudentService",
                        MethodName = "AddStudentAsync",
                        ErrorMessage = ex.Message,
                        StackTrace = ex.StackTrace,
                        Time = DateTime.UtcNow
                    });
                    await _unitOfWork.QualificationDatas.Delete(newQualificationDataStudent);
                    await _unitOfWork.Students.Delete(newStudent);
                    await _accountService.DeleteUser(userId);
                    return Response<int>.ServerError("Error occured while adding student",
                         "An unexpected error occurred while adding student. Please try again later.");
                }

                try
                {
                    if (addStudentDto.PhoneNumbers != null)
                    {
                        List<Phone> phones = addStudentDto.PhoneNumbers.Select(ph =>
                            new Phone
                            {
                                StudentId = studentId,
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
                        ClassName = "StudentService",
                        MethodName = "AddStudentAsync",
                        ErrorMessage = ex.Message,
                        StackTrace = ex.StackTrace,
                        Time = DateTime.UtcNow
                    });
                    await _unitOfWork.FamilyDatas.Delete(FamilyDataStudent);
                    await _unitOfWork.QualificationDatas.AddAsync(newQualificationDataStudent);
                    await _unitOfWork.Students.Delete(newStudent);
                    await _accountService.DeleteUser(userId);
                    return Response<int>.ServerError("Error occured while adding student",
                         "An unexpected error occurred while adding student. Please try again later.");
                }

                return Response<int>.Created("Student added successfully");
            }

            return Response<int>.ServerError("Error occured while adding student",
                         "An unexpected error occurred while adding student. Please try again later.");
        }

        public async Task<Response<int>> AddStudentSemesterAsync(AddStudentSemesterDto addStudentSemesterDto)
        {
            StudentSemester newStudentSemester = new StudentSemester
            {
                StudentId = addStudentSemesterDto.StudentId,
                DepartmentId = addStudentSemesterDto.DepartmentId,
                ScientificDegreeId = addStudentSemesterDto.ScientificDegreeId,
                AcademyYearId = addStudentSemesterDto.AcademyYearId
            };

            try
            {
                await _unitOfWork.StudentSemesters.AddAsync(newStudentSemester);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "StudentService",
                    MethodName = "AddStudentSemesterAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<int>.ServerError("Error occured while adding student to semester",
                     "An unexpected error occurred while adding to semester. Please try again later.");
            }

            bool flag;

            try
            {
                flag = await AddCourseStudent(newStudentSemester.Id, newStudentSemester.ScientificDegreeId);
            }
            catch(Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "StudentService",
                    MethodName = "AddStudentSemesterAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                await _unitOfWork.StudentSemesters.Delete(newStudentSemester);
                return Response<int>.ServerError("Error occured while adding student to semester",
                     "An unexpected error occurred while adding student to semester. Please try again later.");
            }

            bool flag1;

            try
            {
                flag1 = await AddCourseAssessMethodStudent(newStudentSemester.Id, newStudentSemester.ScientificDegreeId);
            }
            catch( Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "StudentService",
                    MethodName = "AddStudentSemesterAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                // revert AddCourseStudent() //bastawy
                await _unitOfWork.StudentSemesters.Delete(newStudentSemester);
                return Response<int>.ServerError("Error occured while adding student to semester",
                     "An unexpected error occurred while adding student to semester. Please try again later.");
            }

            if (flag && flag1)
            {
                return Response<int>.Created("Student assigned to semester successfully");
            }

            return Response<int>.ServerError("Error occured while adding student to semester",
                     "An unexpected error occurred while adding student to semester. Please try again later.");
        }

        public async Task<Response<int>> AssignCoursesToStudents()
        {
            var studentsWithScientificDegreeId = await GetTheCurrentSemesterWithStudents();

            var students = studentsWithScientificDegreeId.First().Students;
            var scientificDegreeId = students.First().ScientificDegreeId;

            foreach (var student in students)
            {
                bool flag;

                try
                {
                    flag = await AddCourseStudent(student.Id, scientificDegreeId);
                }
                catch (Exception ex)
                {
                    await _mailService.SendExceptionEmail(new ExceptionEmailModel
                    {
                        ClassName = "StudentService",
                        MethodName = "AssignCoursesToStudents",
                        ErrorMessage = ex.Message,
                        StackTrace = ex.StackTrace,
                        Time = DateTime.UtcNow
                    });
                    return Response<int>.ServerError("Error occured while adding student to semester",
                         "An unexpected error occurred while adding student to semester. Please try again later.");
                }

                bool flag1;

                try
                {
                    flag1 = await AddCourseAssessMethodStudent(student.Id, scientificDegreeId);
                }
                catch (Exception ex)
                {
                    await _mailService.SendExceptionEmail(new ExceptionEmailModel
                    {
                        ClassName = "StudentService",
                        MethodName = "AssignCoursesToStudents",
                        ErrorMessage = ex.Message,
                        StackTrace = ex.StackTrace,
                        Time = DateTime.UtcNow
                    });
                    // revert AddCourseStudent() //bastawy
                    return Response<int>.ServerError("Error occured while adding student to semester",
                         "An unexpected error occurred while adding student to semester. Please try again later.");
                }

                if (!(flag && flag1))
                {
                    return Response<int>.ServerError("Error occured while adding student to semester",
                         "An unexpected error occurred while adding student to semester. Please try again later.");
                }

            }
            return Response<int>.Created("Student assigned to semester successfully");
        }

        private async Task<bool> AddOldCoursesIfExist(int scientificDegreeId)
        {
            var currentSemester = await _unitOfWork.ScientificDegrees.GetByIdAsync(scientificDegreeId);

            var scientificDegrees = await _unitOfWork.ScientificDegrees
                .GetEntityByPropertyAsync(degree => degree.BylawId == currentSemester.BylawId);

            var allSemesters = scientificDegrees.Where(degree => degree.Type == currentSemester.Type);

            

            return true;
        }

        private async Task<List<SemesterStudentsDTO>> GetTheCurrentSemesterWithStudents()
        {
            var students = await _unitOfWork.StudentSemesters.GetTheCurrentSemesterWithStudents();

            var result = students.Select(group => new SemesterStudentsDTO
            {
                ScientificDegreeId = (int)group.GetType().GetProperty("ScientificDegreeId").GetValue(group, null),
                Students = (List<StudentSemester>)group.GetType().GetProperty("Students").GetValue(group, null)
            }).ToList();

            return result;
        }

        public async Task<List<CourseDto>> GetCourseByScientificDegree(int scientificDegreeId)
        {
            IQueryable<CourseDto> coursesQuery = await _courseService.GetCoursesByScientificDegreeIdAsync(scientificDegreeId);
            List<CourseDto> courses = coursesQuery.ToList();
            return courses;
        }

        public async Task<CourseAssessMethodDto> GetAssessMethodsCourse(int courseId)
        {
            CourseAssessMethodDto courseAssessMethodDto = await _courseService.GetAssessMethodsByCoursesIdAsync(courseId);

            var assessMethodDtos = new CourseAssessMethodDto
            {
                CourseAssessMethods = courseAssessMethodDto.CourseAssessMethods
            };

            return assessMethodDtos;
        }

        private async Task<bool> AddCourseStudent(int studentId, int scientificDegreeId)
        {
            //IQueryable<CourseDto> coursesQuery = await _courseService.GetCoursesByScientificDegreeIdAsync(scientificDegreeId);
            List<CourseDto> courses = await GetCourseByScientificDegree(scientificDegreeId);

            List<StudentSemesterCourse> studentSemesterCourses = courses.Select(course => new StudentSemesterCourse
            {
                StudentSemesterId = studentId,
                CourseId = course.Id
            }).ToList();
            await _unitOfWork.StudentSemesterCourses.AddRangeAsync(studentSemesterCourses);
            await _unitOfWork.SaveAsync();
            return true;
        }

        private async Task<bool> AddCourseAssessMethodStudent(int studentId, int scientificDegreeId)
        {
            List<CourseDto> courses = await GetCourseByScientificDegree(scientificDegreeId);
            foreach (var course in courses)
            {
                CourseAssessMethodDto courseAssessMethodDto = await GetAssessMethodsCourse(course.Id);
                List<StudentSemesterAssessMethod> newStudentSemesterAssessMethod = courseAssessMethodDto.CourseAssessMethods.Select(ac =>
                new StudentSemesterAssessMethod
                {
                    StudentSemesterId = studentId,
                    CourseAssessMethodId = ac.Id
                }).ToList();
                await _unitOfWork.StudentSemesterAssessMethods.AddRangeAsync(newStudentSemesterAssessMethod);
                await _unitOfWork.SaveAsync();
            }
            return true;
        }

        public async Task<Response<GetStudentDetailsByUserIdDto>> GetStudentByUserId(string userId)
        {
            try
            {
                SqlParameter pUserId = new SqlParameter("@UserId", userId);

                var getStudent = await _unitOfWork.GetStudentDetailsByUserIdModels.CallStoredProcedureAsync(
                    "EXECUTE SpGetStudentDetailsByUserId", pUserId);
                if (!getStudent.Any())
                    return Response<GetStudentDetailsByUserIdDto>.BadRequest("This Student doesn't exists");

                GetStudentDetailsByUserIdDto getStudentDetailsByUserIdDto = new GetStudentDetailsByUserIdDto
                {
                    NameArabic = getStudent.FirstOrDefault()?.NameArabic,
                    NameEnglish = getStudent.FirstOrDefault()?.NameEnglish,
                    NationalID = getStudent.FirstOrDefault()?.NationalID,
                    Email = getStudent.FirstOrDefault()?.Email,
                    StudentId = getStudent.FirstOrDefault()?.StudentId ?? 0,
                    StudentAddress = getStudent.FirstOrDefault()?.StudentAddress,
                    DateOfBirth = getStudent.FirstOrDefault()?.DateOfBirth,
                    Gender = Enum.GetName(typeof(Gender), getStudent.FirstOrDefault()?.Gender),
                    Nationality = Enum.GetName(typeof(Nationality), getStudent.FirstOrDefault()?.Nationality),
                    PlaceOfBirth = getStudent.FirstOrDefault()?.PlaceOfBirth,
                    PostalCode = getStudent.FirstOrDefault()?.PostalCode,
                    ReleasePlace = getStudent.FirstOrDefault()?.ReleasePlace,
                    Religion = Enum.GetName(typeof(Religion), getStudent.FirstOrDefault()?.Religion),
                    ParentName = getStudent.FirstOrDefault()?.ParentName,
                    ParentJob = getStudent.FirstOrDefault()?.ParentJob,
                    PostalCodeOfParent = getStudent.FirstOrDefault()?.PostalCodeOfParent,
                    ParentAddress = getStudent.FirstOrDefault()?.ParentAddress,
                    PreQualification = getStudent.FirstOrDefault()?.PreQualification,
                    QualificationYear = getStudent.FirstOrDefault()?.QualificationYear,
                    SeatNumber = getStudent.FirstOrDefault()?.SeatNumber ?? 0,
                    Degree = getStudent.FirstOrDefault()?.Degree ?? 0.0m,
                };

                if (getStudent.Any(s => !string.IsNullOrEmpty(s.StudentPhoneNumber)))
                {
                    getStudentDetailsByUserIdDto.GetPhoneStudentDtos = getStudent
                        .Where(s => !string.IsNullOrEmpty(s.StudentPhoneNumber))
                        .Select(s => new GetPhoneStudentDto
                        {
                            StudentPhoneNumber = s.StudentPhoneNumber,
                            PhoneType = Enum.GetName(typeof(PhoneType), s.PhoneType)
                        })
                        .ToList();
                }
                return Response<GetStudentDetailsByUserIdDto>.Success(getStudentDetailsByUserIdDto, "Student data retrieved successfully")
                    .WithCount();
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "StudentService",
                    MethodName = "GetStudentByUserId",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<GetStudentDetailsByUserIdDto>.ServerError("Error occured while retrieving student's data",
                    "An unexpected error occurred while retrieving student's data. Please try again later.");
            }
        }

        public async Task<Response<List<GetAllStudentsDto>>> GetAllStudentsAsync()
        {
            try
            {
                var students = await _unitOfWork.GetAllModels.CallStoredProcedureAsync("EXECUTE SpGetAllStudents");
                if (!students.Any())
                    return Response<List<GetAllStudentsDto>>.BadRequest("This Student doesn't exists");

                List<GetAllStudentsDto> result = students.Select(student => new GetAllStudentsDto
                {
                    StudentId = student.Id,
                    UserId = student.UserId,
                    Nationality = Enum.GetName(typeof(Gender), student.Nationality),
                    StudentNameArbic = student.NameArabic,
                    StudentNameEnglish = student.NameEnglish,
                    Gender = Enum.GetName(typeof(Gender), student.Gender),
                    Religion = Enum.GetName(typeof(Gender), student.Religion),
                    Email = student.Email
                }).ToList();

                return Response<List<GetAllStudentsDto>>.Success(result, "Students retrieved successfully").WithCount();
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "StudentService",
                    MethodName = "GetAllStudentsAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<List<GetAllStudentsDto>>.ServerError("Error occured while retrieving students",
                    "An unexpected error occurred while retrieving students. Please try again later.");
            }
        }
    }
}
