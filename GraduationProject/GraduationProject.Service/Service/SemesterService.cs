using GraduationProject.Data.Entity;
using GraduationProject.Mails.IService;
using GraduationProject.Mails.Models;
using GraduationProject.Repository.IRepository;
using GraduationProject.Repository.Repository;
using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.SemesterDto;
using GraduationProject.Service.IService;

namespace GraduationProject.Service.Service
{
    public class SemesterService : ISemesterService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMailService _mailService;

        public SemesterService(UnitOfWork unitOfWork, IMailService mailService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mailService = mailService;
        }

        public async Task<Response<int>> AddSemesterAsync(SemesterDto addSemesterDto)
        {
            try
            {
                Semester newSemester = new Semester
                {
                    Name = addSemesterDto.Name,
                    Code = addSemesterDto.Code,
                    Order = addSemesterDto.Order,
                    FacultyId = addSemesterDto.FacultyId
                };
                await _unitOfWork.Semesters.AddAsync(newSemester);
                var result = await _unitOfWork.SaveAsync();

                if (result > 0)
                    return Response<int>.Created("Semester added successfully");

                return Response<int>.ServerError("Error occured while adding Semester",
                    "An unexpected error occurred while adding Semester. Please try again later.");
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "SemesterService",
                    MethodName = "AddSemesterAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<int>.ServerError("Error occured while adding Semester",
                    "An unexpected error occurred while adding Semester. Please try again later.");
            }
        }

        public async Task<Response<IQueryable<SemesterDto>>> GetSemesterAsync()
        {
            try
            {
                var semesterEntities = await _unitOfWork.Semesters.GetAll();

                if (!semesterEntities.Any())
                    return Response<IQueryable<SemesterDto>>.NoContent("No semesters are exist");

                var semesterDtos = semesterEntities.Select(entity => new SemesterDto
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Code = entity.Code,
                    Order = entity.Order,
                    FacultyId = entity.FacultyId
                });

                return Response<IQueryable<SemesterDto>>.Success(semesterDtos.AsQueryable(), "Semesters retrieved successfully").WithCount();
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "SemesterService",
                    MethodName = "GetSemesterAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<IQueryable<SemesterDto>>.ServerError("Error occured while Retrieving Semesters",
                    "An unexpected error occurred while Retrieving Semesters. Please try again later.");
            }
        }

        public async Task<Response<SemesterDto>> GetSemesterByIdAsync(int SemesterId)
        {
            try
            {
                var semesterEntity = await _unitOfWork.Semesters.GetByIdAsync(SemesterId);
                if (semesterEntity == null)
                    return Response<SemesterDto>.BadRequest("This semester doesn't exist");

                SemesterDto semesterDto = new SemesterDto
                {
                    Id = semesterEntity.Id,
                    Name = semesterEntity.Name,
                    Code = semesterEntity.Code,
                    Order = semesterEntity.Order,
                    FacultyId = semesterEntity.FacultyId
                };
                return Response<SemesterDto>.Success(semesterDto, "Semester retrieved successfully").WithCount();
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "SemesterService",
                    MethodName = "GetSemesterByIdAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<SemesterDto>.ServerError("Error occured while Retrieving Semester",
                    "An unexpected error occurred while Retrieving Semester. Please try again later.");
            }
        }

        public async Task<Response<int>> UpdateSemesterAsync(SemesterDto updateSemesterDto)
        {
            try
            {
                Semester existingSemester = await _unitOfWork.Semesters.GetByIdAsync(updateSemesterDto.Id);

                if (existingSemester == null)
                    return Response<int>.BadRequest("This semester doesn't exist");

                existingSemester.Name = updateSemesterDto.Name;
                existingSemester.Code = updateSemesterDto.Code;
                existingSemester.Order = updateSemesterDto.Order;
                existingSemester.FacultyId = updateSemesterDto.FacultyId;

                await _unitOfWork.Semesters.Update(existingSemester);
                var result = await _unitOfWork.SaveAsync();

                if (result > 0)
                    return Response<int>.Updated("Semester updated successfully");

                return Response<int>.ServerError("Error occured while updating Semester",
                    "An unexpected error occurred while updating Semester. Please try again later.");
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "SemesterService",
                    MethodName = "UpdateSemesterAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<int>.ServerError("Error occured while updating Semester",
                    "An unexpected error occurred while updating Semester. Please try again later.");
            }
        }

        public async Task<Response<int>> DeleteSemesterAsync(int SemesterId)
        {
            try
            {
                var existingSemester = await _unitOfWork.Semesters.GetByIdAsync(SemesterId);

                if (existingSemester == null)
                    return Response<int>.BadRequest("This semester doesn't exist");

                await _unitOfWork.Semesters.Delete(existingSemester);
                var result = await _unitOfWork.SaveAsync();

                if (result > 0)
                    return Response<int>.Deleted("Semester deleted successfully");

                return Response<int>.ServerError("Error occured while deleting Semester",
                    "An unexpected error occurred while deleting Semester. Please try again later.");
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "SemesterService",
                    MethodName = "DeleteSemesterAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<int>.ServerError("Error occured while deleting Semester",
                    "An unexpected error occurred while deleting Semester. Please try again later.");
            }
        }

        public async Task<Response<IQueryable<GetSemesterDto>>> GetSemesterByFacultyIdAsync(int facultyId)
        {
            try
            {
                var semesterEntities = await _unitOfWork.Semesters.FindWithIncludeIEnumerableAsync(f => f.Faculty);

                if (semesterEntities == null || !semesterEntities.Any())
                    return Response<IQueryable<GetSemesterDto>>.NoContent("No semesters are exist");
                var semesterEntitiesFilter = semesterEntities.Where(f => f.FacultyId == facultyId).ToList();
                if (semesterEntitiesFilter == null || !semesterEntitiesFilter.Any())
                    return Response<IQueryable<GetSemesterDto>>.NoContent("No semesters are exist");

                var semesterDtos = semesterEntitiesFilter.Select(entity => new GetSemesterDto
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Code = entity.Code,
                    Order = entity.Order,
                    FacultyName = entity.Faculty.Name
                });

                return Response<IQueryable<GetSemesterDto>>.Success(semesterDtos.AsQueryable(), "Semesters retrieved successfully").WithCount();
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "SemesterService",
                    MethodName = "GetSemesterAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<IQueryable<GetSemesterDto>>.ServerError("Error occured while Retrieving Semesters",
                    "An unexpected error occurred while Retrieving Semesters. Please try again later.");
            }
        }
    }
}
