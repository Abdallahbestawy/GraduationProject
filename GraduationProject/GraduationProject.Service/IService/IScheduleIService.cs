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
        Task<Response<GetScheduleByIdDto>> GetScheduleByIdAsync(int scheduleId);
        Task<Response<GetSchedulesForStaffByUserIdDto>> GetSchedulesForStaffByUserIdAsync(string userId);
        Task<Response<List<GetStudentScheduleByUserIdDto>>> GetStudentScheduleByUserIdAsync(string userId);
        Task<Response<List<GetStudentBySectionIdDto>>> GetStudentBySectionIdAsync(int sectionId);
        Task<Response<bool>> AssignStudentsToSchedule(int ScientificDegreeId);
        Task<Response<GetScheduleDetailsDto>> GetScheduleDetailsAsync(int semesterId, int factlyId);
    }
}
