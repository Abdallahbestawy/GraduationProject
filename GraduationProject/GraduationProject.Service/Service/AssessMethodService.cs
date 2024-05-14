using GraduationProject.Data.Entity;
using GraduationProject.Mails.IService;
using GraduationProject.Mails.Models;
using GraduationProject.Repository.IRepository;
using GraduationProject.Repository.Repository;
using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.AssessMethodDto;
using GraduationProject.Service.IService;

namespace GraduationProject.Service.Service
{
    public class AssessMethodService : IAssessMethodService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMailService _mailService;

        public AssessMethodService(UnitOfWork unitOfWork, IMailService mailService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mailService = mailService;
        }

        public async Task<Response<int>> AddAssessMethodAsync(AssessMethodDto addAssessMethodDto)
        {
            try
            {
                AssessMethod newAssessMethod = new AssessMethod
                {
                    Name = addAssessMethodDto.Name,
                    Description = addAssessMethodDto.Description,
                    MinDegree = addAssessMethodDto.MinDegree,
                    MaxDegree = addAssessMethodDto.MaxDegree,
                    FacultyId = addAssessMethodDto.FacultyId
                };
                await _unitOfWork.AssessMethods.AddAsync(newAssessMethod);
                var result = await _unitOfWork.SaveAsync();

                if (result > 0)
                    return Response<int>.Created("Assess Method added successfully");

                return Response<int>.ServerError("Error occured while adding Assess Method",
                    "An unexpected error occurred while adding Assess Method. Please try again later.");
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "AssessMethodService",
                    MethodName = "AddAssessMethodAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<int>.ServerError("Error occured while adding Assess Method",
                    "An unexpected error occurred while adding Assess Method. Please try again later.");
            }
        }

        public async Task<Response<IQueryable<GetAssessMethodDto>>> GetAssessMethodAsync(int facultyId)
        {
            try
            {
                var assessMethodEntities = await _unitOfWork.AssessMethods.GetEntityByPropertyWithIncludeAsync(f => f.FacultyId == facultyId, d => d.Faculty);

                if (!assessMethodEntities.Any())
                    return Response<IQueryable<GetAssessMethodDto>>.NoContent("No Assess Methods are exist");

                var assessMethodDto = assessMethodEntities.Select(entity => new GetAssessMethodDto
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Description = entity.Description,
                    MaxDegree = entity.MaxDegree,
                    MinDegree = entity.MinDegree,
                    FacultyName = entity.Faculty.Name
                });

                return Response<IQueryable<GetAssessMethodDto>>.Success(assessMethodDto.AsQueryable(), "Assess Methods retrieved successfully")
                    .WithCount();
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "AssessMethodService",
                    MethodName = "GetAssessMethodAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<IQueryable<GetAssessMethodDto>>.ServerError("Error occured while retrieving Assess Methods",
                    "An unexpected error occurred while retrieving Assess Methods. Please try again later.");
            }
        }

        public async Task<Response<AssessMethodDto>> GetAssessMethodByIdAsync(int assessMethodId)
        {
            try
            {
                var assessMethodEntities = await _unitOfWork.AssessMethods.GetByIdAsync(assessMethodId);

                if (assessMethodEntities == null)
                    return Response<AssessMethodDto>.BadRequest("This Assess Method doesn't exist");

                AssessMethodDto assessMethodDto = new AssessMethodDto
                {
                    Id = assessMethodEntities.Id,
                    Name = assessMethodEntities.Name,
                    Description = assessMethodEntities.Description,
                    MinDegree = assessMethodEntities.MinDegree,
                    MaxDegree = assessMethodEntities.MaxDegree,
                    FacultyId = assessMethodEntities.FacultyId
                };

                return Response<AssessMethodDto>.Success(assessMethodDto, "Assess Method retrieved successfully").WithCount();
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "AssessMethodService",
                    MethodName = "GetAssessMethodByIdAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<AssessMethodDto>.ServerError("Error occured while retrieving Assess Method",
                    "An unexpected error occurred while retrieving Assess Method. Please try again later.");
            }
        }

        public async Task<Response<int>> UpdateAssessMethodAsync(AssessMethodDto updateAssessMethodDto)
        {
            try
            {
                AssessMethod existingAssessMethod = await _unitOfWork.AssessMethods.GetByIdAsync(updateAssessMethodDto.Id);

                if (existingAssessMethod == null)
                    return Response<int>.BadRequest("This Assess Method doesn't exist");

                existingAssessMethod.Name = updateAssessMethodDto.Name;
                existingAssessMethod.Description = updateAssessMethodDto.Description;
                existingAssessMethod.MinDegree = updateAssessMethodDto.MinDegree;
                existingAssessMethod.MaxDegree = updateAssessMethodDto.MaxDegree;
                existingAssessMethod.FacultyId = updateAssessMethodDto.FacultyId;

                await _unitOfWork.AssessMethods.Update(existingAssessMethod);
                var result = await _unitOfWork.SaveAsync();

                if (result > 0)
                    return Response<int>.Updated("Assess Method updated successfully");

                return Response<int>.ServerError("Error occured while updating Assess Method",
                    "An unexpected error occurred while updating Assess Method. Please try again later.");
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "AssessMethodService",
                    MethodName = "UpdateAssessMethodAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<int>.ServerError("Error occured while updating Assess Method",
                    "An unexpected error occurred while updating Assess Method. Please try again later.");
            }
        }
        public async Task<Response<int>> DeleteAssessMethodAsync(int assessMethodId)
        {
            try
            {
                var existingAssessMethod = await _unitOfWork.AssessMethods.GetByIdAsync(assessMethodId);

                if (existingAssessMethod == null)
                    return Response<int>.BadRequest("This Assess Method doesn't exist");

                await _unitOfWork.AssessMethods.Delete(existingAssessMethod);
                var result = await _unitOfWork.SaveAsync();

                if (result > 0)
                    return Response<int>.Deleted("Assess Method deleted successfully");

                return Response<int>.ServerError("Error occured while deleting Assess Method",
                    "An unexpected error occurred while deleting Assess Method. Please try again later.");
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "AssessMethodService",
                    MethodName = "DeleteAssessMethodAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<int>.ServerError("Error occured while deleting Assess Method",
                    "An unexpected error occurred while deleting Assess Method. Please try again later.");
            }
        }
    }
}
