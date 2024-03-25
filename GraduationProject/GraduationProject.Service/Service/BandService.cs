using GraduationProject.Data.Entity;
using GraduationProject.Mails.IService;
using GraduationProject.Mails.Models;
using GraduationProject.Repository.IRepository;
using GraduationProject.Repository.Repository;
using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.BandDto;
using GraduationProject.Service.IService;

namespace GraduationProject.Service.Service
{
    public class BandService : IBandService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMailService _mailService;

        public BandService(UnitOfWork unitOfWork, IMailService mailService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mailService = mailService;
        }

        public async Task<Response<int>> AddBandAsync(BandDto addBandDto)
        {
            try
            {
                Band newBand = new Band
                {
                    Name = addBandDto.Name,
                    Code = addBandDto.Code,
                    Order = addBandDto.Order,
                    FacultyId = addBandDto.FacultyId
                };
                await _unitOfWork.Bands.AddAsync(newBand);
                var result = await _unitOfWork.SaveAsync();

                if (result > 0)
                    return Response<int>.Created("Band added successfully");

                return Response<int>.ServerError("Error occured while adding Band",
                    "An unexpected error occurred while adding Band. Please try again later.");
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "BandService",
                    MethodName = "AddBandAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<int>.ServerError("Error occured while adding Band",
                    "An unexpected error occurred while adding Band. Please try again later.");
            }
        }

        public async Task<Response<IQueryable<BandDto>>> GetBandAsync()
        {
            try
            {
                var bandEntities = await _unitOfWork.Bands.GetAll();

                if (!bandEntities.Any())
                    return Response<IQueryable<BandDto>>.NoContent("No Bands are exist");

                var bandDto = bandEntities.Select(entity => new BandDto
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Code = entity.Code,
                    Order = entity.Order,
                    FacultyId = entity.FacultyId
                });

                return Response<IQueryable<BandDto>>.Success(bandDto, "Bands retrieved successfully").WithCount();
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "BandService",
                    MethodName = "GetBandAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<IQueryable<BandDto>>.ServerError("Error occured while retrieving Bands",
                    "An unexpected error occurred while retrieving Bands. Please try again later.");
            }
        }

        public async Task<Response<BandDto>> GetBandByIdAsync(int bandId)
        {
            try
            {
                var bandEntity = await _unitOfWork.Bands.GetByIdAsync(bandId);

                if (bandEntity == null)
                    return Response<BandDto>.BadRequest("This band doesn't exist");

                BandDto bandDto = new BandDto
                {
                    Id = bandEntity.Id,
                    Name = bandEntity.Name,
                    Code = bandEntity.Code,
                    Order = bandEntity.Order,
                    FacultyId = bandEntity.FacultyId
                };

                return Response<BandDto>.Success(bandDto, "Band retrieved successfully").WithCount();
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "BandService",
                    MethodName = "GetBandByIdAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<BandDto>.ServerError("Error occured while retrieving Band",
                    "An unexpected error occurred while retrieving Band. Please try again later.");
            }
        }

        public async Task<Response<int>> UpdateBandAsync(BandDto updateBandDto)
        {
            try
            {
                Band existingBand = await _unitOfWork.Bands.GetByIdAsync(updateBandDto.Id);

                if (existingBand == null)
                    return Response<int>.BadRequest("This band doesn't exist");

                existingBand.Name = updateBandDto.Name;
                existingBand.Code = updateBandDto.Code;
                existingBand.Order = updateBandDto.Order;
                existingBand.FacultyId = updateBandDto.FacultyId;

                await _unitOfWork.Bands.Update(existingBand);
                var result = await _unitOfWork.SaveAsync();

                if (result > 0)
                    return Response<int>.Updated("Band updated successfully");

                return Response<int>.ServerError("Error occured while updating Band",
                    "An unexpected error occurred while updating Band. Please try again later.");
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "BandService",
                    MethodName = "UpdateBandAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<int>.ServerError("Error occured while updating Band",
                    "An unexpected error occurred while updating Band. Please try again later.");
            }
        }

        public async Task<Response<int>> DeleteBandAsync(int bandId)
        {
            try
            {
                var existingBand = await _unitOfWork.Bands.GetByIdAsync(bandId);

                if (existingBand == null)
                    return Response<int>.BadRequest("This band doesn't exist");

                await _unitOfWork.Bands.Delete(existingBand);
                var result = await _unitOfWork.SaveAsync();

                if (result > 0)
                    return Response<int>.Deleted("Band deleted successfully");

                return Response<int>.ServerError("Error occured while deleting Band",
                    "An unexpected error occurred while deleting Band. Please try again later.");
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "BandService",
                    MethodName = "DeleteBandAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<int>.ServerError("Error occured while deleting Band",
                    "An unexpected error occurred while deleting Band. Please try again later.");
            }
        }

        public async Task<Response<IQueryable<GetBandDto>>> GetBandByFacultyIdAsync(int facultyId)
        {
            try
            {
                var bandEntities = await _unitOfWork.Bands.FindWithIncludeIEnumerableAsync(f => f.Faculty);

                if (bandEntities == null || !bandEntities.Any())
                    return Response<IQueryable<GetBandDto>>.NoContent("No Bands are exist");
                var bandEntitiesFilter = bandEntities.Where(f => f.FacultyId == facultyId).ToList();
                if (bandEntitiesFilter == null || !bandEntitiesFilter.Any())
                    return Response<IQueryable<GetBandDto>>.NoContent("No Bands are exist");

                var bandDto = bandEntitiesFilter.Select(entity => new GetBandDto
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Code = entity.Code,
                    Order = entity.Order,
                    FacultyName = entity.Faculty.Name
                });

                return Response<IQueryable<GetBandDto>>.Success(bandDto.AsQueryable(), "Bands retrieved successfully").WithCount();
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "BandService",
                    MethodName = "GetBandAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<IQueryable<GetBandDto>>.ServerError("Error occured while retrieving Bands",
                    "An unexpected error occurred while retrieving Bands. Please try again later.");
            }
        }
    }
}
