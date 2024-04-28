using GraduationProject.Data.Entity;
using GraduationProject.Data.Enum;
using GraduationProject.Identity.IService;
using GraduationProject.Mails.IService;
using GraduationProject.Mails.Models;
using GraduationProject.Repository.Repository;
using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.CourseDto;
using GraduationProject.Service.DataTransferObject.PhoneDto;
using GraduationProject.Service.DataTransferObject.ScientificDegreeDto;
using GraduationProject.Service.DataTransferObject.StudentDto;
using GraduationProject.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Security.Claims;

namespace GraduationProject.Service.Service
{
    public class StudentService : IStudentService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IAccountService _accountService;
        private readonly ICourseService _courseService;
        private readonly IMailService _mailService;
        private readonly IExcelHelper _excelHelper;

        public StudentService(UnitOfWork unitOfWork, IAccountService accountService, ICourseService courseService, IMailService mailService, IExcelHelper excelHelper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
            _courseService = courseService ?? throw new ArgumentNullException(nameof(courseService));
            _mailService = mailService;
            _excelHelper = excelHelper;
        }

        public async Task<Response<int>> AddStudentAsync(AddStudentDto addStudentDto)
        {
            string userId = "";

            try
            {
                userId = await _accountService.AddStudentAccount(addStudentDto.NameArabic, addStudentDto.NameEnglish,
                   addStudentDto.NationalID, addStudentDto.Email, addStudentDto.Password);
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "StudentService",
                    MethodName = "AddStudentAccount",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<int>.ServerError("Error occured while adding student",
                     "An unexpected error occurred while adding student. Please try again later.");
            }

            if (!string.IsNullOrEmpty(userId))
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
                    PostalCode = addStudentDto.PostalCode,
                    Code = addStudentDto.StudentCode
                };

                try
                {
                    await _unitOfWork.Students.AddAsync(newStudent);
                    int result = await _unitOfWork.SaveAsync();
                }
                catch (Exception ex)
                {
                    await _mailService.SendExceptionEmail(new ExceptionEmailModel
                    {
                        ClassName = "StudentService",
                        MethodName = "AddStudentAccount",
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
                catch (Exception ex)
                {
                    await _mailService.SendExceptionEmail(new ExceptionEmailModel
                    {
                        ClassName = "StudentService",
                        MethodName = "AddStudentAccount",
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
                    Street = addStudentDto.ParentStreet,
                    PostalCode = addStudentDto.PostalCodeOfParent
                };

                try
                {
                    await _unitOfWork.FamilyDatas.AddAsync(FamilyDataStudent);
                    await _unitOfWork.SaveAsync();
                }
                catch (Exception ex)
                {
                    await _mailService.SendExceptionEmail(new ExceptionEmailModel
                    {
                        ClassName = "StudentService",
                        MethodName = "AddStudentAccount",
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
                        MethodName = "AddStudentAccount",
                        ErrorMessage = ex.Message,
                        StackTrace = ex.StackTrace,
                        Time = DateTime.UtcNow
                    });
                    await _unitOfWork.FamilyDatas.Delete(FamilyDataStudent);
                    await _unitOfWork.QualificationDatas.Delete(newQualificationDataStudent);
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

            var academyYear = await _unitOfWork.AcademyYears.GetEntityByPropertyAsync(s => s.IsCurrent);
            if (academyYear == null || !academyYear.Any())
            {
                return Response<int>.ServerError("Error Academy Years",
                 "There Is No Active Academic Year.");
            }
            int acdemyYearAssignt = academyYear.FirstOrDefault().Id;
            var studentsem = await _unitOfWork.StudentSemesters.GetEntityByPropertyAsync(ss => ss.StudentId == addStudentSemesterDto.StudentId
            && ss.AcademyYearId == acdemyYearAssignt && ss.ScientificDegreeId == addStudentSemesterDto.ScientificDegreeId
            && ss.DepartmentId == addStudentSemesterDto.DepartmentId);
            if (studentsem.Any())
            {
                return Response<int>.BadRequest($"This student already assigned in this semester");
            }
            StudentSemester newStudentSemester = new StudentSemester
            {
                StudentId = addStudentSemesterDto.StudentId,
                DepartmentId = addStudentSemesterDto.DepartmentId,
                ScientificDegreeId = addStudentSemesterDto.ScientificDegreeId,
                AcademyYearId = acdemyYearAssignt
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
                await _unitOfWork.StudentSemesters.Delete(newStudentSemester);
                return Response<int>.ServerError("Error occured while adding student to semester",
                     "An unexpected error occurred while adding student to semester. Please try again later.");
            }

            bool flag1;

            try
            {
                flag1 = await AddCourseAssessMethodStudent(newStudentSemester.Id, newStudentSemester.ScientificDegreeId);
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
            List<SemesterStudentsDTO> studentsWithScientificDegreeId = new List<SemesterStudentsDTO>();
            List<StudentSemester> students = new List<StudentSemester>();
            int scientificDegreeId = 0;
            try
            {
                studentsWithScientificDegreeId = await GetTheCurrentSemesterWithStudents();
                students = studentsWithScientificDegreeId.First().Students;
                scientificDegreeId = students.First().ScientificDegreeId;
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
                return Response<int>.ServerError("Error occured while assigning courses to students",
                     "An unexpected error occurred while assigning courses to students. Please try again later.");
            }

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
                    return Response<int>.ServerError("Error occured while assigning courses to students",
                         "An unexpected error occurred while assigning courses to students. Please try again later.");
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
                    return Response<int>.ServerError("Error occured while assigning courses to students",
                         "An unexpected error occurred while assigning courses to students. Please try again later.");
                }

                if (!(flag && flag1))
                {
                    return Response<int>.ServerError("Error occured while assigning courses to students",
                         "An unexpected error occurred while assigning courses to students. Please try again later.");
                }

                bool falg3;
                try
                {
                    falg3 = await AddOldCoursesIfExist(scientificDegreeId, student.StudentId, student.Id);
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
                    return Response<int>.ServerError("Error occured while assigning courses to students",
                         "An unexpected error occurred while assigning courses to students. Please try again later.");
                }

                if (!falg3)
                {
                    //revert what happen in flag1 and flag2
                    return Response<int>.ServerError("Error occured while assigning courses to students",
                         "An unexpected error occurred while assigning courses to students. Please try again later.");
                }
            }
            return Response<int>.Created("Students assigned to semester's courses successfully");
        }

        private async Task<bool> AddOldCoursesIfExist(int scientificDegreeId, int studentId, int studentSemesterId)
        {
            var currentSemester = _unitOfWork.ScientificDegrees.GetEntityByPropertyAsync(semes => semes.Id == scientificDegreeId).Result.SingleOrDefault();

            var scientificDegrees = await _unitOfWork.ScientificDegrees
                .GetEntityByPropertyAsync(degree => degree.BylawId == currentSemester.BylawId);

            var bands = scientificDegrees.Where(degree => degree.Type == ScientificDegreeType.Band)
                .GroupBy(degree => degree.Order).ToList();

            // check if the student in the first year if true the student doesn't have old courses
            if (currentSemester.ParentId == bands.First().First().Id)
                return true;

            var semestersWithTheSameOrder = await GetSemestersWithTheSameOrder(currentSemester, scientificDegrees);
            var oldCoursesIds = new List<int>();

            foreach (var semester in semestersWithTheSameOrder)
            {
                var courses = await GetCourseByScientificDegree(semester);
                var coursesIds = courses.Select(crs => crs.Id);
                var oldCourses = await _unitOfWork.StudentSemesterCourses.FindWithIncludeIEnumerableAsync(crs => crs.StudentSemester);

                oldCoursesIds = oldCourses
                   .Where(crs => crs.StudentSemester.StudentId == studentId && crs.Passing == false && coursesIds.Contains(crs.CourseId))
                   .Select(crs => crs.CourseId).ToList();

                foreach (var course in oldCoursesIds)
                {
                    //add the course
                    List<StudentSemesterCourse> studentSemesterCourses = courses.Where(crs => crs.Id == course)
                        .Select(course => new StudentSemesterCourse
                        {
                            StudentSemesterId = studentSemesterId,
                            CourseId = course.Id
                        }).ToList();
                    await _unitOfWork.StudentSemesterCourses.AddRangeAsync(studentSemesterCourses);

                    //add the assess methods
                    CourseAssessMethodDto courseAssessMethodDto = await GetAssessMethodsCourse(course);
                    List<StudentSemesterAssessMethod> newStudentSemesterAssessMethod = courseAssessMethodDto.CourseAssessMethods.Select(ac =>
                    new StudentSemesterAssessMethod
                    {
                        StudentSemesterId = studentSemesterId,
                        CourseAssessMethodId = ac.Id
                    }).ToList();
                    await _unitOfWork.StudentSemesterAssessMethods.AddRangeAsync(newStudentSemesterAssessMethod);
                }
            }
            await _unitOfWork.SaveAsync();
            return true;
        }

        private async Task<List<int>> GetSemestersWithTheSameOrder(ScientificDegree currentSemester, IEnumerable<ScientificDegree> scientificDegrees)
        {
            var parentOrder = currentSemester.Parent.Order;

            var result = new List<int>();
            foreach (var scientificdegree in scientificDegrees.Where(degree => degree.Type == ScientificDegreeType.Band && degree.Order < parentOrder))
            {
                var semesterId = scientificDegrees.Where(semes => semes.Order == currentSemester.Order && semes.ParentId == scientificdegree.Id)
                    .Select(semes => semes.Id).SingleOrDefault();
                result.Add(semesterId);
            }

            return result;
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
                    StudentCode = getStudent.FirstOrDefault().Code,
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
                            PhoneId = s.PhoneId,
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
                    Nationality = Enum.GetName(typeof(Nationality), student.Nationality),
                    StudentNameArbic = student.NameArabic,
                    StudentNameEnglish = student.NameEnglish,
                    Gender = Enum.GetName(typeof(Gender), student.Gender),
                    Religion = Enum.GetName(typeof(Religion), student.Religion),
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

        public async Task<Response<bool>> DeleteStudentAsync(int studentId)
        {
            try
            {
                var familydataEntity = await _unitOfWork.FamilyDatas.GetEntityByPropertyAsync(std => std.StudentId == studentId);
                var familydata = familydataEntity.FirstOrDefault();

                if (familydata != null)
                    await _unitOfWork.FamilyDatas.Delete(familydata);

                var qualificationDataEntity = await _unitOfWork.QualificationDatas.GetEntityByPropertyAsync(std => std.StudentId == studentId);
                var qualificationData = qualificationDataEntity.FirstOrDefault();
                if (qualificationData != null)
                    await _unitOfWork.QualificationDatas.Delete(qualificationData);

                var phones = await _unitOfWork.Phones.GetEntityByPropertyAsync(std => std.StudentId == studentId);
                if (phones != null || phones.Any())
                    await _unitOfWork.Phones.DeleteRangeAsyn(phones);

                var oldstd = await _unitOfWork.Students.GetByIdAsync(studentId);
                if (oldstd == null)
                    return Response<bool>.BadRequest("This student doesn't exist");

                await _unitOfWork.Students.Delete(oldstd);
                int result = await _unitOfWork.SaveAsync();
                if (result > 0)
                {
                    bool flag = await _accountService.DeleteUser(oldstd.UserId);
                    if (flag)
                        return Response<bool>.Deleted("The student is deleted successfully");
                }
                return Response<bool>.ServerError("Error occured while deleting student",
                    "An unexpected error occurred while deleting student. Please try again later.");
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "StudentService",
                    MethodName = "DeleteStudentAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<bool>.ServerError("Error occured while deleting student",
                    "An unexpected error occurred while deleting student. Please try again later.");
            }
        }

        public async Task<Response<bool>> DeleteStudentSemesterAsync(int studentSemesterId)
        {
            try
            {
                var oldstudentCourse = await _unitOfWork.StudentSemesterCourses.GetEntityByPropertyAsync(sdt => sdt.StudentSemesterId == studentSemesterId);
                if (oldstudentCourse != null || oldstudentCourse.Any())
                {
                    await _unitOfWork.StudentSemesterCourses.DeleteRangeAsyn(oldstudentCourse);
                }

                var oldstudentSemesterAssessMethods = await _unitOfWork.StudentSemesterAssessMethods.GetEntityByPropertyAsync(sdt => sdt.StudentSemesterId == studentSemesterId);
                if (oldstudentSemesterAssessMethods != null || oldstudentSemesterAssessMethods.Any())
                {
                    await _unitOfWork.StudentSemesterAssessMethods.DeleteRangeAsyn(oldstudentSemesterAssessMethods);
                }

                var oldstudentSemester = await _unitOfWork.StudentSemesters.GetByIdAsync(studentSemesterId);
                if (oldstudentSemester == null)
                    return Response<bool>.BadRequest("This student semseter doesn't exist");

                await _unitOfWork.StudentSemesters.Delete(oldstudentSemester);
                int result = await _unitOfWork.SaveAsync();
                if (result > 0)
                    return Response<bool>.Deleted("This student semseter deleted successfully");

                return Response<bool>.ServerError("Error occured while deleting student semseter",
                        "An unexpected error occurred while deleting student semseter. Please try again later.");
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "StudentService",
                    MethodName = "DeleteStudentSemesterAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<bool>.ServerError("Error occured while deleting student semseter",
                        "An unexpected error occurred while deleting student semseter. Please try again later.");
            }
        }

        public async Task<Response<int>> UpdateStudentAsync(UpdateStudentDto updateStudentDto)
        {
            try
            {
                var existingStudent = await _unitOfWork.Students.GetByIdAsync(updateStudentDto.Id);
                if (existingStudent == null)
                    return Response<int>.BadRequest("This student doesn't exist");

                SqlParameter pUserId = new SqlParameter("@UserId", existingStudent.UserId);
                var getStudent = await _unitOfWork.GetStudentDetailsByUserIdModels.CallStoredProcedureAsync(
                    "EXECUTE SpGetStudentDetailsByUserId", pUserId);
                if (!getStudent.Any() || getStudent == null)
                    return Response<int>.NoContent("This student doesn't exist");
                bool flag = await _accountService.UpdateUser(existingStudent.UserId, updateStudentDto.NameArabic, updateStudentDto.NameEnglish, updateStudentDto.NationalID);
                if (!flag)
                {
                    return Response<int>.BadRequest("This student doesn't exist");
                }
                existingStudent.PlaceOfBirth = updateStudentDto.PlaceOfBirth;
                existingStudent.Gender = updateStudentDto.Gender;
                existingStudent.Nationality = updateStudentDto.Nationality;
                existingStudent.Religion = updateStudentDto.Religion;
                existingStudent.DateOfBirth = updateStudentDto.DateOfBirth;
                existingStudent.CountryId = updateStudentDto.CountryId;
                existingStudent.GovernorateId = updateStudentDto.GovernorateId;
                existingStudent.CityId = updateStudentDto.CityId;
                existingStudent.Street = updateStudentDto.Street;
                existingStudent.PostalCode = updateStudentDto.PostalCode;
                existingStudent.Code = updateStudentDto.StudentCode;
                _unitOfWork.Students.Update(existingStudent);
                var qualicationData = await _unitOfWork.QualificationDatas.GetEntityByPropertyAsync(s => s.StudentId == existingStudent.Id);
                if (qualicationData != null || qualicationData.Any())
                {
                    var existingQualicationData = qualicationData.FirstOrDefault();
                    existingQualicationData.PreQualification = updateStudentDto.PreQualification;
                    existingQualicationData.SeatNumber = updateStudentDto.SeatNumber;
                    existingQualicationData.QualificationYear = updateStudentDto.QualificationYear;
                    existingQualicationData.Degree = updateStudentDto.Degree;
                    _unitOfWork.QualificationDatas.Update(existingQualicationData);
                }
                var familyData = await _unitOfWork.FamilyDatas.GetEntityByPropertyAsync(s => s.StudentId == existingStudent.Id);
                if (familyData != null || familyData.Any())
                {
                    var existingFamilyData = familyData.FirstOrDefault();
                    existingFamilyData.ParentName = updateStudentDto.ParentName;
                    existingFamilyData.Job = updateStudentDto.ParentJob;
                    existingFamilyData.CountryId = updateStudentDto.ParentCountryId;
                    existingFamilyData.GovernorateId = updateStudentDto.ParentGovernorateId;
                    existingFamilyData.CityId = updateStudentDto.ParentCityId;
                    existingFamilyData.Street = updateStudentDto.ParentStreet;
                    _unitOfWork.FamilyDatas.Update(existingFamilyData);
                }
                var existingPhones = await _unitOfWork.Phones.GetEntityByPropertyAsync(s => s.StudentId == existingStudent.Id);
                if (existingPhones != null || existingPhones.Any())
                {
                    foreach (var existingPhone in existingPhones)
                    {
                        var updateDtoPhone = updateStudentDto.PhoneNumbers.FirstOrDefault(ph => ph.Id == existingPhone.Id);
                        if (updateDtoPhone != null)
                        {
                            existingPhone.PhoneNumber = updateDtoPhone.PhoneNumber;
                            existingPhone.Type = updateDtoPhone.Type;
                        }
                    }
                    _unitOfWork.Phones.UpdateRangeAsync(existingPhones);
                }
                else
                {
                    if (updateStudentDto.PhoneNumbers != null)
                    {
                        List<Phone> phones = updateStudentDto.PhoneNumbers.Select(ph =>
                            new Phone
                            {
                                StudentId = existingStudent.Id,
                                PhoneNumber = ph.PhoneNumber,
                                Type = ph.Type,
                            }).ToList();

                        await _unitOfWork.Phones.AddRangeAsync(phones);
                    }
                }
                int result = await _unitOfWork.SaveAsync();
                if (result > 0)
                    return Response<int>.Updated("This student updated successfully");

                return Response<int>.ServerError("Error occured while updating student",
                        "An unexpected error occurred while updating student. Please try again later.");
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "StudentService",
                    MethodName = "UpdateStudentAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<int>.ServerError("Error occured while updating student",
                        "An unexpected error occurred while updating student. Please try again later.");
            }
        }

        public async Task<Response<GetStudentResultDto>> GetStudentResultAsync(string userId)
        {
            try
            {
                SqlParameter pUserId = new SqlParameter("@UserId", userId);

                var getStudentResult = await _unitOfWork.GetStudentResultModels.CallStoredProcedureAsync(
                    "EXECUTE SpGetStudentResult", pUserId);

                if (getStudentResult == null || !getStudentResult.Any())
                    return Response<GetStudentResultDto>.NoContent("There is no results available");

                GetStudentResultDto getStudentResultDto = new GetStudentResultDto
                {
                    StudentName = getStudentResult.FirstOrDefault().NameEnglish
                };

                getStudentResultDto.StudentResultDeltiels = getStudentResult.DistinctBy(detiels => detiels.SemesterName).Select(detiels => new StudentResultDeltielsDto
                {
                    SemesterName = detiels.SemesterName,
                    AcademyYearName = detiels.AcademyYear,
                    BandName = detiels.BandName,
                    SemesterStatus = detiels.SemesterStatus,
                    SemesterPercentage = detiels.SemesterPercentage,
                    SemesterChar = detiels.SemesterChar,
                    CumulativePercentage = detiels.CumulativePercentage,
                    CumulativeChar = detiels.CumulativeChar,
                    studentResultDeltielsSemester = getStudentResult.Where(semester => semester.SemesterName == detiels.SemesterName).Select(semester => new StudentResultDeltielsSemesterDto
                    {
                        CourseName = semester.CourseName,
                        CourseCode = semester.CourseCode,
                        NumberOfPoint = semester.NumberOfPoints,
                        CourseDegree = semester.CourseDegree,
                        CourseChar = semester.CourseChar,
                        CourseStatus = semester.CourseStatus
                    }).ToList()
                }).ToList();

                return Response<GetStudentResultDto>.Success(getStudentResultDto, "Student's results retrieved successfully").WithCount();
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "StudentService",
                    MethodName = "GetStudentResultAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<GetStudentResultDto>.ServerError("Error occured while retrieving student's results",
                        "An unexpected error occurred while retrieving student's results. Please try again later.");
            }
        }
        public async Task<Response<List<GetAllStudentsInSemesterDto>>> GetAllStudentsInSemesterAsync(int semesterId)
        {
            try
            {
                SqlParameter pSemesterId = new SqlParameter("@ScientificDegreeId", semesterId);

                var getStudentInSems = await _unitOfWork.GetAllStudentsInSemesterModels.CallStoredProcedureAsync(
                    "EXECUTE SpGetAllStudentsInSemester", pSemesterId);
                if (getStudentInSems == null || !getStudentInSems.Any())
                    return Response<List<GetAllStudentsInSemesterDto>>.NoContent("No students are exist");

                List<GetAllStudentsInSemesterDto> getAllStudentsInSemesterDtos = getStudentInSems.Select(ses => new GetAllStudentsInSemesterDto
                {
                    StudentSemesterId = ses.Id,
                    StudentName = ses.NameEnglish,
                    StudentCode = ses.Code
                }).ToList();

                return Response<List<GetAllStudentsInSemesterDto>>.Success(getAllStudentsInSemesterDtos, "Students retrieved successfully").WithCount();
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "StudentService",
                    MethodName = "GetAllStudentsInSemesterAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<List<GetAllStudentsInSemesterDto>>.ServerError("Error occured while retrieving students",
                    "An unexpected error occurred while retrieving students. Please try again later.");
            }
        }

        public async Task<Response<GetStudentInfoByStudentIdDto>> GetStudentInfoByStudentIdAsync(int studentId)
        {
            try
            {
                SqlParameter pStudentId = new SqlParameter("@StudentId", studentId);

                var getStudent = await _unitOfWork.GetStudentInfoByStudentIdModels.CallStoredProcedureAsync(
                    "EXECUTE SpGetStudentInfoByStudentId", pStudentId);

                if (getStudent == null || !getStudent.Any())
                    return Response<GetStudentInfoByStudentIdDto>.NoContent("This student doesn't exist");

                GetStudentInfoByStudentIdDto getStudentInfo = new GetStudentInfoByStudentIdDto
                {
                    NameArabic = getStudent.FirstOrDefault()?.NameArabic,
                    NameEnglish = getStudent.FirstOrDefault()?.NameEnglish,
                    NationalID = getStudent.FirstOrDefault()?.NationalID,
                    Email = getStudent.FirstOrDefault()?.Email,
                    StudentCode = getStudent.FirstOrDefault().Code,
                    StudentId = getStudent.FirstOrDefault()?.StudentId ?? 0,
                    StudentStreet = getStudent.FirstOrDefault().StudentsStreet,
                    StudentCountrysId = getStudent.FirstOrDefault().StudentsCountrysId,
                    StudentGovernoratesId = getStudent.FirstOrDefault()?.StudentsGovernoratesId,
                    StudentCitysId = getStudent.FirstOrDefault()?.StudentsCitysId,
                    DateOfBirth = getStudent.FirstOrDefault()?.DateOfBirth,
                    Gender = getStudent.FirstOrDefault().Gender,
                    Nationality = getStudent.FirstOrDefault().Nationality,
                    PlaceOfBirth = getStudent.FirstOrDefault()?.PlaceOfBirth,
                    PostalCode = getStudent.FirstOrDefault()?.PostalCode,
                    ReleasePlace = getStudent.FirstOrDefault()?.ReleasePlace,
                    Religion = getStudent.FirstOrDefault().Religion,
                    ParentName = getStudent.FirstOrDefault()?.ParentName,
                    ParentJob = getStudent.FirstOrDefault()?.ParentJob,
                    PostalCodeOfParent = getStudent.FirstOrDefault()?.PostalCodeOfParent,
                    ParentStreet = getStudent.FirstOrDefault()?.ParentStreet,
                    ParentCountrysId = getStudent.FirstOrDefault()?.ParentCountrysId,
                    ParentGovernoratesId = getStudent.FirstOrDefault()?.ParentGovernoratesId,
                    ParentCitysId = getStudent?.FirstOrDefault()?.ParentCitysId,
                    PreQualification = getStudent.FirstOrDefault()?.PreQualification,
                    QualificationYear = getStudent.FirstOrDefault()?.QualificationYear,
                    SeatNumber = getStudent.FirstOrDefault()?.SeatNumber ?? 0,
                    Degree = getStudent.FirstOrDefault()?.Degree ?? 0.0m,
                };

                if (getStudent.Any(s => !string.IsNullOrEmpty(s.StudentPhoneNumber)))
                {
                    getStudentInfo.GetPhoneStudentDtos = getStudent
                        .Where(s => !string.IsNullOrEmpty(s.StudentPhoneNumber))
                        .Select(s => new GetPhoneTypeDto
                        {
                            PhoneId = s.PhoneId,
                            PhoneNumber = s.StudentPhoneNumber,
                            PhoneType = s.PhoneType
                        })
                        .ToList();
                }

                return Response<GetStudentInfoByStudentIdDto>.Success(getStudentInfo, "Student's info is retrieved successfully").WithCount();
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "StudentService",
                    MethodName = "GetStudentInfoByStudentIdAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<GetStudentInfoByStudentIdDto>.ServerError("Error occured while retrieving student's info",
                    "An unexpected error occurred while retrieving student's info. Please try again later.");
            }
        }

        public async Task<Response<int>> AddStudentsListFromExcelFileAsync(IFormFile file, ClaimsPrincipal user)
        {
            try
            {
                List<object>? errors = new();
                int counter = 0;
                int totalStudents = 0;

                if (file != null && file.Length > 0)
                {
                    var filePath = _excelHelper.SaveFile(file);

                    var studentsList = _excelHelper.Import<AddStudentDto>(filePath);

                    if (studentsList.ValidationErrors.Count > 0)
                        return Response<int>.BadRequest("One or more validation errors occured", studentsList.ValidationErrors);

                    if (studentsList.MappedData.Count == 0)
                        return Response<int>.BadRequest("No data in the file to add");

                    totalStudents = studentsList.MappedData.Count;
                    foreach (var student in studentsList.MappedData)
                    {
                        var result = await AddStudentAsync(student);
                        if (result.StatusCode != 201)
                        {
                            if (result.Errors != null)
                                errors.Add(result.Errors);
                        }
                        counter++;
                    }
                }
                else
                {
                    // Handle case when no file is uploaded
                    return Response<int>.BadRequest("No file is uploaded");
                }

                return Response<int>.Created($"The students list added successfully, done {counter} out of {totalStudents}");
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "StudentService",
                    MethodName = "AddStudentsListFromExcelFileAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<int>.ServerError("Error occured while adding students list",
                    "An unexpected error occurred while adding students list. Please try again later.");
            }
        }
    }
}
