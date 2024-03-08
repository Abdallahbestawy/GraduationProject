using GraduationProject.Data.Entity;
using GraduationProject.Repository.IRepository;
using GraduationProject.Repository.Repository;
using GraduationProject.Service.DataTransferObject.ScientificDegreeDto;
using GraduationProject.Service.IService;

namespace GraduationProject.Service.Service
{
    public class ScientificDegreeService : IScientificDegreeService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ScientificDegreeService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

        }
        public async Task AddScientificDegreeAsync(ScientificDegreeDto addScientificDegreeDto)
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
            await _unitOfWork.SaveAsync();
        }


        public async Task<IQueryable<ScientificDegreeDto>> GetScientificDegreeAsync()
        {
            var scientificDegreeEntities = await _unitOfWork.ScientificDegrees.GetAll();

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

            return scientificDegreeDto.AsQueryable();
        }
        public async Task<ScientificDegreeDto> GetScientificDegreeByIdAsync(int ScientificDegreeId)
        {
            var scientificDegreeEntities = await _unitOfWork.ScientificDegrees.GetByIdAsync(ScientificDegreeId);
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
            return (scientificDegreeDto);
        }

        public async Task UpdateScientificDegreeAsync(ScientificDegreeDto updateScientificDegreeDto)
        {
            ScientificDegree existingScientificDegree = await _unitOfWork.ScientificDegrees.GetByIdAsync(updateScientificDegreeDto.Id);
            if (existingScientificDegree == null)
            {
                throw new Exception("ScientificDegree not found");
            }
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
            await _unitOfWork.SaveAsync();
        }
        public async Task DeleteScientificDegreeAsync(int ScientificDegreeId)
        {
            var existingScientificDegree = await _unitOfWork.ScientificDegrees.GetByIdAsync(ScientificDegreeId);
            await _unitOfWork.ScientificDegrees.Delete(existingScientificDegree);
            await _unitOfWork.SaveAsync();
        }
    }
}
