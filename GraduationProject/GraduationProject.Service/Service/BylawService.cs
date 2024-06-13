using GraduationProject.Data.Entity;
using GraduationProject.Data.Enum;
using GraduationProject.Identity.IService;
using GraduationProject.Identity.Models;
using GraduationProject.LogHandler.IService;
using GraduationProject.LogHandler.Service;
using GraduationProject.Mails.IService;
using GraduationProject.Mails.Models;
using GraduationProject.Repository.IRepository;
using GraduationProject.Repository.Repository;
using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.BylawDto;
using GraduationProject.Service.IService;
using System.Security.Claims;

namespace GraduationProject.Service.Service
{
    public class BylawService : IBylawService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMailService _mailService;
        private readonly ILoggerHandler _loggerHandler; //not finshed
        private readonly IAccountService _accountService;

        public BylawService(UnitOfWork unitOfWork, IMailService mailService, ILoggerHandler loggerHandler, IAccountService accountService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mailService = mailService;
            _loggerHandler = loggerHandler;
            _accountService = accountService;
        }

        public async Task<Response<int>> AddBylawAsync(BylawDto addBylawDto, ClaimsPrincipal user)
        {
            ApplicationUser? userData = null;
            try
            {
                userData = await _accountService.GetUser(user);
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
                await _loggerHandler.InsertLog(userData.Id, "Bylaws", newBylaw.Id.ToString(), null, newBylaw, typeof(Bylaw));
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
                foreach (var estimate in estimates)
                {
                    await _loggerHandler.InsertLog(userData.Id, "Estimates", estimate.Id.ToString(), null, estimate, 
                        typeof(Estimates));
                }
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
                {
                    foreach (var estimate in estimatesCourses)
                    {
                        await _loggerHandler.InsertLog(userData.Id, "EstimatesCourses", estimate.Id.ToString(), null, estimate,
                            typeof(EstimatesCourse));
                    }
                    return Response<int>.Created("Bylaw added successfully");
                }

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

        public async Task<Response<int>> UpdateBylawAsync(BylawDto updateBylawDto, ClaimsPrincipal user)
        {
            try
            {
                bool estimatesIsUpdated = false;
                bool estimatesCoursesIsUpdated = false;

                var userData = await _accountService.GetUser(user);

                Bylaw existingBylaw = await _unitOfWork.Bylaws.GetByIdAsync(updateBylawDto.Id);

                if (existingBylaw == null)
                    return Response<int>.BadRequest("This bylaw doesn't exist");

                var oldBylaw = ObjectDuplicater.Duplicate(existingBylaw);

                existingBylaw.Name = updateBylawDto.Name;
                existingBylaw.Description = updateBylawDto.Description;
                existingBylaw.Type = updateBylawDto.Type;
                existingBylaw.GraduateValuerRequired = updateBylawDto.GraduateValuerRequired;
                existingBylaw.Start = updateBylawDto.Start;
                existingBylaw.End = updateBylawDto.End;
                existingBylaw.FacultyId = updateBylawDto.FacultyId;

                await _unitOfWork.Bylaws.Update(existingBylaw);
                var existingEstimates = await _unitOfWork.Estimates.GetEntityByPropertyAsync(s => s.BylawId == existingBylaw.Id);
                if (existingEstimates.Any())
                {
                    foreach (var existingEstimate in existingEstimates)
                    {
                        var updateDtoEstimate = updateBylawDto.Estimates.FirstOrDefault(es => es.Id == existingEstimate.Id);
                        if (updateDtoEstimate != null)
                        {
                            existingEstimate.Name = updateDtoEstimate.NameEstimates;
                            existingEstimate.Char = updateDtoEstimate.CharEstimates;
                            existingEstimate.MaxPercentage = updateDtoEstimate.MaxPercentageEstimates;
                            existingEstimate.MinPercentage = updateDtoEstimate.MinPercentageEstimates;
                            existingEstimate.MaxGpa = updateDtoEstimate.MaxGpaEstimates;
                            existingEstimate.MinGpa = updateDtoEstimate.MinGpaEstimates;
                        }
                    }
                    await _unitOfWork.Estimates.UpdateRangeAsync(existingEstimates);
                }
                else
                {
                    if (updateBylawDto.Estimates != null)
                    {
                        List<Estimates> estimates = updateBylawDto.Estimates.Select(es =>
                            new Estimates
                            {
                                BylawId = existingBylaw.Id,
                                Name = es.NameEstimates,
                                Char = es.CharEstimates,
                                MaxPercentage = es.MaxPercentageEstimates,
                                MinPercentage = es.MinPercentageEstimates,
                                MaxGpa = es.MaxGpaEstimates,
                                MinGpa = es.MinGpaEstimates
                            }).ToList();

                        await _unitOfWork.Estimates.AddRangeAsync(estimates);
                    }
                }
                var existingEstimatesCourses = await _unitOfWork.EstimatesCourses.GetEntityByPropertyAsync(s => s.BylawId == existingBylaw.Id);
                if (existingEstimatesCourses.Any())
                {
                    foreach (var existingEstimatesCourse in existingEstimatesCourses)
                    {
                        var updateDtoEstimateCourse = updateBylawDto.EstimatesCourses.FirstOrDefault(esc => esc.Id == existingEstimatesCourse.Id);
                        if (updateDtoEstimateCourse != null)
                        {
                            existingEstimatesCourse.Name = updateDtoEstimateCourse.NameEstimatesCourse;
                            existingEstimatesCourse.Char = updateDtoEstimateCourse.CharEstimatesCourse;
                            existingEstimatesCourse.MaxPercentage = updateDtoEstimateCourse.MaxPercentageEstimatesCourse;
                            existingEstimatesCourse.MinPercentage = updateDtoEstimateCourse.MinPercentageEstimatesCourse;

                        }
                    }
                    await _unitOfWork.EstimatesCourses.UpdateRangeAsync(existingEstimatesCourses);
                }
                else
                {
                    if (updateBylawDto.EstimatesCourses != null)
                    {
                        List<EstimatesCourse> estimatesCourse = updateBylawDto.EstimatesCourses.Select(esc =>
                            new EstimatesCourse
                            {
                                BylawId = existingBylaw.Id,
                                Name = esc.NameEstimatesCourse,
                                Char = esc.CharEstimatesCourse,
                                MaxPercentage = esc.MaxPercentageEstimatesCourse,
                                MinPercentage = esc.MinPercentageEstimatesCourse,
                            }).ToList();

                        await _unitOfWork.EstimatesCourses.AddRangeAsync(estimatesCourse);
                    }
                }
                var result = await _unitOfWork.SaveAsync();

                if (result > 0)
                {
                    await _loggerHandler.UpdateLog(userData.Id, "Bylaws", existingBylaw.Id.ToString(), oldBylaw, existingBylaw,
                        typeof(Bylaw));
                    return Response<int>.Updated("Bylaw updated successfully");
                }

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

        public async Task<Response<int>> DeleteBylawAsync(int BylawId, ClaimsPrincipal user)
        {
            try
            {
                var userData = await _accountService.GetUser(user);

                var existingBlaw = await _unitOfWork.Bylaws.GetByIdAsync(BylawId);

                var oldBylaw = ObjectDuplicater.Duplicate(existingBlaw);

                if (existingBlaw == null)
                    return Response<int>.BadRequest("This bylaw doesn't exist");

                await _unitOfWork.Bylaws.Delete(existingBlaw);
                var result = await _unitOfWork.SaveAsync();

                if (result > 0)
                {
                    await _loggerHandler.DeleteLog(userData.Id, "Bylaws", oldBylaw.Id.ToString(), oldBylaw, null, typeof(Bylaw));
                    return Response<int>.Deleted("Bylaw deleted successfully");
                }

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

        public async Task<Response<int>> DeleteEstimatesAsync(int estimatesId, ClaimsPrincipal user)
        {
            try
            {
                var userData = await _accountService.GetUser(user);

                var existingEstimates = await _unitOfWork.Estimates.GetByIdAsync(estimatesId);
                if (existingEstimates == null)
                    return Response<int>.BadRequest("This Estimates doesn't exist");
                var oldEstimate = ObjectDuplicater.Duplicate(existingEstimates);

                await _unitOfWork.Estimates.Delete(existingEstimates);
                var result = await _unitOfWork.SaveAsync();

                if (result > 0)
                {
                    await _loggerHandler.DeleteLog(userData.Id, "Estimates", oldEstimate.Id.ToString(), oldEstimate, null,
                        typeof(Estimates));
                    return Response<int>.Deleted("Estimates deleted successfully");
                }

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

        public async Task<Response<int>> DeleteEstimatesCourseAsync(int estimatesCourseId, ClaimsPrincipal user)
        {
            try
            {
                var userData = await _accountService.GetUser(user);

                var existingEstimatesCourse = await _unitOfWork.EstimatesCourses.GetByIdAsync(estimatesCourseId);
                if (existingEstimatesCourse == null)
                    return Response<int>.BadRequest("This EstimatesCourse doesn't exist");

                var oldEstimateCourse = ObjectDuplicater.Duplicate(existingEstimatesCourse);

                await _unitOfWork.EstimatesCourses.Delete(existingEstimatesCourse);
                var result = await _unitOfWork.SaveAsync();

                if (result > 0)
                {
                    await _loggerHandler.DeleteLog(userData.Id, "EstimatesCourses", oldEstimateCourse.Id.ToString(),
                        oldEstimateCourse, null, typeof(EstimatesCourse));
                    return Response<int>.Deleted("EstimatesCourse deleted successfully");
                }

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
