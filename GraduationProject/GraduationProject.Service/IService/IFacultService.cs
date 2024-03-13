using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.FacultyDto;

namespace GraduationProject.Service.IService
{

    public interface IFacultService
    {
        Task<Response<int>> AddFacultAsync(FacultyDto facultyDto);
        Task<Response<GetFacultyDetailsDto>> GetFacultyDetailsAsync(int facultyId);
        Task<Response<GetFacultyByUserIdDto>> GetFacultByUserIdAsync(string userId);
    }
}
