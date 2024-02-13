using GraduationProject.Data.Entity;
using GraduationProject.Repository.IRepository;
using GraduationProject.Repository.Repository;
using GraduationProject.Service.DataTransferObject.BylawDto;
using GraduationProject.Service.IService;

namespace GraduationProject.Service.Service
{
    public class BylawService : IBylawService
    {
        private readonly IUnitOfWork _unitOfWork;
        public BylawService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

        }
        public async Task AddBylawAsync(BylawDto addBylawDto)
        {
            Bylaw newBylaw = new Bylaw
            {
                Name = addBylawDto.Name,
                Description = addBylawDto.Description,
                Type = addBylawDto.Type,
                Start = addBylawDto.Start,
                End = addBylawDto.End,
                FacultyId = addBylawDto.FacultyId,
            };

            await _unitOfWork.Bylaws.AddAsync(newBylaw);
            _unitOfWork.Save();

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

            await _unitOfWork.Estimates.AddRangeAsync(estimates);
            _unitOfWork.Save();

            List<EstimatesCourse> estimatesCourses = addBylawDto.EstimatesCourses.Select(estCourse =>
                new EstimatesCourse
                {
                    BylawId = bylawyId,
                    Name = estCourse.NameEstimatesCourse,
                    Char = estCourse.CharEstimatesCourse,
                    MaxPercentage = estCourse.MaxPercentageEstimatesCourse,
                    MinPercentage = estCourse.MinPercentageEstimatesCourse
                }).ToList();

            await _unitOfWork.EstimatesCourses.AddRangeAsync(estimatesCourses);
            _unitOfWork.Save();
        }




        public async Task<IQueryable<BylawDto>> GetBylawAsync()
        {
            var bylawEntities = await _unitOfWork.Bylaws.GetAll();

            var bylawDtos = bylawEntities.Select(entity => new BylawDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Start = entity.Start,
                End = entity.End,
                FacultyId = entity.FacultyId
            });

            return bylawDtos.AsQueryable();
        }

        public async Task<BylawDto> GetBylawByIdAsync(int BylawId)
        {
            var bylawEntity = await _unitOfWork.Bylaws.GetByIdAsync(BylawId);
            BylawDto bylawDto = new BylawDto
            {
                Id = bylawEntity.Id,
                Name = bylawEntity.Name,
                Description = bylawEntity.Description,
                Type = bylawEntity.Type,
                Start = bylawEntity.Start,
                End = bylawEntity.End,
                FacultyId = bylawEntity.FacultyId
            };
            return (bylawDto);
        }

        public async Task UpdateBylawAsync(BylawDto updateBylawDto)
        {
            Bylaw existingBylaw = await _unitOfWork.Bylaws.GetByIdAsync(updateBylawDto.Id);
            if (existingBylaw == null)
            {
                throw new Exception("Bylaw not found");
            }
            existingBylaw.Name = updateBylawDto.Name;
            existingBylaw.Description = updateBylawDto.Description;
            existingBylaw.Type = updateBylawDto.Type;
            existingBylaw.Start = updateBylawDto.Start;
            existingBylaw.End = updateBylawDto.End;
            existingBylaw.FacultyId = updateBylawDto.FacultyId;

            await _unitOfWork.Bylaws.Update(existingBylaw);
            _unitOfWork.Save();
        }
        public async Task DeleteBylawAsync(int BylawId)
        {
            var existingBlaw = await _unitOfWork.Bylaws.GetByIdAsync(BylawId);
            await _unitOfWork.Bylaws.Delete(existingBlaw);
            _unitOfWork.Save();
        }
    }
}
