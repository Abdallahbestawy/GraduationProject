using GraduationProject.Data.Entity;
using GraduationProject.Identity.IService;
using GraduationProject.Repository.Repository;
using GraduationProject.Service.DataTransferObject.StaffDto;
using GraduationProject.Service.IService;

namespace GraduationProject.Service.Service
{
    public class TeacherAssistantService : ITeacherAssistantService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IAccountService _accountService;
        public TeacherAssistantService(UnitOfWork unitOfWork, IAccountService accountService)
        {
            _unitOfWork = unitOfWork;
            _accountService = accountService;
        }
        public async Task<int> AddTeacherAssistantAsync(AddStaffDto addTeacherAssistantDto)
        {
            string userId = await _accountService.AddTeacherAssistantAccount(addTeacherAssistantDto.NameArabic, addTeacherAssistantDto.NameEnglish,
                    addTeacherAssistantDto.NationalID, addTeacherAssistantDto.Email, addTeacherAssistantDto.Password);
            if (!string.IsNullOrEmpty(userId))
            {
                Staff staff = new Staff
                {
                    UserId = userId,
                    PlaceOfBirth = addTeacherAssistantDto.PlaceOfBirth,
                    Gender = addTeacherAssistantDto.Gender,
                    Nationality = addTeacherAssistantDto.Nationality,
                    Religion = addTeacherAssistantDto.Religion,
                    DateOfBirth = addTeacherAssistantDto.DateOfBirth,
                    CountryId = addTeacherAssistantDto.CountryId,
                    GovernorateId = addTeacherAssistantDto.GovernorateId,
                    CityId = addTeacherAssistantDto.CityId,
                    Street = addTeacherAssistantDto.Street,
                    PostalCode = addTeacherAssistantDto.PostalCode
                };
                _unitOfWork.Staffs.AddAsync(staff);
                _unitOfWork.Save();
                int staffId = staff.Id;
                QualificationData qualificationDataStudent = new QualificationData
                {
                    StaffId = staffId,
                    PreQualification = addTeacherAssistantDto.PreQualification,
                    SeatNumber = addTeacherAssistantDto.SeatNumber,
                    QualificationYear = addTeacherAssistantDto.QualificationYear,
                    Degree = addTeacherAssistantDto.Degree
                };
                _unitOfWork.QualificationDatas.AddAsync(qualificationDataStudent);
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
