using GraduationProject.Data.Entity;
using GraduationProject.Data.Enum;
using GraduationProject.Identity.Enum;
using GraduationProject.Identity.IService;
using GraduationProject.Repository.Repository;
using GraduationProject.Service.DataTransferObject.StaffDto;
using GraduationProject.Service.IService;
using Microsoft.Data.SqlClient;

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

        public async Task<List<GetAllStaffsDto>> GetAllAdministrationsAsync()
        {
            var userType = UserType.Administration;
            SqlParameter pUserType = new SqlParameter("@UserType", userType);
            var administrations = await _unitOfWork.GetAllModels.CallStoredProcedureAsync("EXECUTE SpGetAllStaffs", pUserType);
            if (administrations.Any())
            {

                List<GetAllStaffsDto> result = administrations.Select(administration => new GetAllStaffsDto
                {
                    StaffId = administration.Id,
                    UserId = administration.UserId,
                    Nationality = Enum.GetName(typeof(Nationality), administration.Nationality),
                    StaffNameArbic = administration.NameArabic,
                    StaffNameEnglish = administration.NameEnglish,
                    Gender = Enum.GetName(typeof(Gender), administration.Gender),
                    Religion = Enum.GetName(typeof(Religion), administration.Religion),
                    Email = administration.Email
                }).ToList();

                return result;
            }
            else
            {
                return null;
            }
        }
    }
}
