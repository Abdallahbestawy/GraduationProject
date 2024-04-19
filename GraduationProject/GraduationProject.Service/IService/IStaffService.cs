using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.StaffDto;

namespace GraduationProject.Service.IService
{
    public interface IStaffService
    {
        Task<Response<int>> AddStAffAsync(AddStaffDto addSaffDto);
        Task<Response<int>> AddStaffSemesterAsync(List<AddStaffSemesterDto> addStaffSemesterDto);
        Task<Response<GetCourseStaffSemesterDto>> GetCourseStaffSemesterAsync(string userId);
        Task<Response<GetStaffDetailsByUserIdDto>> GetStaffByUserIdAsync(string userId);
        Task<Response<List<GetAllStaffsDto>>> GetAllStaffsAsync();
        Task<Response<bool>> DeleteStaffSemesterAsync(int staffSemesterId);
        Task<Response<bool>> DeleteAsync(int id);
        Task<Response<int>> UpdateStaffAsync(UpdateStaffDto updateStaffDto);
    }
}
