using GraduationProject.Data.Entity;
using GraduationProject.Identity.IService;
using GraduationProject.Repository.Repository;
using GraduationProject.Service.DataTransferObject.StaffDto;
using GraduationProject.Service.IService;

namespace GraduationProject.Service.Service
{

    public class TeacherService : ITeacherService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IAccountService _accountService;
        public TeacherService(UnitOfWork unitOfWork, IAccountService accountService)
        {
            _unitOfWork = unitOfWork;
            _accountService = accountService;
        }
        public async Task<int> AddTeacheAsync(AddStaffDto addSaffDto)
        {
            string userId = await _accountService.AddTeacherAccount(addSaffDto.NameArabic, addSaffDto.NameEnglish,
                              addSaffDto.NationalID, addSaffDto.Email, addSaffDto.Password);
            if (!string.IsNullOrEmpty(userId))
            {
                Staff staff = new Staff
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
                _unitOfWork.Staffs.AddAsync(staff);
                _unitOfWork.Save();
                int staffId = staff.Id;
                QualificationData qualificationDataStudent = new QualificationData
                {
                    StaffId = staffId,
                    //StaffId = 0,
                    PreQualification = addSaffDto.PreQualification,
                    SeatNumber = addSaffDto.SeatNumber,
                    QualificationYear = addSaffDto.QualificationYear,
                    Degree = addSaffDto.Degree
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
