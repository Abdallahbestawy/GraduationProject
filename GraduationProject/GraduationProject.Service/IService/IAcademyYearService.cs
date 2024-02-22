using GraduationProject.Service.DataTransferObject.AcademyYearDto;

namespace GraduationProject.Service.IService
{
    public interface IAcademyYearService
    {
        Task AddAcademyYearAsync(AcademyYearDto addAcademyYearDto);
        Task UpdateAcademyYearAsync(AcademyYearDto updateAcademyYearDto);
        Task DeleteAcademyYearAsync(int academyYearId);
        Task<AcademyYearDto> GetAcademyYearByIdAsync(int academyYearId);
        Task<IQueryable<AcademyYearDto>> GetAcademyYearAsync();
    }
}
