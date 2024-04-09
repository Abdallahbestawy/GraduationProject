using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.StaffDto;

namespace GraduationProject.Service.IService
{
    public interface IAdministrationService
    {
        Task<Response<int>> AddAdministrationAsync(AddStaffDto addStaffDto);
        Task<Response<List<GetAllStaffsDto>>> GetAllAdministrationsAsync();
        Task<Response<bool>> DeleteAsync(int id);
    }
}
