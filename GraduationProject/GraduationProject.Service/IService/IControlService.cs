using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.SemesterDto;
using GraduationProject.Service.DataTransferObject.StaffDto;

namespace GraduationProject.Service.IService
{
    public interface IControlService
    {
        Task<Response<bool>> RaisingGradesSemesterAsync(int semesterId);
        Task<Response<bool>> RaisingGradesCourseAsync(int courseId);
        Task<Response<GetAllSemesterCurrentDto>> GetAllSemesterCurrentAsync();
        Task<Response<bool>> EndSemesterAsync(int semesterId);
        Task<Response<int>> AddControlMembersAsync(AddStaffDto addControlMembersDto);
        Task<Response<List<GetAllStaffsDto>>> GetAllControlMembersAsync();
        Task<List<GetAllSemesterActiveDto>> GetAllSemesterActiveAsync(int academyYearId);
        Task<GetStudentsSemesterResultDto> GetStudentsSemesterResultAsync(int semesterId, int acedemyYearId);
        Task<GetStudentInSemesterResultDto> GetStudentInSemesterResulAsync(int studentSemesterId);
        Task<GetAllStudentInCourseResultDto> GetAllStudentInCourseResultAsync(int semesterId, int acedemyYearId, int courseId);
    }
}
