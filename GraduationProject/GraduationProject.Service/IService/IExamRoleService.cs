using GraduationProject.Service.DataTransferObject.ExamRolesDto;

namespace GraduationProject.Service.IService
{
    public interface IExamRoleService
    {
        Task AddExamRoleAsync(ExamRolesDto addExamRoleDto);
        Task UpdateExamRoleAsync(ExamRolesDto updateExamRoleDto);
        Task DeleteExamRoleAsync(int ExamRoleId);
        Task<ExamRolesDto> GetExamRoleByIdAsync(int ExamRoleId);
        Task<IQueryable<ExamRolesDto>> GetExamRoleAsync();
    }
}
