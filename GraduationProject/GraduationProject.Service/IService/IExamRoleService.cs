using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.ExamRolesDto;

namespace GraduationProject.Service.IService
{
    public interface IExamRoleService
    {
        Task<Response<int>> AddExamRoleAsync(ExamRolesDto addExamRoleDto);
        Task<Response<int>> UpdateExamRoleAsync(ExamRolesDto updateExamRoleDto);
        Task<Response<int>> DeleteExamRoleAsync(int ExamRoleId);
        Task<Response<ExamRolesDto>> GetExamRoleByIdAsync(int ExamRoleId);
        Task<Response<IQueryable<ExamRolesDto>>> GetExamRoleAsync();
        Task<Response<IQueryable<ExamRolesDto>>> GetExamRoleByFacultyIdAsync(int facultyId);
    }
}
