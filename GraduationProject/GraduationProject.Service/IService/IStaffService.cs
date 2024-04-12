using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.StaffDto;

namespace GraduationProject.Service.IService
{
    public interface IStaffService
    {
        Task<Response<int>> AddStAffAsync(AddStaffDto addSaffDto);
        Task<Response<int>> AddStaffSemesterAsync(List<AddStaffSemesterDto> addStaffSemesterDto);
        Task<Response<GetCourseStaffSemesterDto>> GetCourseStaffSemesterAsync(int satffId);
        Task<Response<GetStaffDetailsByUserIdDto>> GetStaffByUserIdAsync(string userId);
        Task<Response<List<GetAllStaffsDto>>> GetAllStaffsAsync();
        Task<bool> DeleteStaffSemesterAsync(int staffSemesterId);
    }
}
