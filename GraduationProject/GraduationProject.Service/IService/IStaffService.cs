using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.StaffDto;

namespace GraduationProject.Service.IService
{
    public interface IStaffService
    {
        Task<int> AddStAffAsync(AddStaffDto addSaffDto);
        Task<Response<int>> AddStaffSemesterAsync(AddStaffSemesterDto addStaffSemesterDto);
        Task<Response<GetCourseStaffSemester>> Test(int satffId);
        Task<Response<GetStaffDetailsByUserIdDto>> GetStaffByUserId(string userId);
    }
}
