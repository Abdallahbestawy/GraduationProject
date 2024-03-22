using GraduationProject.Data.Entity;
using GraduationProject.Mails.IService;
using GraduationProject.Mails.Models;
using GraduationProject.Repository.IRepository;
using GraduationProject.Repository.Repository;
using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.ScientificDegreeDto;
using GraduationProject.Service.IService;

namespace GraduationProject.Service.Service
{
    public class ScientificDegreeService : IScientificDegreeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMailService _mailService;

        public ScientificDegreeService(UnitOfWork unitOfWork, IMailService mailService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mailService = mailService;
        }

        public async Task<Response<int>> AddScientificDegreeAsync(ScientificDegreeDto addScientificDegreeDto)
        {
            try
            {
                ScientificDegree newScientificDegree = new ScientificDegree
                {
                    Name = addScientificDegreeDto.Name,
                    Description = addScientificDegreeDto.Description,
                    Type = addScientificDegreeDto.Type,
                    BylawId = addScientificDegreeDto.BylawId,
                    SuccessPercentageCourse = addScientificDegreeDto.SuccessPercentageCourse,
                    SuccessPercentageBand = addScientificDegreeDto.SuccessPercentageBand,
                    SuccessPercentageSemester = addScientificDegreeDto.SuccessPercentageSemester,
                    SuccessPercentagePhase = addScientificDegreeDto.SuccessPercentagePhase,
                    PhaseId = addScientificDegreeDto.PhaseId,
                    BandId = addScientificDegreeDto.BandId,
                    SemesterId = addScientificDegreeDto.SemesterId,
                    ExamRoleId = addScientificDegreeDto.ExamRoleId,
                    ParentId = addScientificDegreeDto.ParentId
                };
                await _unitOfWork.ScientificDegrees.AddAsync(newScientificDegree);
                var result = await _unitOfWork.SaveAsync();

                if (result > 0)
                    return Response<int>.Created("Scientific Degree added successfully");

                return Response<int>.ServerError("Error occured while adding Scientific Degree",
                    "An unexpected error occurred while adding Scientific Degree. Please try again later.");
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "ScientificDegreeService",
                    MethodName = "AddScientificDegreeAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<int>.ServerError("Error occured while adding Scientific Degree",
                    "An unexpected error occurred while adding Scientific Degree. Please try again later.");
            }
        }

        public async Task<Response<IQueryable<ScientificDegreeDto>>> GetScientificDegreeAsync()
        {
            try
            {
                var scientificDegreeEntities = await _unitOfWork.ScientificDegrees.GetAll();

                if (!scientificDegreeEntities.Any())
                    return Response<IQueryable<ScientificDegreeDto>>.NoContent("No Scientific Degrees are exist");

                var scientificDegreeDto = scientificDegreeEntities.Select(entity => new ScientificDegreeDto
                {
                    Name = entity.Name,
                    Description = entity.Description,
                    Type = entity.Type,
                    BylawId = entity.BylawId,
                    SuccessPercentageCourse = entity.SuccessPercentageCourse,
                    SuccessPercentageBand = entity.SuccessPercentageBand,
                    SuccessPercentageSemester = entity.SuccessPercentageSemester,
                    SuccessPercentagePhase = entity.SuccessPercentagePhase,
                    PhaseId = entity.PhaseId,
                    BandId = entity.BandId,
                    SemesterId = entity.SemesterId,
                    ExamRoleId = entity.ExamRoleId,
                    ParentId = entity.ParentId
                });

                return Response<IQueryable<ScientificDegreeDto>>.Success(scientificDegreeDto.AsQueryable()
                    , "Scientific Degrees retrieved successfully").WithCount();
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "ScientificDegreeService",
                    MethodName = "GetScientificDegreeAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<IQueryable<ScientificDegreeDto>>.ServerError("Error occured while retrieving Scientific Degrees",
                    "An unexpected error occurred while retrieving Scientific Degrees. Please try again later.");
            }
        }

        public async Task<Response<ScientificDegreeDto>> GetScientificDegreeByIdAsync(int ScientificDegreeId)
        {
            try
            {
                var scientificDegreeEntities = await _unitOfWork.ScientificDegrees.GetByIdAsync(ScientificDegreeId);

                if (scientificDegreeEntities == null)
                    return Response<ScientificDegreeDto>.BadRequest("This Scientific Degree doesn't exist");

                ScientificDegreeDto scientificDegreeDto = new ScientificDegreeDto
                {
                    Id = ScientificDegreeId,
                    Name = scientificDegreeEntities.Name,
                    Description = scientificDegreeEntities.Description,
                    Type = scientificDegreeEntities.Type,
                    BylawId = scientificDegreeEntities.BylawId,
                    SuccessPercentageCourse = scientificDegreeEntities.SuccessPercentageCourse,
                    SuccessPercentageBand = scientificDegreeEntities.SuccessPercentageBand,
                    SuccessPercentageSemester = scientificDegreeEntities.SuccessPercentageSemester,
                    SuccessPercentagePhase = scientificDegreeEntities.SuccessPercentagePhase,
                    PhaseId = scientificDegreeEntities.PhaseId,
                    BandId = scientificDegreeEntities.BandId,
                    SemesterId = scientificDegreeEntities.SemesterId,
                    ExamRoleId = scientificDegreeEntities.ExamRoleId,
                    ParentId = scientificDegreeEntities.ParentId
                };

                return Response<ScientificDegreeDto>.Success(scientificDegreeDto, "Scientific Degree retrieved successfully").WithCount();
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "ScientificDegreeService",
                    MethodName = "GetScientificDegreeByIdAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<ScientificDegreeDto>.ServerError("Error occured while retrieving Scientific Degree",
                    "An unexpected error occurred while retrieving Scientific Degree. Please try again later.");
            }
        }

        public async Task<Response<int>> UpdateScientificDegreeAsync(ScientificDegreeDto updateScientificDegreeDto)
        {
            try
            {
                ScientificDegree existingScientificDegree = await _unitOfWork.ScientificDegrees.GetByIdAsync(updateScientificDegreeDto.Id);

                if (existingScientificDegree == null)
                    return Response<int>.BadRequest("This Scientific Degree doesn't exist");

                existingScientificDegree.Name = updateScientificDegreeDto.Name;
                existingScientificDegree.Description = updateScientificDegreeDto.Description;
                existingScientificDegree.Type = updateScientificDegreeDto.Type;
                existingScientificDegree.BylawId = updateScientificDegreeDto.BylawId;
                existingScientificDegree.SuccessPercentageCourse = updateScientificDegreeDto.SuccessPercentageCourse;
                existingScientificDegree.SuccessPercentageBand = updateScientificDegreeDto.SuccessPercentageBand;
                existingScientificDegree.SuccessPercentageSemester = updateScientificDegreeDto.SuccessPercentageSemester;
                existingScientificDegree.SuccessPercentagePhase = updateScientificDegreeDto.SuccessPercentagePhase;
                existingScientificDegree.PhaseId = updateScientificDegreeDto.PhaseId;
                existingScientificDegree.BandId = updateScientificDegreeDto.BandId;
                existingScientificDegree.SemesterId = updateScientificDegreeDto.SemesterId;
                existingScientificDegree.ExamRoleId = updateScientificDegreeDto.ExamRoleId;
                existingScientificDegree.ParentId = updateScientificDegreeDto.ParentId;

                await _unitOfWork.ScientificDegrees.Update(existingScientificDegree);
                var result = await _unitOfWork.SaveAsync();

                if (result > 0)
                    return Response<int>.Updated("Scientific Degree updated successfully");

                return Response<int>.ServerError("Error occured while updating Scientific Degree",
                    "An unexpected error occurred while updating Scientific Degree. Please try again later.");
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "ScientificDegreeService",
                    MethodName = "UpdateScientificDegreeAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<int>.ServerError("Error occured while updating Scientific Degree",
                    "An unexpected error occurred while updating Scientific Degree. Please try again later.");
            }
        }

        public async Task<Response<int>> DeleteScientificDegreeAsync(int ScientificDegreeId)
        {
            try
            {
                var existingScientificDegree = await _unitOfWork.ScientificDegrees.GetByIdAsync(ScientificDegreeId);

                if (existingScientificDegree == null)
                    return Response<int>.BadRequest("This Scientific Degree doesn't exist");

                await _unitOfWork.ScientificDegrees.Delete(existingScientificDegree);
                var result = await _unitOfWork.SaveAsync();

                if (result > 0)
                    return Response<int>.Deleted("Scientific Degree deleted successfully");

                return Response<int>.ServerError("Error occured while deleting Scientific Degree",
                    "An unexpected error occurred while deleting Scientific Degree. Please try again later.");
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "ScientificDegreeService",
                    MethodName = "DeleteScientificDegreeAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<int>.ServerError("Error occured while deleting Scientific Degree",
                    "An unexpected error occurred while deleting Scientific Degree. Please try again later.");
            }
        }

        public async Task<Response<GetDetailsByParentIdDto>> GetDetailsByParentIdAsync(int ParentId)
        {
            try
            {
                var results = await _unitOfWork.ScientificDegrees.GetEntityByPropertyAsync(p => p.ParentId == ParentId);

                if (results == null || !results.Any())
                    return Response<GetDetailsByParentIdDto>.BadRequest("This Scientific Degree doesn't exist");

                var scientificDegree = results.Select(sc => new GetDetailsDtos
                {
                    Id = sc.Id,
                    Name = sc.Name,
                }).ToList();
                var getDetailsByParentIdDto = new GetDetailsByParentIdDto
                {
                    GetDetailsDtos = scientificDegree
                };

                return Response<GetDetailsByParentIdDto>.Success(getDetailsByParentIdDto, "Scientific Degree retrieved successfully")
                    .WithCount(getDetailsByParentIdDto.GetDetailsDtos.Count);
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "ScientificDegreeService",
                    MethodName = "GetDetailsByParentIdAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<GetDetailsByParentIdDto>.ServerError("Error occured while retrieving Scientific Degree",
                    "An unexpected error occurred while retrieving Scientific Degree. Please try again later.");
            }
        }

        public async Task<Response<IQueryable<ScientificDegreeDto>>> GetScientificDegreeByBylawId(int bylawId)
        {
            try
            {
                var scientificDegreeEntities = await _unitOfWork.ScientificDegrees.GetEntityByPropertyAsync(scien => scien.BylawId == bylawId);

                if (!scientificDegreeEntities.Any())
                    return Response<IQueryable<ScientificDegreeDto>>.NoContent("No Scientific Degrees are exist");

                var scientificDegreeDto = scientificDegreeEntities.Select(entity => new ScientificDegreeDto
                {
                    Name = entity.Name,
                    Description = entity.Description,
                    Type = entity.Type,
                    BylawId = entity.BylawId,
                    SuccessPercentageCourse = entity.SuccessPercentageCourse,
                    SuccessPercentageBand = entity.SuccessPercentageBand,
                    SuccessPercentageSemester = entity.SuccessPercentageSemester,
                    SuccessPercentagePhase = entity.SuccessPercentagePhase,
                    PhaseId = entity.PhaseId,
                    BandId = entity.BandId,
                    SemesterId = entity.SemesterId,
                    ExamRoleId = entity.ExamRoleId,
                    ParentId = entity.ParentId
                });

                return Response<IQueryable<ScientificDegreeDto>>.Success(scientificDegreeDto.AsQueryable()
                    , "Scientific Degrees retrieved successfully").WithCount();
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "ScientificDegreeService",
                    MethodName = "GetScientificDegreeByBylawId",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<IQueryable<ScientificDegreeDto>>.ServerError("Error occured while retrieving Scientific Degrees",
                    "An unexpected error occurred while retrieving Scientific Degrees. Please try again later.");
            }
        }
    }
}
