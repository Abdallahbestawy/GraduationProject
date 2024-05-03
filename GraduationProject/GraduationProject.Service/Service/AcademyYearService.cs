using GraduationProject.Data.Entity;
using GraduationProject.Identity.IService;
using GraduationProject.LogHandler.Service;
using GraduationProject.Mails.IService;
using GraduationProject.Mails.Models;
using GraduationProject.Repository.IRepository;
using GraduationProject.Repository.Repository;
using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.AcademyYearDto;
using GraduationProject.Service.IService;
using System.Security.Claims;

namespace GraduationProject.Service.Service
{
    public class AcademyYearService : IAcademyYearService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMailService _mailService;
        private readonly LoggerHandler<AcademyYear> _logger;
        private readonly IAccountService _accountService;

        public AcademyYearService(UnitOfWork unitOfWork, IMailService mailService, LoggerHandler<AcademyYear> logger, IAccountService accountService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mailService = mailService;
            _logger = logger;
            _accountService = accountService;
        }

        public async Task<Response<int>> AddAcademyYearAsync(AcademyYearDto addAcademyYearDto, ClaimsPrincipal user)
        {
            try
            {
                var oldAcademyYear = await _unitOfWork.AcademyYears.GetEntityByPropertyAsync(c => c.IsCurrent && c.FacultyId == addAcademyYearDto.FacultyId);
                if (oldAcademyYear.Any())
                {
                    var academyYear = oldAcademyYear.FirstOrDefault();
                    academyYear.IsCurrent = false;
                    academyYear.End = DateTime.UtcNow;
                    await _unitOfWork.AcademyYears.Update(academyYear);
                }
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
                var result = await _unitOfWork.SaveAsync();
                if (result > 0)
                {
                    var userId = await _accountService.GetUserIdByUser(user);
                    await _logger.InsertLog(userId, "AcademyYear", newAcademyYear.Id.ToString(), null, newAcademyYear);
                    return Response<int>.Created("Academic year added successfully");
                }

                return Response<int>.ServerError("Error occured while adding Academic year",
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
                return Response<int>.ServerError("Error occured while adding Academic year",
                    "An unexpected error occurred while adding Academic year. Please try again later.");
            }
        }

        public async Task<Response<IQueryable<GetAcademyYearDto>>> GetAcademyYearAsync(int facultId)
        {
            try
            {
                var academyYearEntities = await _unitOfWork.AcademyYears.FindWithIncludeIEnumerableAsync(d => d.Facultys);

                if (academyYearEntities == null || !academyYearEntities.Any())
                    return Response<IQueryable<GetAcademyYearDto>>.NoContent("No Academic years are exist");
                var academyYearEntitiesFilter = academyYearEntities.Where(f => f.FacultyId == facultId).ToList();
                if (academyYearEntitiesFilter == null || !academyYearEntitiesFilter.Any())
                    return Response<IQueryable<GetAcademyYearDto>>.NoContent("No Academic years are exist");
                var academyYearDto = academyYearEntitiesFilter.Select(entity => new GetAcademyYearDto
                {
                    Id = entity.Id,
                    Start = entity.Start,
                    End = entity.End,
                    Description = entity.Description,
                    AcademyYearOrder = entity.AcademyYearOrder,
                    FacultyName = entity.Facultys.Name,
                    IsCurrent = entity.IsCurrent
                });

                return Response<IQueryable<GetAcademyYearDto>>.Success(academyYearDto.AsQueryable(), "Academic years retrieved successfully")
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
                return Response<IQueryable<GetAcademyYearDto>>.ServerError("Error occured while retrieving Academic years",
                    "An unexpected error occurred while retrieving Academic years. Please try again later.");
            }
        }

        public async Task<Response<AcademyYearDto>> GetAcademyYearByIdAsync(int academyYearId)
        {
            try
            {
                var academyYearEntity = await _unitOfWork.AcademyYears.GetByIdAsync(academyYearId);

                if (academyYearEntity == null)
                    return Response<AcademyYearDto>.BadRequest("This Academic year doesn't exist");

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
                return Response<AcademyYearDto>.ServerError("Error occured while retrieving Academic year",
                    "An unexpected error occurred while retrieving Academic year. Please try again later.");
            }
        }

        public async Task<Response<int>> UpdateAcademyYearAsync(AcademyYearDto updateAcademyYearDto, ClaimsPrincipal user)
        {
            try
            {
                AcademyYear existingAcademyYear = await _unitOfWork.AcademyYears.GetByIdAsync(updateAcademyYearDto.Id);

                if (existingAcademyYear == null)
                    return Response<int>.BadRequest("This Academic year doesn't exist");

                var oldAcademyYear = new AcademyYear
                {
                    Start = existingAcademyYear.Start,
                    End = existingAcademyYear.End,
                    Description = existingAcademyYear.Description,
                    AcademyYearOrder = existingAcademyYear.AcademyYearOrder,
                    FacultyId = existingAcademyYear.FacultyId,
                    IsCurrent = existingAcademyYear.IsCurrent
                };

                existingAcademyYear.Start = updateAcademyYearDto.Start;
                existingAcademyYear.End = updateAcademyYearDto.End;
                existingAcademyYear.Description = updateAcademyYearDto.Description;
                existingAcademyYear.AcademyYearOrder = updateAcademyYearDto.AcademyYearOrder;
                existingAcademyYear.FacultyId = updateAcademyYearDto.FacultyId;
                existingAcademyYear.IsCurrent = updateAcademyYearDto.IsCurrent;

                await _unitOfWork.AcademyYears.Update(existingAcademyYear);
                int result = await _unitOfWork.SaveAsync();

                if (result > 0)
                {
                    var userId = await _accountService.GetUserIdByUser(user);
                    await _logger.UpdateLog(userId, "AcademyYear", existingAcademyYear.Id.ToString(), oldAcademyYear, existingAcademyYear);
                    return Response<int>.Updated("Academic year updated successfully");
                }

                return Response<int>.ServerError("Error occured while updating Academic year",
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
                return Response<int>.ServerError("Error occured while updating Academic year",
                    "An unexpected error occurred while updating Academic year. Please try again later.");
            }
        }

        public async Task<Response<int>> DeleteAcademyYearAsync(int academyYearId, ClaimsPrincipal user)
        {
            try
            {
                var existingAcademyYear = await _unitOfWork.AcademyYears.GetByIdAsync(academyYearId);
                if (existingAcademyYear == null)
                    return Response<int>.BadRequest("This Academic year doesn't exist");
                await _unitOfWork.AcademyYears.Delete(existingAcademyYear);
                var result = await _unitOfWork.SaveAsync();

                var oldAcademyYear = new AcademyYear
                {
                    Start = existingAcademyYear.Start,
                    End = existingAcademyYear.End,
                    Description = existingAcademyYear.Description,
                    AcademyYearOrder = existingAcademyYear.AcademyYearOrder,
                    FacultyId = existingAcademyYear.FacultyId,
                    IsCurrent = existingAcademyYear.IsCurrent
                };

                if (result > 0)
                {
                    var userId = await _accountService.GetUserIdByUser(user);
                    await _logger.DeleteLog(userId, "AcademyYear", existingAcademyYear.Id.ToString(), oldAcademyYear, null);
                    return Response<int>.Deleted("Academic year deleted successfully");
                }

                return Response<int>.ServerError("Error occured while deleting Academic year",
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
                return Response<int>.ServerError("Error occured while updating Academic year",
                    "An unexpected error occurred while updating Academic year. Please try again later.");
            }
        }

        public async Task<Response<GetAcademyYearDto>> GetCurrentAcademyYearAsync(int facultId)
        {
            try
            {
                var result = await _unitOfWork.AcademyYears.FindWithIncludeIEnumerableAsync(d => d.Facultys);
                if (result == null || !result.Any())
                    return Response<GetAcademyYearDto>.BadRequest("This fuculty doesn't have academic years");

                var firstResult = result.Where(f => f.FacultyId == facultId && f.IsCurrent).FirstOrDefault();
                if (firstResult == null)
                    return Response<GetAcademyYearDto>.NoContent("there is no current academic year");

                GetAcademyYearDto academyYearDto = new GetAcademyYearDto
                {
                    Id = firstResult.Id,
                    Description = firstResult.Description,
                    Start = firstResult.Start,
                    End = firstResult.End,
                    AcademyYearOrder = firstResult.AcademyYearOrder,
                    FacultyName = firstResult.Facultys.Name,
                    IsCurrent = firstResult.IsCurrent,
                };
                return Response<GetAcademyYearDto>.Success(academyYearDto, "Academic year retrieved successfully").WithCount();
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "AcademyYearService",
                    MethodName = "GetCurrentAcademyYearAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<GetAcademyYearDto>.ServerError("Error occured while retrieving current Academic year",
                    "An unexpected error occurred while retrieving current Academic year. Please try again later.");
            }
        }

        public async Task<Response<List<GetAcademyYearNameDto>>> GetAcademyYearNameAsync()
        {
            try
            {
                var academyYearName = await _unitOfWork.AcademyYears.GetAll();
                if (academyYearName == null)
                    return Response<List<GetAcademyYearNameDto>>.NoContent("No Academic years are exist");

                List<GetAcademyYearNameDto> getAcademyYearNameDto = academyYearName
                    .Select(year => new GetAcademyYearNameDto
                    {
                        Id = year.Id,
                        AcademyYearName = $"{year.Start.Year}/{year.End.Year}"
                    })
                    .ToList();

                return Response<List<GetAcademyYearNameDto>>.Success(getAcademyYearNameDto, "Academic years retrieved successfully").WithCount();
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "AcademyYearService",
                    MethodName = "GetAcademyYearNameAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<List<GetAcademyYearNameDto>>.ServerError("Error occured while retrieving Academic years",
                    "An unexpected error occurred while retrieving Academic years. Please try again later.");
            }
        }

    }
}
