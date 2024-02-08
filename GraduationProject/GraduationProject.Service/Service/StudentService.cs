using GraduationProject.Data.Entity;
using GraduationProject.Identity.IService;
using GraduationProject.Repository.Repository;
using GraduationProject.Service.DataTransferObject.StudentDto;
using GraduationProject.Service.IService;

namespace GraduationProject.Service.Service
{
    public class StudentService : IStudentService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IAccountService _accountService;
        public StudentService(UnitOfWork unitOfWork, IAccountService accountService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
        }
        public async Task<int> AddStudentAsync(AddStudentDto addStudentDto)
        {
            string userId = await _accountService.AddStudentAccount(addStudentDto.NameArabic, addStudentDto.NameEnglish,
        addStudentDto.NationalID, addStudentDto.Email, addStudentDto.Password);
            if (!string.IsNullOrEmpty(userId))
            {
                Student student = new Student
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
                _unitOfWork.Students.AddAsync(student);
                _unitOfWork.Save();
                int studentId = student.Id;
                QualificationData qualificationDataStudent = new QualificationData
                {
                    StudentId = studentId,
                    //StaffId = 0,
                    PreQualification = addStudentDto.PreQualification,
                    SeatNumber = addStudentDto.SeatNumber,
                    QualificationYear = addStudentDto.QualificationYear,
                    Degree = addStudentDto.Degree
                };
                _unitOfWork.QualificationDatas.AddAsync(qualificationDataStudent);
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
                _unitOfWork.FamilyDatas.AddAsync(FamilyDataStudent);
                _unitOfWork.Save();
                return 1;

            }
            else
            {
                return -1;
            }
        }
    }
}
