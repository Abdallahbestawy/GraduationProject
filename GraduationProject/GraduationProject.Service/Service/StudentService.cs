using GraduationProject.Data.Entity;
using GraduationProject.Data.Enum;
using GraduationProject.Identity.IService;
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
        public StudentService(UnitOfWork unitOfWork, IAccountService accountService, ICourseService courseService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
            _courseService = courseService ?? throw new ArgumentNullException(nameof(courseService));
        }
        public async Task<int> AddStudentAsync(AddStudentDto addStudentDto)
        {
            string userId = await _accountService.AddStudentAccount(addStudentDto.NameArabic, addStudentDto.NameEnglish,
        addStudentDto.NationalID, addStudentDto.Email, addStudentDto.Password);
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
                    PostalCode = addStudentDto.PostalCode
                };
                await _unitOfWork.Students.AddAsync(newStudent);
                await _unitOfWork.SaveAsync();
                int studentId = newStudent.Id;
                QualificationData newQualificationDataStudent = new QualificationData
                {
                    StudentId = studentId,
                    PreQualification = addStudentDto.PreQualification,
                    SeatNumber = addStudentDto.SeatNumber,
                    QualificationYear = addStudentDto.QualificationYear,
                    Degree = addStudentDto.Degree
                };
                await _unitOfWork.QualificationDatas.AddAsync(newQualificationDataStudent);
                await _unitOfWork.SaveAsync();
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
                await _unitOfWork.FamilyDatas.AddAsync(FamilyDataStudent);
                await _unitOfWork.SaveAsync();
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
                return 1;

            }
            else
            {
                return -1;
            }
        }

        public async Task<int> AddStudentSemesterAsync(AddStudentSemesterDto addStudentSemesterDto)
        {
            StudentSemester newStudentSemester = new StudentSemester
            {
                StudentId = addStudentSemesterDto.StudentId,
                DepartmentId = addStudentSemesterDto.DepartmentId,
                ScientificDegreeId = addStudentSemesterDto.ScientificDegreeId,
                AcademyYearId = addStudentSemesterDto.AcademyYearId
            };
            await _unitOfWork.StudentSemesters.AddAsync(newStudentSemester);
            await _unitOfWork.SaveAsync();
            bool flag = await AddCourseStudent(newStudentSemester.Id, newStudentSemester.ScientificDegreeId);
            bool flag1 = await AddCourseAssessMethodStudent(newStudentSemester.Id, newStudentSemester.ScientificDegreeId);
            if (flag && flag1)
            {
                return 1;
            }
            return -1;
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
            //SqlParameter pUserId = new SqlParameter("@UserId", SqlDbType.NVarChar, 450);
            //pUserId.Value = userId;
            try
            {
                SqlParameter pUserId = new SqlParameter("@UserId", userId);

                var getStudent = await _unitOfWork.GetStudentDetailsByUserIdModels.CallStoredProcedureAsync(
                    "EXECUTE SpGetStudentDetailsByUserId", pUserId);
                if (getStudent.Any())
                {
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
                else
                {
                    return Response<GetStudentDetailsByUserIdDto>.NoContent("This Students doesn't exists");
                }
            }
            catch
            {
                return Response<GetStudentDetailsByUserIdDto>.ServerError("Error occured while retrieving student's data",
                    "An unexpected error occurred while retrieving student's data. Please try again later.");
            }
        }
    }
}
