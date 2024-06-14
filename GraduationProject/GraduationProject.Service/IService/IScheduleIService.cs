using GraduationProject.Service.DataTransferObject.ScheduleDto;

namespace GraduationProject.Service.IService
{
    public interface IScheduleIService
    {
        Task<int> AddScheduleAsync(ScheduleDto addScheduleDto);
        Task<int> UpdateScheduleAsync(ScheduleDto updateScheduleDto);
        Task<int> DeleteScheduleAsync(int scheduleId);
        Task<ScheduleDto> GetScheduleBySemesterIdAsync(int semesterId, int factlyId);
        Task<GetSchedulesForStaffByUserIdDto> GetSchedulesForStaffByUserIdAsync(string userId);




    }
}
