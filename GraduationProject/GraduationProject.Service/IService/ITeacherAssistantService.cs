using GraduationProject.Service.DataTransferObject.StaffDto;

namespace GraduationProject.Service.IService
{
    public interface ITeacherAssistantService
    {
        Task<int> AddTeacherAssistantAsync(AddStaffDto addTeacherAssistantDto);
        Task<List<GetAllStaffsDto>> GetAllTeacherAssistantsAsync();
    }
}
