using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.ScientificDegreeDto;
using GraduationProject.Service.DataTransferObject.SemesterDto;

namespace GraduationProject.Service.IService
{
    public interface IScientificDegreeService
    {
        Task<Response<int>> AddScientificDegreeAsync(ScientificDegreeDto addScientificDegreeDto);
        Task<Response<int>> UpdateScientificDegreeAsync(ScientificDegreeDto updateScientificDegreeDto);
        Task<Response<int>> DeleteScientificDegreeAsync(int ScientificDegreeId);
        Task<Response<ScientificDegreeDto>> GetScientificDegreeByIdAsync(int ScientificDegreeId);
        Task<Response<IQueryable<ScientificDegreeDto>>> GetScientificDegreeAsync();
        Task<Response<IQueryable<ScientificDegreeDto>>> GetScientificDegreeByBylawIdForSpecificTypeAsync(int bylawId, int type);
        Task<Response<GetDetailsByParentIdDto>> GetDetailsByParentIdAsync(int ParentId);
        Task<Response<List<GetSemesterNameDto>>> GetSemsetersByBylawIdAsync(int facultyId);
        Task<Response<List<GetAllStudentsInSemesterDto>>> GetAllStudentsInSemesterAsync(int semesterId);
    }
}
