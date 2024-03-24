using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.AcademyYearDto;

namespace GraduationProject.Service.IService
{
    public interface IAcademyYearService
    {
        Task<Response<int>> AddAcademyYearAsync(AcademyYearDto addAcademyYearDto);
        Task<Response<int>> UpdateAcademyYearAsync(AcademyYearDto updateAcademyYearDto);
        Task<Response<int>> DeleteAcademyYearAsync(int academyYearId);
        Task<Response<AcademyYearDto>> GetAcademyYearByIdAsync(int academyYearId);
        Task<Response<IQueryable<AcademyYearDto>>> GetAcademyYearAsync();
        //Task<int> AcademyYearOperation(AcademyYear academyYear);
    }
}
