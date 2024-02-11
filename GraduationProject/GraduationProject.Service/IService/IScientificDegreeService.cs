using GraduationProject.Service.DataTransferObject.ScientificDegreeDto;

namespace GraduationProject.Service.IService
{
    public interface IScientificDegreeService
    {
        Task AddScientificDegreeAsync(ScientificDegreeDto addScientificDegreeDto);
        Task UpdateScientificDegreeAsync(ScientificDegreeDto updateScientificDegreeDto);
        Task DeleteScientificDegreeAsync(int ScientificDegreeId);
        Task<ScientificDegreeDto> GetScientificDegreeByIdAsync(int ScientificDegreeId);
        Task<IQueryable<ScientificDegreeDto>> GetScientificDegreeAsync();
    }
}
