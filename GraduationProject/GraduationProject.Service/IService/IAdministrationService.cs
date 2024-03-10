using GraduationProject.Service.DataTransferObject.StaffDto;

namespace GraduationProject.Service.IService
{
    public interface IAdministrationService
    {
        Task<int> AddAdministrationAsync(AddStaffDto addStaffDto);
        Task<List<GetAllStaffsDto>> GetAllAdministrationsAsync();
    }
}
