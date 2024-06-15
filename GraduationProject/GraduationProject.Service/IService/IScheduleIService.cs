using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.ScheduleDto;

namespace GraduationProject.Service.IService
{
    public interface IScheduleIService
    {
        Task<Response<int>> AddScheduleAsync(ScheduleDto addScheduleDto);
        Task<Response<int>> UpdateScheduleAsync(ScheduleDto updateScheduleDto);
        Task<Response<int>> DeleteScheduleAsync(int scheduleId);
        Task<Response<ScheduleDto>> GetScheduleBySemesterIdAsync(int semesterId, int factlyId);
        Task<Response<GetSchedulesForStaffByUserIdDto>> GetSchedulesForStaffByUserIdAsync(string userId);
    }
}
