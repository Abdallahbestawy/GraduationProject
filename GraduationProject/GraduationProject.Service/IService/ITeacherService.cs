using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.StaffDto;

namespace GraduationProject.Service.IService
{
    public interface ITeacherService
    {
        Task<Response<int>> AddTeacheAsync(AddStaffDto addSaffDto);
        Task<Response<List<GetAllStaffsDto>>> GetAllTeachersAsync();
    }
}
