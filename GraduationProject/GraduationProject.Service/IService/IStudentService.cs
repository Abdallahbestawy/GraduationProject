using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.StudentDto;

namespace GraduationProject.Service.IService
{
    public interface IStudentService
    {
        Task<Response<int>> AddStudentAsync(AddStudentDto addStudentDto);
        Task<Response<int>> AddStudentSemesterAsync(AddStudentSemesterDto addStudentSemesterDto);
        Task<Response<GetStudentDetailsByUserIdDto>> GetStudentByUserId(string userId);
        Task<Response<List<GetAllStudentsDto>>> GetAllStudentsAsync();
    }
}
