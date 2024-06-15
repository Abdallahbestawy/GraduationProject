using GraduationProject.Data.Entity;
using GraduationProject.Mails.IService;
using GraduationProject.Mails.Models;
using GraduationProject.Repository.IRepository;
using GraduationProject.Repository.Repository;
using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.SchedulePlaceDto;
using GraduationProject.Service.IService;

namespace GraduationProject.Service.Service
{
    public class SchedulePlaceService : ISchedulePlaceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMailService _mailService;

        public SchedulePlaceService(UnitOfWork unitOfWork, IMailService mailService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mailService = mailService;
        }
        public async Task<Response<int>> AddSchedulePlaceAsync(SchedulePlaceDto addSchedulePlaceDto)
        {
            try
            {
                SchedulePlace newSchedulePlace = new SchedulePlace
                {
                    Name = addSchedulePlaceDto.Name,
                    PlaceCapacity = addSchedulePlaceDto.PlaceCapacity,
                    FacultyId = addSchedulePlaceDto.FacultyId
                };
                await _unitOfWork.SchedulePlaces.AddAsync(newSchedulePlace);
                var result = await _unitOfWork.SaveAsync();

                if (result > 0)
                {
                    return Response<int>.Created("Schedule Place added successfully");
                }

                return Response<int>.ServerError("Error occured while adding Schedule Place",
                    "An unexpected error occurred while adding Schedule Place. Please try again later.");
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = nameof(SchedulePlaceService),
                    MethodName = nameof(AddSchedulePlaceAsync),
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<int>.ServerError("Error occured while adding Schedule Place",
                      "An unexpected error occurred while adding Schedule Place. Please try again later.");
            }
        }
        public async Task<Response<IQueryable<GetSchedulePlaceDto>>> GetSchedulePlaceByFacultyIdAsync(int facultyId)
        {
            try
            {
                var schedulePlaceEntities = await _unitOfWork.SchedulePlaces.GetEntityByPropertyWithIncludeAsync(f => f.FacultyId == facultyId, d => d.Faculty);

                if (!schedulePlaceEntities.Any())
                {
                    return Response<IQueryable<GetSchedulePlaceDto>>.NoContent("No Schedule Place are exist");
                }

                var schedulePlaceDto = schedulePlaceEntities.Select(entity => new GetSchedulePlaceDto
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    PlaceCapacity = entity.PlaceCapacity,
                    FacultyName = entity.Faculty.Name
                });

                return Response<IQueryable<GetSchedulePlaceDto>>.Success(schedulePlaceDto.AsQueryable(), "Schedule Place retrieved successfully").WithCount();
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = nameof(SchedulePlaceService),
                    MethodName = nameof(GetSchedulePlaceByFacultyIdAsync),
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<IQueryable<GetSchedulePlaceDto>>.ServerError("Error occured while retrieving Schedule Place",
                    "An unexpected error occurred while retrieving Schedule Place. Please try again later.");
            }
        }

        public async Task<Response<SchedulePlaceDto>> GetSchedulePlaceByIdAsync(int schedulePlaceId)
        {
            try
            {
                var schedulePlaceEntity = await _unitOfWork.SchedulePlaces.GetByIdAsync(schedulePlaceId);

                if (schedulePlaceEntity == null)
                {
                    return Response<SchedulePlaceDto>.BadRequest("This Schedule Place doesn't exist");
                }

                SchedulePlaceDto schedulePlaceDto = new SchedulePlaceDto
                {
                    Id = schedulePlaceEntity.Id,
                    Name = schedulePlaceEntity.Name,
                    PlaceCapacity = schedulePlaceEntity.PlaceCapacity,
                    FacultyId = schedulePlaceEntity.FacultyId
                };

                return Response<SchedulePlaceDto>.Success(schedulePlaceDto, "Schedule Place retrieved successfully").WithCount();
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = nameof(SchedulePlaceService),
                    MethodName = nameof(GetSchedulePlaceByIdAsync),
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<SchedulePlaceDto>.ServerError("Error occured while retrieving Schedule Place",
                    "An unexpected error occurred while retrieving Schedule Place. Please try again later.");
            }
        }

        public async Task<Response<int>> UpdateSchedulePlaceAsync(SchedulePlaceDto updateSchedulePlaceDto)
        {
            try
            {
                SchedulePlace existingSchedulePlace = await _unitOfWork.SchedulePlaces.GetByIdAsync(updateSchedulePlaceDto.Id);
                if (existingSchedulePlace == null)
                {
                    return Response<int>.BadRequest("This Schedule Place doesn't exist");
                }
                existingSchedulePlace.Name = updateSchedulePlaceDto.Name;
                existingSchedulePlace.PlaceCapacity = updateSchedulePlaceDto.PlaceCapacity;
                existingSchedulePlace.FacultyId = updateSchedulePlaceDto.FacultyId;

                await _unitOfWork.SchedulePlaces.Update(existingSchedulePlace);
                var result = await _unitOfWork.SaveAsync();

                if (result > 0)
                {
                    return Response<int>.Updated("Schedule Place updated successfully");
                }

                return Response<int>.ServerError("Error occured while updating Schedule Place",
                    "An unexpected error occurred while updating Schedule Place. Please try again later.");
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = nameof(SchedulePlaceService),
                    MethodName = nameof(UpdateSchedulePlaceAsync),
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<int>.ServerError("Error occured while updating Schedule Place",
                    "An unexpected error occurred while updating Schedule Place. Please try again later.");
            }
        }
        public async Task<Response<int>> DeleteSchedulePlaceAsync(int schedulePlaceId)
        {
            try
            {
                var existingSchedulePlace = await _unitOfWork.SchedulePlaces.GetByIdAsync(schedulePlaceId);

                if (existingSchedulePlace == null)
                {
                    return Response<int>.BadRequest("This Schedule Place doesn't exist");
                }

                await _unitOfWork.SchedulePlaces.Delete(existingSchedulePlace);
                var result = await _unitOfWork.SaveAsync();

                if (result > 0)
                {
                    return Response<int>.Deleted("Schedule Place deleted successfully");
                }

                return Response<int>.ServerError("Error occured while deleting Schedule Place",
                    "An unexpected error occurred while deleting Schedule Place. Please try again later.");
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = nameof(SchedulePlaceService),
                    MethodName = nameof(DeleteSchedulePlaceAsync),
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<int>.ServerError("Error occured while deleting Schedule Place",
                    "An unexpected error occurred while deleting Schedule Place. Please try again later.");
            }
        }
    }
}
