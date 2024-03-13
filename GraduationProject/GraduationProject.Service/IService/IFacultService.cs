using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.FacultyDto;

namespace GraduationProject.Service.IService
{

    public interface IFacultService
    {
        Task<Response<int>> AddFacultAsync(FacultyDto facultyDto);
        Task<GetFacultyDetailsDto> GetFacultyDetailsAsync(int facultyId);
        Task<GetFacultyByUserIdDto> GetFacultByUserIdAsync(string userId);
    }
}
