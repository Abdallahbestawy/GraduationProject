using GraduationProject.Data.Entity;
using GraduationProject.Repository.IRepository;
using GraduationProject.Repository.Repository;
using GraduationProject.Service.DataTransferObject.PhaseDto;
using GraduationProject.Service.IService;

namespace GraduationProject.Service.Service
{
    public class PhaseService : IPhaseService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PhaseService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

        }
        public async Task AddPhaseAsync(PhaseDto addPhaseDto)
        {
            Phase newPhase = new Phase
            {
                Name = addPhaseDto.Name,
                Code = addPhaseDto.Code,
                Order = addPhaseDto.Order,
                FacultyId = addPhaseDto.FacultyId
            };
            await _unitOfWork.Phases.AddAsync(newPhase);
            _unitOfWork.Save();
        }



        public async Task<IQueryable<PhaseDto>> GetPhaseAsync()
        {
            var phaseEntities = await _unitOfWork.Phases.GetAll();

            var phaseDtos = phaseEntities.Select(entity => new PhaseDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Code = entity.Code,
                Order = entity.Order,
                FacultyId = entity.FacultyId
            });

            return phaseDtos.AsQueryable();
        }

        public async Task<PhaseDto> GetPhaseByIdAsync(int PhaseId)
        {
            var phaseEntity = await _unitOfWork.Phases.GetByIdAsync(PhaseId);
            PhaseDto phaseDto = new PhaseDto
            {
                Id = phaseEntity.Id,
                Name = phaseEntity.Name,
                Code = phaseEntity.Code,
                Order = phaseEntity.Order,
                FacultyId = phaseEntity.FacultyId
            };
            return (phaseDto);
        }

        public async Task UpdatePhaseAsync(PhaseDto updatePhaseDto)
        {
            Phase existingPhase = await _unitOfWork.Phases.GetByIdAsync(updatePhaseDto.Id);
            if (existingPhase == null)
            {
                throw new Exception("Band not found");
            }
            existingPhase.Name = updatePhaseDto.Name;
            existingPhase.Code = updatePhaseDto.Code;
            existingPhase.Order = updatePhaseDto.Order;
            existingPhase.FacultyId = updatePhaseDto.FacultyId;

            await _unitOfWork.Phases.Update(existingPhase);
            _unitOfWork.Save();
        }
        public async Task DeletePhaseAsync(int PhaseId)
        {
            var existingPhase = await _unitOfWork.Phases.GetByIdAsync(PhaseId);
            await _unitOfWork.Phases.Delete(existingPhase);
            _unitOfWork.Save();
        }
    }
}
