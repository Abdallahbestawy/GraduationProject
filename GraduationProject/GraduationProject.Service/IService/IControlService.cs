using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.SemesterDto;
using GraduationProject.Service.DataTransferObject.StaffDto;

namespace GraduationProject.Service.IService
{
    public interface IControlService
    {
        Task<Response<bool>> RaisingGradesSemesterAsync(int semesterId);
        Task<Response<bool>> RaisingGradesCourseAsync(int courseId);
        Task<Response<List<GetAllSemesterCurrentDto>>> GetAllSemesterCurrentAsync();
        Task<bool> EndSemesterAsync(int semesterId);
        Task<Response<int>> AddControlMembersAsync(AddStaffDto addControlMembersDto);
        Task<Response<List<GetAllStaffsDto>>> GetAllControlMembersAsync();
        //Task Test();
    }
}
