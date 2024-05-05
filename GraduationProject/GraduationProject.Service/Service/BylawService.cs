using GraduationProject.Data.Entity;
using GraduationProject.Mails.IService;
using GraduationProject.Mails.Models;
using GraduationProject.Repository.IRepository;
using GraduationProject.Repository.Repository;
using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.BylawDto;
using GraduationProject.Service.IService;

namespace GraduationProject.Service.Service
{
    public class BylawService : IBylawService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMailService _mailService;

        public BylawService(UnitOfWork unitOfWork, IMailService mailService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mailService = mailService;
        }

        public async Task<Response<int>> AddBylawAsync(BylawDto addBylawDto)
        {
            Bylaw newBylaw = new Bylaw
            {
                Name = addBylawDto.Name,
                Description = addBylawDto.Description,
                GraduateValuerRequired = addBylawDto.GraduateValuerRequired,
                Type = addBylawDto.Type,
                Start = addBylawDto.Start,
                End = addBylawDto.End,
                FacultyId = addBylawDto.FacultyId,
            };

            try
            {
                await _unitOfWork.Bylaws.AddAsync(newBylaw);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "BylawService",
                    MethodName = "AddBylawAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<int>.ServerError("Error occured while adding Bylaw",
                     "An unexpected error occurred while adding Bylaw. Please try again later.");
            }

            int bylawyId = newBylaw.Id;

            List<Estimates> estimates = addBylawDto.Estimates.Select(est =>
                new Estimates
                {
                    BylawId = bylawyId,
                    Name = est.NameEstimates,
                    Char = est.CharEstimates,
                    MaxGpa = est.MaxGpaEstimates,
                    MinGpa = est.MinGpaEstimates,
                    MaxPercentage = est.MaxPercentageEstimates,
                    MinPercentage = est.MinPercentageEstimates,
                }).ToList();

            try
            {
                await _unitOfWork.Estimates.AddRangeAsync(estimates);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "BylawService",
                    MethodName = "AddBylawAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                await _unitOfWork.Bylaws.Delete(newBylaw);
                return Response<int>.ServerError("Error occured while adding Bylaw",
                     "An unexpected error occurred while adding Bylaw. Please try again later.");
            }

            List<EstimatesCourse> estimatesCourses = addBylawDto.EstimatesCourses.Select(estCourse =>
                new EstimatesCourse
                {
                    BylawId = bylawyId,
                    Name = estCourse.NameEstimatesCourse,
                    Char = estCourse.CharEstimatesCourse,
                    MaxPercentage = estCourse.MaxPercentageEstimatesCourse,
                    MinPercentage = estCourse.MinPercentageEstimatesCourse
                }).ToList();

            try
            {
                await _unitOfWork.EstimatesCourses.AddRangeAsync(estimatesCourses);
                var result = await _unitOfWork.SaveAsync();
                if (result > 0)
                    return Response<int>.Created("Bylaw added successfully");

                return Response<int>.ServerError("Error occured while adding Bylaw",
                     "An unexpected error occurred while adding Bylaw. Please try again later.");
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "BylawService",
                    MethodName = "AddBylawAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                //add deleteRangge function //bastawy
                //await _unitOfWork.Estimates.DeleteRange(estimates);
                await _unitOfWork.Bylaws.Delete(newBylaw);
                return Response<int>.ServerError("Error occured while adding Bylaw",
                     "An unexpected error occurred while adding Bylaw. Please try again later.");
            }
        }

        public async Task<Response<IQueryable<BylawDto>>> GetBylawAsync()
        {
            try
            {
                var bylawEntities = await _unitOfWork.Bylaws.GetAll();

                if (!bylawEntities.Any())
                    return Response<IQueryable<BylawDto>>.NoContent("No Bylaws are exist");

                var bylawDtos = bylawEntities.Select(entity => new BylawDto
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    GraduateValuerRequired = entity.GraduateValuerRequired,
                    Description = entity.Description,
                    Start = entity.Start,
                    End = entity.End,
                    FacultyId = entity.FacultyId
                });

                return Response<IQueryable<BylawDto>>.Success(bylawDtos.AsQueryable(), "Bylaws retrieved successfully").WithCount();
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "BylawService",
                    MethodName = "GetBylawAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<IQueryable<BylawDto>>.ServerError("Error occured while retrieving bylaws",
                    "An unexpected error occurred while retrieving bylaws. Please try again later.");
            }
        }

        public async Task<Response<BylawDto>> GetBylawByIdAsync(int BylawId)
        {
            try
            {
                var bylawEntity = await _unitOfWork.Bylaws.GetByIdAsync(BylawId);

                if (bylawEntity == null)
                    return Response<BylawDto>.BadRequest("This bylaw doesn't exist");

                BylawDto bylawDto = new BylawDto
                {
                    Id = bylawEntity.Id,
                    Name = bylawEntity.Name,
                    Description = bylawEntity.Description,
                    GraduateValuerRequired = bylawEntity.GraduateValuerRequired,
                    Type = bylawEntity.Type,
                    Start = bylawEntity.Start,
                    End = bylawEntity.End,
                    FacultyId = bylawEntity.FacultyId
                };

                return Response<BylawDto>.Success(bylawDto, "Bylaw retrieved successfully").WithCount();
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "BylawService",
                    MethodName = "GetBylawByIdAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<BylawDto>.ServerError("Error occured while retrieving bylaw",
                    "An unexpected error occurred while retrieving bylaw. Please try again later.");
            }
        }

        public async Task<Response<int>> UpdateBylawAsync(BylawDto updateBylawDto)
        {
            try
            {
                Bylaw existingBylaw = await _unitOfWork.Bylaws.GetByIdAsync(updateBylawDto.Id);

                if (existingBylaw == null)
                    return Response<int>.BadRequest("This bylaw doesn't exist");

                existingBylaw.Name = updateBylawDto.Name;
                existingBylaw.Description = updateBylawDto.Description;
                existingBylaw.Type = updateBylawDto.Type;
                existingBylaw.GraduateValuerRequired = updateBylawDto.GraduateValuerRequired;
                existingBylaw.Start = updateBylawDto.Start;
                existingBylaw.End = updateBylawDto.End;
                existingBylaw.FacultyId = updateBylawDto.FacultyId;

                await _unitOfWork.Bylaws.Update(existingBylaw);
                var result = await _unitOfWork.SaveAsync();

                if (result > 0)
                    return Response<int>.Updated("Bylaw updated successfully");

                return Response<int>.ServerError("Error occured while updating bylaw",
                        "An unexpected error occurred while updating bylaw. Please try again later.");
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "BylawService",
                    MethodName = "UpdateBylawAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<int>.ServerError("Error occured while updating bylaw",
                        "An unexpected error occurred while updating bylaw. Please try again later.");
            }
        }

        public async Task<Response<int>> DeleteBylawAsync(int BylawId)
        {
            try
            {
                var existingBlaw = await _unitOfWork.Bylaws.GetByIdAsync(BylawId);

                if (existingBlaw == null)
                    return Response<int>.BadRequest("This bylaw doesn't exist");

                await _unitOfWork.Bylaws.Delete(existingBlaw);
                var result = await _unitOfWork.SaveAsync();

                if (result > 0)
                    return Response<int>.Deleted("Bylaw deleted successfully");

                return Response<int>.ServerError("Error occured while deleting bylaw",
                        "An unexpected error occurred while deleting bylaw. Please try again later.");
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "BylawService",
                    MethodName = "DeleteBylawAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<int>.ServerError("Error occured while deleting bylaw",
                        "An unexpected error occurred while deleting bylaw. Please try again later.");
            }
        }

        public async Task<Response<IQueryable<BylawDto>>> GetBylawByFacultyIdAsync(int facultyId)
        {
            try
            {
                var bylawEntities = await _unitOfWork.Bylaws.GetEntityByPropertyAsync(bylaw => bylaw.FacultyId == facultyId);

                if (!bylawEntities.Any())
                    return Response<IQueryable<BylawDto>>.NoContent("No Bylaws are exist");

                var bylawDtos = bylawEntities.Select(entity => new BylawDto
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Description = entity.Description,
                    GraduateValuerRequired = entity.GraduateValuerRequired,
                    Start = entity.Start,
                    End = entity.End,
                    FacultyId = entity.FacultyId
                });

                return Response<IQueryable<BylawDto>>.Success(bylawDtos.AsQueryable(), "Bylaws retrieved successfully").WithCount();
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "BylawService",
                    MethodName = "GetBylawByFacultyIdAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<IQueryable<BylawDto>>.ServerError("Error occured while retrieving bylaws",
                    "An unexpected error occurred while retrieving bylaws. Please try again later.");
            }
        }
    }
}
