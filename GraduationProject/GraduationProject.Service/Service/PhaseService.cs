using GraduationProject.Data.Entity;
using GraduationProject.Mails.IService;
using GraduationProject.Mails.Models;
using GraduationProject.Repository.IRepository;
using GraduationProject.Repository.Repository;
using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.PhaseDto;
using GraduationProject.Service.IService;

namespace GraduationProject.Service.Service
{
    public class PhaseService : IPhaseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMailService _mailService;
        public PhaseService(UnitOfWork unitOfWork, IMailService mailService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mailService = mailService;
        }
        public async Task<Response<int>> AddPhaseAsync(PhaseDto addPhaseDto)
        {
            try
            {
                Phase newPhase = new Phase
                {
                    Name = addPhaseDto.Name,
                    Code = addPhaseDto.Code,
                    Order = addPhaseDto.Order,
                    FacultyId = addPhaseDto.FacultyId
                };
                await _unitOfWork.Phases.AddAsync(newPhase);
                var result = await _unitOfWork.SaveAsync();

                if (result > 0)
                    return Response<int>.Created("Phase added successfully");

                return Response<int>.ServerError("Error occured while adding phase",
                    "An unexpected error occurred while adding phase. Please try again later.");
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "PhaseService",
                    MethodName = "AddPhaseAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<int>.ServerError("Error occured while adding phase",
                    "An unexpected error occurred while adding phase. Please try again later.");
            }
        }



        public async Task<Response<IQueryable<PhaseDto>>> GetPhaseAsync()
        {
            try
            {
                var phaseEntities = await _unitOfWork.Phases.GetAll();
                if (!phaseEntities.Any())
                    return Response<IQueryable<PhaseDto>>.NoContent("No phases are exist");

                var phaseDtos = phaseEntities.Select(entity => new PhaseDto
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Code = entity.Code,
                    Order = entity.Order,
                    FacultyId = entity.FacultyId
                });

                return Response<IQueryable<PhaseDto>>.Success(phaseDtos.AsQueryable(), "Phases retrieved successfully").WithCount();
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "PhaseService",
                    MethodName = "GetPhaseAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<IQueryable<PhaseDto>>.ServerError("Error occured while retrieving phases",
                    "An unexpected error occurred while retrieving phases. Please try again later.");
            }
        }

        public async Task<Response<PhaseDto>> GetPhaseByIdAsync(int PhaseId)
        {
            try
            {
                var phaseEntity = await _unitOfWork.Phases.GetByIdAsync(PhaseId);
                if (phaseEntity == null)
                    return Response<PhaseDto>.BadRequest("This phase doesn't exist");

                PhaseDto phaseDto = new PhaseDto
                {
                    Id = phaseEntity.Id,
                    Name = phaseEntity.Name,
                    Code = phaseEntity.Code,
                    Order = phaseEntity.Order,
                    FacultyId = phaseEntity.FacultyId
                };
                return Response<PhaseDto>.Success(phaseDto, "Phase retrieved successfully").WithCount();
            }
            catch(Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "PhaseService",
                    MethodName = "GetPhaseByIdAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<PhaseDto>.ServerError("Error occured while retrieving phase",
                    "An unexpected error occurred while retrieving phase. Please try again later.");
            }
        }

        public async Task<Response<int>> UpdatePhaseAsync(PhaseDto updatePhaseDto)
        {
            try
            {
                Phase existingPhase = await _unitOfWork.Phases.GetByIdAsync(updatePhaseDto.Id);
                if (existingPhase == null)
                    return Response<int>.BadRequest("This phase doesn't exist");

                existingPhase.Name = updatePhaseDto.Name;
                existingPhase.Code = updatePhaseDto.Code;
                existingPhase.Order = updatePhaseDto.Order;
                existingPhase.FacultyId = updatePhaseDto.FacultyId;

                await _unitOfWork.Phases.Update(existingPhase);
                var result = await _unitOfWork.SaveAsync();

                if (result > 0)
                    return Response<int>.Updated("Phase updated successfully");

                return Response<int>.ServerError("Error occured while updating phase",
                        "An unexpected error occurred while updating phase. Please try again later.");
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "PhaseService",
                    MethodName = "UpdatePhaseAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<int>.ServerError("Error occured while updating phase",
                        "An unexpected error occurred while updating phase. Please try again later.");
            }
        }
        public async Task<Response<int>> DeletePhaseAsync(int PhaseId)
        {
            try
            {
                var existingPhase = await _unitOfWork.Phases.GetByIdAsync(PhaseId);
                if (existingPhase == null)
                    return Response<int>.BadRequest("This phase doesn't exist");

                await _unitOfWork.Phases.Delete(existingPhase);
                var result = await _unitOfWork.SaveAsync();

                if (result > 0)
                    return Response<int>.Deleted("Phase deleted successfully");

                return Response<int>.ServerError("Error occured while deleting phase",
                        "An unexpected error occurred while deleting phase. Please try again later.");
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "PhaseService",
                    MethodName = "DeletePhaseAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<int>.ServerError("Error occured while deleting phase",
                        "An unexpected error occurred while deleting phase. Please try again later.");
            }
        }
    }
}
