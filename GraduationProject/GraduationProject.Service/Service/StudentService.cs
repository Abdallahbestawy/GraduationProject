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
                _unitOfWork.Students.AddAsync(newStudent);
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
                _unitOfWork.QualificationDatas.AddAsync(newQualificationDataStudent);
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
                if (addStudentDto.PhoneNumbers != null)
                {
                    List<Phone> phones = addStudentDto.PhoneNumbers.Select(ph =>
                        new Phone
                        {
                            StudentId = studentId,
                            PhoneNumber = ph.PhoneNumber,
                            Type = ph.Type,
                        }).ToList();

                    _unitOfWork.Phones.AddRangeAsync(phones);
                    _unitOfWork.Save();
                }
                return 1;

            }
            else
            {
                return -1;
            }
        }
    }
}
