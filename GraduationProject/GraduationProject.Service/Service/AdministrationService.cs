using GraduationProject.Data.Entity;
using GraduationProject.Data.Enum;
using GraduationProject.Identity.Enum;
using GraduationProject.Identity.IService;
using GraduationProject.Mails.IService;
using GraduationProject.Mails.Models;
using GraduationProject.Repository.Repository;
using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.StaffDto;
using GraduationProject.Service.IService;
using Microsoft.Data.SqlClient;

namespace GraduationProject.Service.Service
{
    public class AdministrationService : IAdministrationService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IAccountService _accountService;
        private readonly IMailService _mailService;

        public AdministrationService(UnitOfWork unitOfWork, IAccountService accountService, IMailService mailService)
        {
            _unitOfWork = unitOfWork;
            _accountService = accountService;
            _mailService = mailService;
        }

        public async Task<Response<int>> AddAdministrationAsync(AddStaffDto addSaffDto)
        {
            string userId = "";

            try
            {
                userId = await _accountService.AddAdministrationAccount(addSaffDto.NameArabic, addSaffDto.NameEnglish,
                       addSaffDto.NationalID, addSaffDto.Email, addSaffDto.Password);
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "AdministrationService",
                    MethodName = "AddAdministrationAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<int>.ServerError("Error occured while adding AddAdministration",
                     "An unexpected error occurred while adding AddAdministration. Please try again later.");
            }

            if (string.IsNullOrEmpty(userId))
                return Response<int>.ServerError("Error occured while adding AddAdministration",
                     "An unexpected error occurred while adding AddAdministration. Please try again later.");

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

            try
            {
                await _unitOfWork.Staffs.AddAsync(newAdministration);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "AdministrationService",
                    MethodName = "AddAdministrationAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                await _accountService.DeleteUser(userId);
                return Response<int>.ServerError("Error occured while adding AddAdministration",
                     "An unexpected error occurred while adding AddAdministration. Please try again later.");
            }

            int AdministrationId = newAdministration.Id;
            QualificationData newQualificationDataStaff = new QualificationData
            {
                StaffId = AdministrationId,
                PreQualification = addSaffDto.PreQualification,
                SeatNumber = addSaffDto.SeatNumber,
                QualificationYear = addSaffDto.QualificationYear,
                Degree = addSaffDto.Degree
            };

            try
            {
                await _unitOfWork.QualificationDatas.AddAsync(newQualificationDataStaff);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "AdministrationService",
                    MethodName = "AddAdministrationAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                await _unitOfWork.Staffs.Delete(newAdministration);
                await _accountService.DeleteUser(userId);
                return Response<int>.ServerError("Error occured while adding AddAdministration",
                     "An unexpected error occurred while adding AddAdministration. Please try again later.");
            }
            try
            {
                if (addSaffDto.PhoneNumbers != null)
                {
                    List<Phone> phones = addSaffDto.PhoneNumbers.Select(ph =>
                        new Phone
                        {
                            StaffId = AdministrationId,
                            PhoneNumber = ph.PhoneNumber,
                            Type = ph.Type,
                        }).ToList();

                    await _unitOfWork.Phones.AddRangeAsync(phones);
                    await _unitOfWork.SaveAsync();
                }
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "AdministrationService",
                    MethodName = "AddAdministrationAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                await _unitOfWork.QualificationDatas.Delete(newQualificationDataStaff);
                await _unitOfWork.Staffs.Delete(newAdministration);
                await _accountService.DeleteUser(userId);
                return Response<int>.ServerError("Error occured while adding Administration",
                     "An unexpected error occurred while adding Administration. Please try again later.");
            }

            return Response<int>.Created("AddAdministration added successfully");
        }

        public async Task<Response<bool>> DeleteAsync(int id)
        {
            try
            {
                var qualificationDataEntity = await _unitOfWork.QualificationDatas.GetEntityByPropertyAsync(std => std.StaffId == id);
                var qualificationData = qualificationDataEntity.FirstOrDefault();
                if (qualificationData != null)
                    await _unitOfWork.QualificationDatas.Delete(qualificationData);

                var phones = await _unitOfWork.Phones.GetEntityByPropertyAsync(std => std.StaffId == id);
                if (phones != null || phones.Any())
                    await _unitOfWork.Phones.DeleteRangeAsyn(phones);

                var oldstaff = await _unitOfWork.Staffs.GetByIdAsync(id);
                if (oldstaff == null)
                    return Response<bool>.BadRequest("This user doesn't exist");

                await _unitOfWork.Staffs.Delete(oldstaff);
                int result = await _unitOfWork.SaveAsync();
                if (result > 0)
                {
                    bool flag = await _accountService.DeleteUser(oldstaff.UserId);
                    if (flag)
                        return Response<bool>.Deleted("This user Deleted successfully");
                }

                return Response<bool>.ServerError("Error occured while deleting this user",
                    "An unexpected error occurred while deleting this user. Please try again later.");
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "AdministrationService",
                    MethodName = "DeleteAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<bool>.ServerError("Error occured while deleting this user",
                    "An unexpected error occurred while deleting this user. Please try again later.");
            }
        }

        public async Task<Response<List<GetAllStaffsDto>>> GetAllAdministrationsAsync()
        {
            try
            {
                var userType = UserType.Administration;
                SqlParameter pUserType = new SqlParameter("@UserType", userType);
                var administrations = await _unitOfWork.GetAllModels.CallStoredProcedureAsync("EXECUTE SpGetAllStaffs", pUserType);

                if (!administrations.Any())
                    return Response<List<GetAllStaffsDto>>.NoContent("No AddAdministrations are exist");

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

                return Response<List<GetAllStaffsDto>>.Success(result, "AddAdministrations retrieved successfully").WithCount();
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "AdministrationService",
                    MethodName = "GetAllAdministrationsAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<List<GetAllStaffsDto>>.ServerError("Error occured while retrieving AddAdministrations",
                     "An unexpected error occurred while retrieving AddAdministrations. Please try again later.");
            }
        }

        //need to handle errors ocuuered during the updating process
        public async Task<Response<int>> UpdateStaffAsync(AddStaffDto updateStaffDto)
        {
            try
            {
                var existingStaff = await _unitOfWork.Staffs.GetByIdAsync(updateStaffDto.id ?? 0);
                if (existingStaff == null)
                    return Response<int>.BadRequest("This staff doesn't exist ");

                SqlParameter pUserId = new SqlParameter("@UserId", existingStaff.UserId);
                var getStaff = await _unitOfWork.GetStaffDetailsByUserIdModels.CallStoredProcedureAsync(
                    "EXECUTE SpGetStaffDetailsByUserId", pUserId);

                if (!getStaff.Any() || getStaff == null)
                    return Response<int>.NoContent("This user doesn't exist");

                existingStaff.PlaceOfBirth = updateStaffDto.PlaceOfBirth;
                existingStaff.Gender = updateStaffDto.Gender;
                existingStaff.Nationality = updateStaffDto.Nationality;
                existingStaff.Religion = updateStaffDto.Religion;
                existingStaff.DateOfBirth = updateStaffDto.DateOfBirth;
                existingStaff.CountryId = updateStaffDto.CountryId;
                existingStaff.GovernorateId = updateStaffDto.GovernorateId;
                existingStaff.CityId = updateStaffDto.CityId;
                existingStaff.Street = updateStaffDto.Street;
                existingStaff.PostalCode = updateStaffDto.PostalCode;
                _unitOfWork.Staffs.Update(existingStaff);
                var qualicationData = await _unitOfWork.QualificationDatas.GetEntityByPropertyAsync(s => s.StaffId == existingStaff.Id);
                if (qualicationData != null || qualicationData.Any())
                {
                    var existingQualicationData = qualicationData.FirstOrDefault();
                    existingQualicationData.PreQualification = updateStaffDto.PreQualification;
                    existingQualicationData.SeatNumber = updateStaffDto.SeatNumber;
                    existingQualicationData.QualificationYear = updateStaffDto.QualificationYear;
                    existingQualicationData.Degree = updateStaffDto.Degree;
                    _unitOfWork.QualificationDatas.Update(existingQualicationData);
                }
                var existingPhones = await _unitOfWork.Phones.GetEntityByPropertyAsync(s => s.StaffId == existingStaff.Id);
                if (existingPhones != null || existingPhones.Any())
                {
                    foreach (var existingPhone in existingPhones)
                    {
                        var updateDtoPhone = updateStaffDto.PhoneNumbers.FirstOrDefault(ph => ph.Id == existingPhone.Id);
                        if (updateDtoPhone != null)
                        {
                            existingPhone.PhoneNumber = updateDtoPhone.PhoneNumber;
                            existingPhone.Type = updateDtoPhone.Type;
                        }
                    }
                    _unitOfWork.Phones.UpdateRangeAsync(existingPhones);
                }
                else
                {
                    if (updateStaffDto.PhoneNumbers != null)
                    {
                        List<Phone> phones = updateStaffDto.PhoneNumbers.Select(ph =>
                            new Phone
                            {
                                StaffId = existingStaff.Id,
                                PhoneNumber = ph.PhoneNumber,
                                Type = ph.Type,
                            }).ToList();

                        await _unitOfWork.Phones.AddRangeAsync(phones);
                    }
                }
                int result = await _unitOfWork.SaveAsync();
                if (result > 0)
                    return Response<int>.Updated("This staff updated successfully");

                return Response<int>.ServerError("Error occured while updating user",
                     "An unexpected error occurred while updating user. Please try again later.");
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "AdministrationService",
                    MethodName = "UpdateStaffAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<int>.ServerError("Error occured while updating user",
                     "An unexpected error occurred while updating user. Please try again later.");
            }
        }
    }
}
