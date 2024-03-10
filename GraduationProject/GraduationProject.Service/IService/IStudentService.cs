using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.StudentDto;

namespace GraduationProject.Service.IService
{
    public interface IStudentService
    {
        Task<int> AddStudentAsync(AddStudentDto addStudentDto);
        Task<int> AddStudentSemesterAsync(AddStudentSemesterDto addStudentSemesterDto);
        Task<Response<GetStudentDetailsByUserIdDto>> GetStudentByUserId(string userId);
        Task<List<GetAllStudentsDto>> GetAllStudentsAsync();
    }
}
