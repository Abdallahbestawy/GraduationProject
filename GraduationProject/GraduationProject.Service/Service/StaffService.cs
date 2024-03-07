using GraduationProject.Data.Entity;
using GraduationProject.Identity.IService;
using GraduationProject.Repository.Repository;
using GraduationProject.Service.DataTransferObject.StaffDto;
using GraduationProject.Service.IService;

namespace GraduationProject.Service.Service
{
    public class StaffService : IStaffService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IAccountService _accountService;
        private readonly ICourseService _courseService;
        public StaffService(UnitOfWork unitOfWork, IAccountService accountService, ICourseService courseService)
        {
            _unitOfWork = unitOfWork;
            _accountService = accountService;
            _courseService = courseService;
        }
        public async Task<int> AddStAffAsync(AddStaffDto addSaffDto)
        {
            string userId = await _accountService.AddStaffAccount(addSaffDto.NameArabic, addSaffDto.NameEnglish,
        addSaffDto.NationalID, addSaffDto.Email, addSaffDto.Password);
            if (!string.IsNullOrEmpty(userId))
            {
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
                _unitOfWork.Staffs.AddAsync(newStaff);
                _unitOfWork.Save();
                int staffId = newStaff.Id;
                QualificationData newQualificationDataStudent = new QualificationData
                {
                    StaffId = staffId,
                    //StaffId = 0,
                    PreQualification = addSaffDto.PreQualification,
                    SeatNumber = addSaffDto.SeatNumber,
                    QualificationYear = addSaffDto.QualificationYear,
                    Degree = addSaffDto.Degree
                };
                _unitOfWork.QualificationDatas.AddAsync(newQualificationDataStudent);
                _unitOfWork.Save();
                return 1;

            }
            else
            {
                return -1;
            }
        }

        public async Task<int> AddStaffSemesterAsync(AddStaffSemesterDto addStaffSemesterDto)
        {
            StaffSemester newStaffSemester = new StaffSemester
            {
                StaffId = addStaffSemesterDto.StaffId,
                CourseId = addStaffSemesterDto.CourseId,
                AcademyYearId = addStaffSemesterDto.AcademyYearId
            };
            await _unitOfWork.StaffSemesters.AddAsync(newStaffSemester);
            await _unitOfWork.SaveAsync();
            return 1;
        }

        public async Task<GetCourseStaffSemester> Test(int satffId)
        {
            var staffSemesters = await _unitOfWork.StaffSemesters
               .FindWithIncludeAsync(d => d.AcademyYear, c => c.Course);

            var results = staffSemesters
                .Where(dc => dc.AcademyYear.IsCurrent && dc.StaffId == satffId)
                .ToList();

            if (!results.Any())
            {
                return null;
            }

            var staffSemesterDto = new GetCourseStaffSemester
            {
                StaffId = results.First().StaffId,
                AcademyYearId = results.First().AcademyYearId,
                CourseDoctorDtos = results.Select(result => new CourseDoctorDto
                {
                    CourseId = result.Course.Id,
                    CourseName = result.Course.Name
                }).ToList()
            };

            return staffSemesterDto;
        }

        //public async Task<GetCourseStaffSemester> Test(int staffId)
        //{

        //}

    }




}




