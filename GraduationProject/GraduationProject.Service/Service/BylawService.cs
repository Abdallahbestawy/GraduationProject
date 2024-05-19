using GraduationProject.Data.Entity;
using GraduationProject.Data.Enum;
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
                var bylawEntity = await _unitOfWork.Bylaws.GetEntityByPropertyWithIncludeAsync(
                    b => b.Id == BylawId,
                    e => e.Estimatess,
                    e => e.EstimatesCourses
                    );
                if (!bylawEntity.Any())
                    return Response<BylawDto>.BadRequest("This bylaw doesn't exist");

                BylawDto bylawDto = new BylawDto
                {
                    Id = bylawEntity.FirstOrDefault().Id,
                    Name = bylawEntity.FirstOrDefault().Name,
                    Description = bylawEntity.FirstOrDefault().Description,
                    GraduateValuerRequired = bylawEntity.FirstOrDefault().GraduateValuerRequired,
                    Type = bylawEntity.FirstOrDefault().Type,
                    Start = bylawEntity.FirstOrDefault().Start,
                    End = bylawEntity.FirstOrDefault().End,
                    FacultyId = bylawEntity.FirstOrDefault().FacultyId,
                };
                if (bylawEntity.Any(s => s.Estimatess != null && s.Estimatess.Any()))
                {
                    bylawDto.Estimates = bylawEntity
                        .Where(s => s.Estimatess != null && s.Estimatess.Any())
                        .SelectMany(s => s.Estimatess.Select(e => new EstimateDto
                        {
                            Id = e.Id,
                            NameEstimates = e.Name,
                            CharEstimates = e.Char,
                            MaxGpaEstimates = e.MaxGpa,
                            MinGpaEstimates = e.MinGpa,
                            MaxPercentageEstimates = e.MaxPercentage,
                            MinPercentageEstimates = e.MinPercentage
                        })).ToList();
                }
                if (bylawEntity.Any(s => s.EstimatesCourses != null && s.EstimatesCourses.Any()))
                {
                    bylawDto.EstimatesCourses = bylawEntity
                        .Where(s => s.EstimatesCourses != null && s.EstimatesCourses.Any())
                        .SelectMany(s => s.EstimatesCourses.Select(e => new EstimateCourseDto
                        {
                            Id = e.Id,
                            NameEstimatesCourse = e.Name,
                            CharEstimatesCourse = e.Char,
                            MaxPercentageEstimatesCourse = e.MaxPercentage,
                            MinPercentageEstimatesCourse = e.MinPercentage
                        })).ToList();
                }

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

        public async Task<Response<IQueryable<GetBylawDto>>> GetBylawByFacultyIdAsync(int facultyId)
        {
            try
            {
                var bylawEntities = await _unitOfWork.Bylaws.GetEntityByPropertyWithIncludeAsync(bylaw => bylaw.FacultyId == facultyId, f => f.Faculty);

                if (!bylawEntities.Any())
                    return Response<IQueryable<GetBylawDto>>.NoContent("No Bylaws are exist");

                var bylawDtos = bylawEntities.Select(entity => new GetBylawDto
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Description = entity.Description,
                    Type = Enum.GetName(typeof(CourseCategory), entity.Type),
                    GraduateValuerRequired = entity.GraduateValuerRequired,
                    Start = entity.Start,
                    End = entity.End,
                    FacultyName = entity.Faculty.Name
                });

                return Response<IQueryable<GetBylawDto>>.Success(bylawDtos.AsQueryable(), "Bylaws retrieved successfully").WithCount();
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
                return Response<IQueryable<GetBylawDto>>.ServerError("Error occured while retrieving bylaws",
                    "An unexpected error occurred while retrieving bylaws. Please try again later.");
            }
        }

        public async Task<Response<int>> DeleteEstimatesAsync(int estimatesId)
        {
            try
            {
                var existingEstimates = await _unitOfWork.Estimates.GetByIdAsync(estimatesId);
                if (existingEstimates == null)
                    return Response<int>.BadRequest("This Estimates doesn't exist");

                await _unitOfWork.Estimates.Delete(existingEstimates);
                var result = await _unitOfWork.SaveAsync();

                if (result > 0)
                    return Response<int>.Deleted("Estimates deleted successfully");

                return Response<int>.ServerError("Error occured while deleting Estimates",
                        "An unexpected error occurred while deleting Estimates. Please try again later.");
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "BylawService",
                    MethodName = "DeleteEstimatesAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<int>.ServerError("Error occured while deleting Estimates",
                        "An unexpected error occurred while deleting Estimates. Please try again later.");
            }
        }

        public async Task<Response<int>> DeleteEstimatesCourseAsync(int estimatesCourseId)
        {
            try
            {
                var existingEstimatesCourse = await _unitOfWork.EstimatesCourses.GetByIdAsync(estimatesCourseId);
                if (existingEstimatesCourse == null)
                    return Response<int>.BadRequest("This EstimatesCourse doesn't exist");

                await _unitOfWork.EstimatesCourses.Delete(existingEstimatesCourse);
                var result = await _unitOfWork.SaveAsync();

                if (result > 0)
                    return Response<int>.Deleted("EstimatesCourse deleted successfully");

                return Response<int>.ServerError("Error occured while deleting EstimatesCourse",
                        "An unexpected error occurred while deleting EstimatesCourse. Please try again later.");
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "BylawService",
                    MethodName = "DeleteEstimatesCourseAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<int>.ServerError("Error occured while deleting EstimatesCourse",
                        "An unexpected error occurred while deleting EstimatesCourse. Please try again later.");
            }
        }
    }
}
