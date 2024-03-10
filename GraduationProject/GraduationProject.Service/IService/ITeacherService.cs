using GraduationProject.Service.DataTransferObject.StaffDto;

namespace GraduationProject.Service.IService
{
    public interface ITeacherService
    {
        Task<int> AddTeacheAsync(AddStaffDto addSaffDto);
        Task<List<GetAllStaffsDto>> GetAllTeachersAsync();
    }
}
