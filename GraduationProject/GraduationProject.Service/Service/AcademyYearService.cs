using GraduationProject.Data.Entity;
using GraduationProject.Identity.Models;
using GraduationProject.Mails.IService;
using GraduationProject.Mails.Models;
using GraduationProject.Repository.IRepository;
using GraduationProject.Repository.Repository;
using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.AcademyYearDto;
using GraduationProject.Service.IService;

namespace GraduationProject.Service.Service
{
    public class AcademyYearService : IAcademyYearService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMailService _mailService;
        public AcademyYearService(UnitOfWork unitOfWork, IMailService mailService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mailService = mailService;
        }
        public async Task<Response<int>> AddAcademyYearAsync(AcademyYearDto addAcademyYearDto)
        {
            try
            {
                AcademyYear newAcademyYear = new AcademyYear
                {
                    Start = addAcademyYearDto.Start,
                    End = addAcademyYearDto.End,
                    Description = addAcademyYearDto.Description,
                    AcademyYearOrder = addAcademyYearDto.AcademyYearOrder,
                    FacultyId = addAcademyYearDto.FacultyId,
                    IsCurrent = addAcademyYearDto.IsCurrent
                };
                await _unitOfWork.AcademyYears.AddAsync(newAcademyYear);
                var result  = await _unitOfWork.SaveAsync();
                if(result > 0)
                    return Response<int>.Created( "Academic year added successfully");

                return Response<int>
                    .ServerError("Error occured while adding Academic year",
                    "An unexpected error occurred while adding Academic year. Please try again later.");
            } 
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "AcademyYearService",
                    MethodName = "AddAcademyYearAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<int>
                    .ServerError("Error occured while adding Academic year",
                    "An unexpected error occurred while adding Academic year. Please try again later.");
            }
        }

        public async Task<Response<IQueryable<AcademyYearDto>>> GetAcademyYearAsync()
        {
            try
            {
                var academyYearEntities = await _unitOfWork.AcademyYears.GetAll();
                if (!academyYearEntities.Any())
                    return Response<IQueryable<AcademyYearDto>>.NoContent("No Academic years is exist");

                var academyYearDto = academyYearEntities.Select(entity => new AcademyYearDto
                {
                    Id = entity.Id,
                    Start = entity.Start,
                    End = entity.End,
                    Description = entity.Description,
                    AcademyYearOrder = entity.AcademyYearOrder,
                    FacultyId = entity.FacultyId,
                    IsCurrent = entity.IsCurrent
                });

                return Response<IQueryable<AcademyYearDto>>.Success(academyYearDto.AsQueryable(), "Academic years retrieved successfully")
                    .WithCount();
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "AcademyYearService",
                    MethodName = "GetAcademyYearAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<IQueryable<AcademyYearDto>>
                    .ServerError("Error occured while retrieving Academic years",
                    "An unexpected error occurred while retrieving Academic years. Please try again later.");
            }
        }

        public async Task<Response<AcademyYearDto>> GetAcademyYearByIdAsync(int academyYearId)
        {
            try
            {
                var academyYearEntity = await _unitOfWork.AcademyYears.GetByIdAsync(academyYearId);
                if (academyYearEntity == null)
                    return Response<AcademyYearDto>.BadRequest("This Academic year doesn't exists");
                AcademyYearDto academyYearDto = new AcademyYearDto
                {
                    Id = academyYearEntity.Id,
                    Start = academyYearEntity.Start,
                    End = academyYearEntity.End,
                    Description = academyYearEntity.Description,
                    AcademyYearOrder = academyYearEntity.AcademyYearOrder,
                    FacultyId = academyYearEntity.FacultyId,
                    IsCurrent = academyYearEntity.IsCurrent
                };
                return Response<AcademyYearDto>.Success(academyYearDto, "Academic year retrieved successfully").WithCount();
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "AcademyYearService",
                    MethodName = "GetAcademyYearByIdAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<AcademyYearDto>
                    .ServerError("Error occured while retrieving Academic year",
                    "An unexpected error occurred while retrieving Academic year. Please try again later.");
            }
        }

        public async Task<Response<int>> UpdateAcademyYearAsync(AcademyYearDto updateAcademyYearDto)
        {
            try
            {
                AcademyYear existingAcademyYear = await _unitOfWork.AcademyYears.GetByIdAsync(updateAcademyYearDto.Id);
                if (existingAcademyYear == null)
                    return Response<int>.BadRequest("This Academic year doesn't exists");
                existingAcademyYear.Start = updateAcademyYearDto.Start;
                existingAcademyYear.End = updateAcademyYearDto.End;
                existingAcademyYear.Description = updateAcademyYearDto.Description;
                existingAcademyYear.AcademyYearOrder = updateAcademyYearDto.AcademyYearOrder;
                existingAcademyYear.FacultyId = updateAcademyYearDto.FacultyId;
                existingAcademyYear.IsCurrent = updateAcademyYearDto.IsCurrent;

                await _unitOfWork.AcademyYears.Update(existingAcademyYear);
                int result = await _unitOfWork.SaveAsync();

                if (result > 0)
                    return Response<int>.Updated("Academic year updated successfully");

                return Response<int>
                    .ServerError("Error occured while updating Academic year",
                    "An unexpected error occurred while updating Academic year. Please try again later.");

            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "AcademyYearService",
                    MethodName = "UpdateAcademyYearAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<int>
                    .ServerError("Error occured while updating Academic year",
                    "An unexpected error occurred while updating Academic year. Please try again later.");
            }
        }
        public async Task<Response<int>> DeleteAcademyYearAsync(int academyYearId)
        {
            try
            {
                var existingAcademyYear = await _unitOfWork.AcademyYears.GetByIdAsync(academyYearId);
                if (existingAcademyYear == null)
                    return Response<int>.BadRequest("This Academic year doesn't exists");
                await _unitOfWork.AcademyYears.Delete(existingAcademyYear);
                var result = await _unitOfWork.SaveAsync();

                if (result > 0)
                    return Response<int>.Deleted("Academic year deleted successfully");

                return Response<int>
                    .ServerError("Error occured while deleting Academic year",
                    "An unexpected error occurred while deleting Academic year. Please try again later.");
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "AcademyYearService",
                    MethodName = "DeleteAcademyYearAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<int>
                    .ServerError("Error occured while updating Academic year",
                    "An unexpected error occurred while updating Academic year. Please try again later.");
            }
        }
    }
}
