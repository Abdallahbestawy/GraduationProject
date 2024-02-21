using GraduationProject.Data.Entity;
using GraduationProject.Repository.IRepository;
using GraduationProject.Repository.Repository;
using GraduationProject.Service.DataTransferObject.AssessMethodDto;
using GraduationProject.Service.IService;

namespace GraduationProject.Service.Service
{
    public class AssessMethodService : IAssessMethodService
    {
        private readonly IUnitOfWork _unitOfWork;
        public AssessMethodService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

        }
        public async Task AddAssessMethodAsync(AssessMethodDto addAssessMethodDto)
        {
            AssessMethod newAssessMethod = new AssessMethod
            {
                Name = addAssessMethodDto.Name,
                Description = addAssessMethodDto.Description,
                MinDegree = addAssessMethodDto.MinDegree,
                MaxDegree = addAssessMethodDto.MaxDegree,
                FacultyId = addAssessMethodDto.FacultyId
            };
            await _unitOfWork.AssessMethods.AddAsync(newAssessMethod);
            _unitOfWork.Save();
        }
        public async Task<IQueryable<AssessMethodDto>> GetAssessMethodAsync()
        {
            var assessMethodEntities = await _unitOfWork.AssessMethods.GetAll();

            var assessMethodDto = assessMethodEntities.Select(entity => new AssessMethodDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                MaxDegree = entity.MaxDegree,
                MinDegree = entity.MinDegree,
                FacultyId = entity.FacultyId
            });

            return assessMethodDto.AsQueryable();
        }

        public async Task<AssessMethodDto> GetAssessMethodByIdAsync(int assessMethodId)
        {
            var assessMethodEntities = await _unitOfWork.AssessMethods.GetByIdAsync(assessMethodId);
            AssessMethodDto assessMethodDto = new AssessMethodDto
            {
                Id = assessMethodEntities.Id,
                Name = assessMethodEntities.Name,
                Description = assessMethodEntities.Description,
                MinDegree = assessMethodEntities.MinDegree,
                MaxDegree = assessMethodEntities.MaxDegree,
                FacultyId = assessMethodEntities.FacultyId
            };
            return (assessMethodDto);
        }

        public async Task UpdateAssessMethodAsync(AssessMethodDto updateAssessMethodDto)
        {
            AssessMethod existingAssessMethod = await _unitOfWork.AssessMethods.GetByIdAsync(updateAssessMethodDto.Id);
            if (existingAssessMethod == null)
            {
                throw new Exception("AssessMethod not found");
            }
            existingAssessMethod.Name = updateAssessMethodDto.Name;
            existingAssessMethod.Description = updateAssessMethodDto.Description;
            existingAssessMethod.MinDegree = updateAssessMethodDto.MinDegree;
            existingAssessMethod.MaxDegree = updateAssessMethodDto.MaxDegree;
            existingAssessMethod.FacultyId = updateAssessMethodDto.FacultyId;

            await _unitOfWork.AssessMethods.Update(existingAssessMethod);
            _unitOfWork.Save();
        }
        public async Task DeleteAssessMethodAsync(int assessMethodId)
        {
            var existingAssessMethod = await _unitOfWork.AssessMethods.GetByIdAsync(assessMethodId);
            await _unitOfWork.AssessMethods.Delete(existingAssessMethod);
            _unitOfWork.Save();
        }
    }
}
