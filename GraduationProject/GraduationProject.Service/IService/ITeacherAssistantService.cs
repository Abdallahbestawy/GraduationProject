using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.StaffDto;

namespace GraduationProject.Service.IService
{
    public interface ITeacherAssistantService
    {
        Task<Response<int>> AddTeacherAssistantAsync(AddStaffDto addTeacherAssistantDto);
        Task<Response<List<GetAllStaffsDto>>> GetAllTeacherAssistantsAsync();
    }
}
