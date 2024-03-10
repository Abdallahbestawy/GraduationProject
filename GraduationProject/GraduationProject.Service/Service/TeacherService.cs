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
                Staff newTeacher = new Staff
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
                await _unitOfWork.Staffs.AddAsync(newTeacher);
                await _unitOfWork.SaveAsync();
                int teacherId = newTeacher.Id;
                QualificationData newQualificationDataStudent = new QualificationData
                {
                    StaffId = teacherId,
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

        public async Task<List<GetAllStaffsDto>> GetAllTeachersAsync()
        {
            var userType = UserType.Teacher;
            SqlParameter pUserType = new SqlParameter("@UserType", userType);
            var teachers = await _unitOfWork.GetAllModels.CallStoredProcedureAsync("EXECUTE SpGetAllStaffs", pUserType);
            if (teachers.Any())
            {

                List<GetAllStaffsDto> result = teachers.Select(teacher => new GetAllStaffsDto
                {
                    StaffId = teacher.Id,
                    UserId = teacher.UserId,
                    Nationality = Enum.GetName(typeof(Nationality), teacher.Nationality),
                    StaffNameArbic = teacher.NameArabic,
                    StaffNameEnglish = teacher.NameEnglish,
                    Gender = Enum.GetName(typeof(Gender), teacher.Gender),
                    Religion = Enum.GetName(typeof(Religion), teacher.Religion),
                    Email = teacher.Email
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
