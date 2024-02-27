using GraduationProject.Service.DataTransferObject.StudentDto;
using GraduationProject.Service.DataTransferObject.StudentSemester;

namespace GraduationProject.Service.IService
{
    public interface IStudentService
    {
        Task<int> AddStudentAsync(AddStudentDto addStudentDto);
        Task<int> AddStudentSemesterAsync(AddStudentSemesterDto addStudentSemesterDto);
    }
}
