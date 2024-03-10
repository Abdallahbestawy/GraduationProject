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
                Staff newTeacherAssistant = new Staff
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
                await _unitOfWork.Staffs.AddAsync(newTeacherAssistant);
                await _unitOfWork.SaveAsync();
                int teacherAssistantId = newTeacherAssistant.Id;
                QualificationData newQualificationDataStudent = new QualificationData
                {
                    StaffId = teacherAssistantId,
                    PreQualification = addTeacherAssistantDto.PreQualification,
                    SeatNumber = addTeacherAssistantDto.SeatNumber,
                    QualificationYear = addTeacherAssistantDto.QualificationYear,
                    Degree = addTeacherAssistantDto.Degree
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

        public async Task<List<GetAllStaffsDto>> GetAllTeacherAssistantsAsync()
        {
            var userType = UserType.TeacherAssistant;
            SqlParameter pUserType = new SqlParameter("@UserType", userType);
            var teacherAssistants = await _unitOfWork.GetAllModels.CallStoredProcedureAsync("EXECUTE SpGetAllStaffs", pUserType);
            if (teacherAssistants.Any())
            {

                List<GetAllStaffsDto> result = teacherAssistants.Select(teacherAssistant => new GetAllStaffsDto
                {
                    StaffId = teacherAssistant.Id,
                    UserId = teacherAssistant.UserId,
                    Nationality = Enum.GetName(typeof(Nationality), teacherAssistant.Nationality),
                    StaffNameArbic = teacherAssistant.NameArabic,
                    StaffNameEnglish = teacherAssistant.NameEnglish,
                    Gender = Enum.GetName(typeof(Gender), teacherAssistant.Gender),
                    Religion = Enum.GetName(typeof(Religion), teacherAssistant.Religion),
                    Email = teacherAssistant.Email
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
