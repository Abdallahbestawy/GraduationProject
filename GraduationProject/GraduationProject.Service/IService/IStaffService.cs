using GraduationProject.Service.DataTransferObject.StaffDto;

namespace GraduationProject.Service.IService
{
    public interface IStaffService
    {
        Task<int> AddStAffAsync(AddStaffDto addSaffDto);
    }
}
