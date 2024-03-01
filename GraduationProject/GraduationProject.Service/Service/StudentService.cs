using GraduationProject.Data.Entity;
using GraduationProject.Identity.IService;
using GraduationProject.Repository.Repository;
using GraduationProject.Service.DataTransferObject.CourseDto;
using GraduationProject.Service.DataTransferObject.StudentDto;
using GraduationProject.Service.DataTransferObject.StudentSemester;
using GraduationProject.Service.IService;

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
                _unitOfWork.Save();
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
                _unitOfWork.Save();
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
                _unitOfWork.Save();
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
    }
}
