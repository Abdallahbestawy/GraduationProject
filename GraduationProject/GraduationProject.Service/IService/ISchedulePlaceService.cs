using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.SchedulePlaceDto;

namespace GraduationProject.Service.IService
{
    public interface ISchedulePlaceService
    {
        Task<Response<int>> AddSchedulePlaceAsync(SchedulePlaceDto addSchedulePlaceDto);
        Task<Response<int>> UpdateSchedulePlaceAsync(SchedulePlaceDto updateSchedulePlaceDto);
        Task<Response<int>> DeleteSchedulePlaceAsync(int schedulePlaceId);
        Task<Response<SchedulePlaceDto>> GetSchedulePlaceByIdAsync(int schedulePlaceId);
        Task<Response<IQueryable<GetSchedulePlaceDto>>> GetSchedulePlaceByFacultyIdAsync(int facultyId);
    }
}
