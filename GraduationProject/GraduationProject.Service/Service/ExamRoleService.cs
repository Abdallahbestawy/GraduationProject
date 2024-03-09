using GraduationProject.Data.Entity;
using GraduationProject.Mails.IService;
using GraduationProject.Mails.Models;
using GraduationProject.Repository.IRepository;
using GraduationProject.Repository.Repository;
using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.ExamRolesDto;
using GraduationProject.Service.IService;

namespace GraduationProject.Service.Service
{
    public class ExamRoleService : IExamRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMailService _mailService;
        public ExamRoleService(UnitOfWork unitOfWork, IMailService mailService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mailService = mailService;
        }
        public async Task<Response<int>> AddExamRoleAsync(ExamRolesDto addExamRoleDto)
        {
            try
            {
                ExamRole newExamRole = new ExamRole
                {
                    Name = addExamRoleDto.Name,
                    Code = addExamRoleDto.Code,
                    Order = addExamRoleDto.Order,
                    FacultyId = addExamRoleDto.FacultyId
                };
                await _unitOfWork.ExamRoles.AddAsync(newExamRole);
                var result = await _unitOfWork.SaveAsync();
                if (result > 0)
                    return Response<int>.Created("Exam Role added successfully");

                return Response<int>.ServerError("Error occured while adding Exam Role",
                    "An unexpected error occurred while adding Exam Role. Please try again later.");
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "ExamRoleService",
                    MethodName = "AddExamRoleAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<int>.ServerError("Error occured while adding Exam Role",
                    "An unexpected error occurred while adding Exam Role. Please try again later.");
            }
        }

        public async Task<Response<IQueryable<ExamRolesDto>>> GetExamRoleAsync()
        {
            try
            {
                var examRolesEntities = await _unitOfWork.ExamRoles.GetAll();
                if (!examRolesEntities.Any())
                    return Response<IQueryable<ExamRolesDto>>.NoContent("No Exam Roles is exist");

                var examRolesDtos = examRolesEntities.Select(entity => new ExamRolesDto
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Code = entity.Code,
                    Order = entity.Order,
                    FacultyId = entity.FacultyId
                });

                return Response<IQueryable<ExamRolesDto>>.Success(examRolesDtos.AsQueryable(), "Exam Roles retrieved successfully").WithCount();
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "ExamRoleService",
                    MethodName = "GetExamRoleAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<IQueryable<ExamRolesDto>>.ServerError("Error occured while retriveing Exam Roles",
                    "An unexpected error occurred while retriveing Exam Roles. Please try again later.");
            }
        }

        public async Task<Response<ExamRolesDto>> GetExamRoleByIdAsync(int ExamRoleId)
        {
            try
            {
                var examRolesEntity = await _unitOfWork.ExamRoles.GetByIdAsync(ExamRoleId);
                if(examRolesEntity == null)
                    return Response<ExamRolesDto>.BadRequest("This Exam Role doesn't exist");
                ExamRolesDto examRolesDto = new ExamRolesDto
                {
                    Id = examRolesEntity.Id,
                    Name = examRolesEntity.Name,
                    Code = examRolesEntity.Code,
                    Order = examRolesEntity.Order,
                    FacultyId = examRolesEntity.FacultyId
                };
                return Response<ExamRolesDto>.Success(examRolesDto, "Exam Role retrieved successfully").WithCount();
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "ExamRoleService",
                    MethodName = "GetExamRoleByIdAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<ExamRolesDto>.ServerError("Error occured while retriveing Exam Role",
                    "An unexpected error occurred while retriveing Exam Role. Please try again later.");
            }
        }

        public async Task<Response<int>> UpdateExamRoleAsync(ExamRolesDto updateExamRoleDto)
        {
            try
            {
                ExamRole existingExamRole = await _unitOfWork.ExamRoles.GetByIdAsync(updateExamRoleDto.Id);
                if (existingExamRole == null)
                    return Response<int>.BadRequest("This Exam Role doesn't exist");
                existingExamRole.Name = updateExamRoleDto.Name;
                existingExamRole.Code = updateExamRoleDto.Code;
                existingExamRole.Order = updateExamRoleDto.Order;
                existingExamRole.FacultyId = updateExamRoleDto.FacultyId;

                await _unitOfWork.ExamRoles.Update(existingExamRole);
                var result = await _unitOfWork.SaveAsync();
                if (result > 0)
                    return Response<int>.Created("Exam Role updated successfully");

                return Response<int>.ServerError("Error occured while updating Exam Role",
                    "An unexpected error occurred while updating Exam Role. Please try again later.");
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "ExamRoleService",
                    MethodName = "UpdateExamRoleAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<int>.ServerError("Error occured while updating Exam Role",
                    "An unexpected error occurred while updating Exam Role. Please try again later.");
            }
        }
        public async Task<Response<int>> DeleteExamRoleAsync(int ExamRoleId)
        {
            try
            {
                var existingExamRole = await _unitOfWork.ExamRoles.GetByIdAsync(ExamRoleId);
                if (existingExamRole == null)
                    return Response<int>.BadRequest("This Exam Role doesn't exist");

                await _unitOfWork.ExamRoles.Delete(existingExamRole);
                var result = await _unitOfWork.SaveAsync();
                if (result > 0)
                    return Response<int>.Created("Exam Role deleted successfully");

                return Response<int>.ServerError("Error occured while deleting Exam Role",
                    "An unexpected error occurred while deleting Exam Role. Please try again later.");
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "ExamRoleService",
                    MethodName = "DeleteExamRoleAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<int>.ServerError("Error occured while deleting Exam Role",
                    "An unexpected error occurred while deleting Exam Role. Please try again later.");
            }
        }
    }
}
