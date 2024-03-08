using GraduationProject.Data.Entity;
using GraduationProject.Identity.IService;
using GraduationProject.Repository.Repository;
using GraduationProject.Service.DataTransferObject.StaffDto;
using GraduationProject.Service.IService;

namespace GraduationProject.Service.Service
{
    public class AdministrationService : IAdministrationService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IAccountService _accountService;
        public AdministrationService(UnitOfWork unitOfWork, IAccountService accountService)
        {
            _unitOfWork = unitOfWork;
            _accountService = accountService;
        }
        public async Task<int> AddAdministrationAsync(AddStaffDto addSaffDto)
        {
            string userId = await _accountService.AddAdministrationAccount(addSaffDto.NameArabic, addSaffDto.NameEnglish,
                   addSaffDto.NationalID, addSaffDto.Email, addSaffDto.Password);
            if (!string.IsNullOrEmpty(userId))
            {
                Staff newAdministration = new Staff
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
                await _unitOfWork.Staffs.AddAsync(newAdministration);
                await _unitOfWork.SaveAsync();
                int AdministrationId = newAdministration.Id;
                QualificationData newQualificationDataStudent = new QualificationData
                {
                    StaffId = AdministrationId,
                    PreQualification = addSaffDto.PreQualification,
                    SeatNumber = addSaffDto.SeatNumber,
                    QualificationYear = addSaffDto.QualificationYear,
                    Degree = addSaffDto.Degree
                };
                await _unitOfWork.QualificationDatas.AddAsync(newQualificationDataStudent);
                await _unitOfWork.SaveAsync();
                return 1;

            }
            else
            {
                return -1;
            }
        }
    }
}
