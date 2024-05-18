using GraduationProject.Data.Entity;
using GraduationProject.Mails.IService;
using GraduationProject.Mails.Models;
using GraduationProject.Repository.IRepository;
using GraduationProject.Repository.Repository;
using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.FormatStudentCodeDto;
using GraduationProject.Service.IService;

namespace GraduationProject.Service.Service
{
    public class FormatStudentCodeService : IFormatStudentCodeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMailService _mailService;

        public FormatStudentCodeService(UnitOfWork unitOfWork, IMailService mailService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mailService = mailService;
        }
        public async Task<Response<int>> AddFormatStudentCodeAsync(FormatStudentCodeDto addFormatStudentCodeDto)
        {
            try
            {
                FormatStudentCode newFormatStudentCode = new FormatStudentCode
                {
                    FormatStudentCodeName = addFormatStudentCodeDto.FormatStudentCodeName,
                    FacultyId = addFormatStudentCodeDto.FacultyId
                };
                await _unitOfWork.FormatStudentCodes.AddAsync(newFormatStudentCode);
                var result = await _unitOfWork.SaveAsync();

                if (result > 0)
                {
                    return Response<int>.Created("Format Student Code added successfully");
                }

                return Response<int>.ServerError("Error occured while adding Format Student Code",
                    "An unexpected error occurred while adding Format Student Code. Please try again later.");
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "FormatStudentCodeService",
                    MethodName = "AddFormatStudentCodeAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<int>.ServerError("Error occured while adding Format Student Code",
                    "An unexpected error occurred while adding Format Student Code. Please try again later.");
            }
        }

        public async Task<Response<FormatStudentCodeDto>> GetFormatStudentCodeByFacultyIdAsync(int facultyId)
        {
            try
            {
                var formatStudentCodeEntity = await _unitOfWork.FormatStudentCodes.GetEntityByPropertyAsync(f => f.FacultyId == facultyId);

                if (formatStudentCodeEntity == null)
                    return Response<FormatStudentCodeDto>.BadRequest("This Format Student Code doesn't exist");

                FormatStudentCodeDto formatStudentCodeDto = new FormatStudentCodeDto
                {
                    Id = formatStudentCodeEntity.FirstOrDefault().Id,
                    FormatStudentCodeName = formatStudentCodeEntity.FirstOrDefault().FormatStudentCodeName,
                    FacultyId = formatStudentCodeEntity.FirstOrDefault().FacultyId
                };

                return Response<FormatStudentCodeDto>.Success(formatStudentCodeDto, "Format Student Code retrieved successfully").WithCount();
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "FormatStudentCodeService",
                    MethodName = "GetFormatStudentCodeByFacultyIdAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<FormatStudentCodeDto>.ServerError("Error occured while retrieving Format Student Code",
                    "An unexpected error occurred while retrieving Format Student Code. Please try again later.");
            }
        }

        public async Task<Response<FormatStudentCodeDto>> GetFormatStudentCodeByIdAsync(int formatStudentCodeId)
        {
            try
            {
                var formatStudentCodeEntity = await _unitOfWork.FormatStudentCodes.GetByIdAsync(formatStudentCodeId);

                if (formatStudentCodeEntity == null)
                    return Response<FormatStudentCodeDto>.BadRequest("This Format Student Code doesn't exist");

                FormatStudentCodeDto formatStudentCodeDto = new FormatStudentCodeDto
                {
                    Id = formatStudentCodeEntity.Id,
                    FormatStudentCodeName = formatStudentCodeEntity.FormatStudentCodeName,
                    FacultyId = formatStudentCodeEntity.FacultyId
                };

                return Response<FormatStudentCodeDto>.Success(formatStudentCodeDto, "Format Student Code retrieved successfully").WithCount();
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "FormatStudentCodeService",
                    MethodName = "GetFormatStudentCodeByIdAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<FormatStudentCodeDto>.ServerError("Error occured while retrieving Format Student Code",
                    "An unexpected error occurred while retrieving Format Student Code. Please try again later.");
            }
        }

        public async Task<Response<int>> UpdateFormatStudentCodeAsync(FormatStudentCodeDto updateFormatStudentCodeDto)
        {
            try
            {
                FormatStudentCode existingformatStudentCode = await _unitOfWork.FormatStudentCodes.GetByIdAsync(updateFormatStudentCodeDto.Id ?? 0);

                if (existingformatStudentCode == null)
                    return Response<int>.BadRequest("This Format Student Code doesn't exist");

                existingformatStudentCode.FormatStudentCodeName = updateFormatStudentCodeDto.FormatStudentCodeName;
                existingformatStudentCode.FacultyId = updateFormatStudentCodeDto.FacultyId;

                await _unitOfWork.FormatStudentCodes.Update(existingformatStudentCode);
                var result = await _unitOfWork.SaveAsync();

                if (result > 0)
                    return Response<int>.Updated("Format Student Code updated successfully");

                return Response<int>.ServerError("Error occured while updating Format Student Code",
                        "An unexpected error occurred while updating Format Student Code. Please try again later.");
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "FormatStudentCodeService",
                    MethodName = "UpdateFormatStudentCodeAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<int>.ServerError("Error occured while updating Format Student Code",
                        "An unexpected error occurred while updating Format Student Code. Please try again later.");
            }
        }
        public async Task<Response<int>> DeleteFormatStudentCodeAsync(int formatStudentCodeId)
        {
            try
            {
                var existingFormatStudentCode = await _unitOfWork.FormatStudentCodes.GetByIdAsync(formatStudentCodeId);

                if (existingFormatStudentCode == null)
                    return Response<int>.BadRequest("This Format Student Code doesn't exist");

                await _unitOfWork.FormatStudentCodes.Delete(existingFormatStudentCode);
                var result = await _unitOfWork.SaveAsync();

                if (result > 0)
                    return Response<int>.Deleted("Format Student Code deleted successfully");

                return Response<int>.ServerError("Error occured while deleting Format Student Code",
                        "An unexpected error occurred while deleting Format Student Code. Please try again later.");
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "FormatStudentCodeService",
                    MethodName = "DeleteFormatStudentCodeAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<int>.ServerError("Error occured while deleting Format Student Code",
                        "An unexpected error occurred while deleting Format Student Code. Please try again later.");
            }
        }
    }
}
