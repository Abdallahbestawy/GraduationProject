using GraduationProject.Service.DataTransferObject.FacultyDto;

namespace GraduationProject.Service.IService
{

    public interface IFacultService
    {
        Task AddFacultAsync(FacultyDto facultyDto);
    }
}
