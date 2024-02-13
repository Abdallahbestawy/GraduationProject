using GraduationProject.Data.Entity;
using GraduationProject.Repository.IRepository;
using GraduationProject.Repository.Repository;
using GraduationProject.Service.DataTransferObject.BandDto;
using GraduationProject.Service.IService;

namespace GraduationProject.Service.Service
{
    public class BandService : IBandService
    {
        private readonly IUnitOfWork _unitOfWork;
        public BandService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

        }
        public async Task AddBandAsync(BandDto addBandDto)
        {
            Band newBand = new Band
            {
                Name = addBandDto.Name,
                Code = addBandDto.Code,
                Order = addBandDto.Order,
                FacultyId = addBandDto.FacultyId
            };
            await _unitOfWork.Bands.AddAsync(newBand);
            _unitOfWork.Save();
        }


        public async Task<IQueryable<BandDto>> GetBandAsync()
        {
            var bandEntities = await _unitOfWork.Bands.GetAll();

            var bandDto = bandEntities.Select(entity => new BandDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Code = entity.Code,
                Order = entity.Order,
                FacultyId = entity.FacultyId
            });

            return bandDto.AsQueryable();
        }

        public async Task<BandDto> GetBandByIdAsync(int bandId)
        {
            var bandEntity = await _unitOfWork.Bands.GetByIdAsync(bandId);
            BandDto bandDto = new BandDto
            {
                Id = bandEntity.Id,
                Name = bandEntity.Name,
                Code = bandEntity.Code,
                Order = bandEntity.Order,
                FacultyId = bandEntity.FacultyId
            };
            return (bandDto);
        }

        public async Task UpdateBandAsync(BandDto updateBandDto)
        {
            Band existingBand = await _unitOfWork.Bands.GetByIdAsync(updateBandDto.Id);
            if (existingBand == null)
            {
                throw new Exception("Band not found");
            }
            existingBand.Name = updateBandDto.Name;
            existingBand.Code = updateBandDto.Code;
            existingBand.Order = updateBandDto.Order;
            existingBand.FacultyId = updateBandDto.FacultyId;

            await _unitOfWork.Bands.Update(existingBand);
            _unitOfWork.Save();
        }
        public async Task DeleteBandAsync(int bandId)
        {
            var existingBand = await _unitOfWork.Bands.GetByIdAsync(bandId);
            await _unitOfWork.Bands.Delete(existingBand);
            _unitOfWork.Save();
        }
    }
}
