using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.SemesterDto;

namespace GraduationProject.Service.IService
{
    public interface ISemesterService
    {
        Task<Response<int>> AddSemesterAsync(SemesterDto addSemesterDto);
        Task<Response<int>> UpdateSemesterAsync(SemesterDto updateSemesterDto);
        Task<Response<int>> DeleteSemesterAsync(int SemesterId);
        Task<Response<SemesterDto>> GetSemesterByIdAsync(int SemesterId);
        Task<Response<IQueryable<SemesterDto>>> GetSemesterAsync();
    }
}
