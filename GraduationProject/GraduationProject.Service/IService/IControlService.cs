using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.AcademyYearDto;
using GraduationProject.Service.DataTransferObject.SemesterDto;
using GraduationProject.Service.DataTransferObject.StaffDto;
using System.Security.Claims;

namespace GraduationProject.Service.IService
{
    public interface IControlService
    {
        Task<Response<bool>> RaisingGradesSemesterAsync(int semesterId);
        Task<Response<bool>> RaisingGradesCourseAsync(int courseId);
        Task<Response<GetAllSemesterCurrentDto>> GetAllSemesterCurrentAsync(int facultyId);
        Task<Response<bool>> EndSemesterAsync(int semesterId);
        Task<Response<int>> AddControlMembersAsync(AddStaffDto addControlMembersDto, ClaimsPrincipal user);
        Task<Response<List<GetAllStaffsDto>>> GetAllControlMembersAsync(int FacultyId);
        Task<Response<List<GetAllSemesterActiveDto>>> GetAllSemesterActiveAsync(int academyYearId);
        Task<Response<GetStudentsSemesterResultDto>> GetStudentsSemesterResultAsync(int semesterId, int acedemyYearId);
        Task<Response<GetStudentInSemesterResultDto>> GetStudentInSemesterResulAsync(int studentSemesterId);
        Task<Response<GetAllStudentInCourseResultDto>> GetAllStudentInCourseResultAsync(int semesterId, int acedemyYearId, int courseId);
        Task<Response<List<GetAllAcdemyYearGraduatesDto>>> GetAllAcdemyYearGraduatesAsync(int facultyId);
        Task<Response<GetGraduateStudentsByAcademyYearIdDto>> GetGraduateStudentsByAcademyYearIdAsync(int acedemyYearId);
    }
}
