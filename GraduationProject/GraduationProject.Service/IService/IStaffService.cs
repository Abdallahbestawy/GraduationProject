using GraduationProject.Service.DataTransferObject.StaffDto;

namespace GraduationProject.Service.IService
{
    public interface IStaffService
    {
        Task<int> AddStAffAsync(AddStaffDto addSaffDto);
        Task<int> AddStaffSemesterAsync(AddStaffSemesterDto addStaffSemesterDto);
        Task<GetCourseStaffSemester> Test(int satffId);
        Task<GetStaffDetailsByUserIdDto> GetStaffByUserId(string userId);
    }
}
