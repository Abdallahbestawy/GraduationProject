using GraduationProject.Data.Entity;
using GraduationProject.Repository.IRepository;
using GraduationProject.Repository.Repository;
using GraduationProject.Service.DataTransferObject.AcademyYearDto;
using GraduationProject.Service.IService;

namespace GraduationProject.Service.Service
{
    public class AcademyYearService : IAcademyYearService
    {
        private readonly IUnitOfWork _unitOfWork;
        public AcademyYearService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

        }
        public async Task AddAcademyYearAsync(AcademyYearDto addAcademyYearDto)
        {
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
            _unitOfWork.Save();
        }



        public async Task<IQueryable<AcademyYearDto>> GetAcademyYearAsync()
        {
            var academyYearEntities = await _unitOfWork.AcademyYears.GetAll();

            var academyYearDto = academyYearEntities.Select(entity => new AcademyYearDto
            {
                Id = entity.Id,
                Start = entity.Start,
                End = entity.End,
                Description = entity.Description,
                AcademyYearOrder = entity.AcademyYearOrder,
                FacultyId = entity.FacultyId,
                IsCurrent = entity.IsCurrent
            });

            return academyYearDto.AsQueryable();
        }

        public async Task<AcademyYearDto> GetAcademyYearByIdAsync(int academyYearId)
        {
            var academyYearEntity = await _unitOfWork.AcademyYears.GetByIdAsync(academyYearId);
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
            return (academyYearDto);
        }

        public async Task UpdateAcademyYearAsync(AcademyYearDto updateAcademyYearDto)
        {
            AcademyYear existingAcademyYear = await _unitOfWork.AcademyYears.GetByIdAsync(updateAcademyYearDto.Id);
            if (existingAcademyYear == null)
            {
                throw new Exception("AcademyYear not found");
            }
            existingAcademyYear.Start = updateAcademyYearDto.Start;
            existingAcademyYear.End = updateAcademyYearDto.End;
            existingAcademyYear.Description = updateAcademyYearDto.Description;
            existingAcademyYear.AcademyYearOrder = updateAcademyYearDto.AcademyYearOrder;
            existingAcademyYear.FacultyId = updateAcademyYearDto.FacultyId;
            existingAcademyYear.IsCurrent = updateAcademyYearDto.IsCurrent;

            await _unitOfWork.AcademyYears.Update(existingAcademyYear);
            _unitOfWork.Save();
        }
        public async Task DeleteAcademyYearAsync(int academyYearId)
        {
            var existingAcademyYear = await _unitOfWork.AcademyYears.GetByIdAsync(academyYearId);
            await _unitOfWork.AcademyYears.Delete(existingAcademyYear);
            _unitOfWork.Save();
        }
    }
}
