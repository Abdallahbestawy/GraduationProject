using GraduationProject.Service.DataTransferObject.StudentDto;

namespace GraduationProject.Service.IService
{
    public interface IStudentService
    {
        Task<int> AddStudentAsync(AddStudentDto addStudentDto);
    }
}
