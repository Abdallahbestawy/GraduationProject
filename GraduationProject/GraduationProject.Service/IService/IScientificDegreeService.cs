using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.ScientificDegreeDto;

namespace GraduationProject.Service.IService
{
    public interface IScientificDegreeService
    {
        Task<Response<int>> AddScientificDegreeAsync(ScientificDegreeDto addScientificDegreeDto);
        Task<Response<int>> UpdateScientificDegreeAsync(ScientificDegreeDto updateScientificDegreeDto);
        Task<Response<int>> DeleteScientificDegreeAsync(int ScientificDegreeId);
        Task<Response<ScientificDegreeDto>> GetScientificDegreeByIdAsync(int ScientificDegreeId);
        Task<Response<IQueryable<ScientificDegreeDto>>> GetScientificDegreeAsync();
        Task<Response<IQueryable<ScientificDegreeDto>>> GetScientificDegreeByBylawId(int bylawId);
        Task<Response<GetDetailsByParentIdDto>> GetDetailsByParentIdAsync(int ParentId);
    }
}
