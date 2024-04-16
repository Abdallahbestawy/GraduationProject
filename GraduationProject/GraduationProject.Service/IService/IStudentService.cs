using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.ScientificDegreeDto;
using GraduationProject.Service.DataTransferObject.StudentDto;

namespace GraduationProject.Service.IService
{
    public interface IStudentService
    {
        Task<Response<int>> AddStudentAsync(AddStudentDto addStudentDto);
        Task<Response<int>> AddStudentSemesterAsync(AddStudentSemesterDto addStudentSemesterDto);
        Task<Response<GetStudentDetailsByUserIdDto>> GetStudentByUserId(string userId);
        Task<Response<List<GetAllStudentsDto>>> GetAllStudentsAsync();
        Task<Response<int>> AssignCoursesToStudents();
        Task<Response<bool>> DeleteStudentAsync(int studentId);
        Task<Response<bool>> DeleteStudentSemesterAsync(int studentSemesterId);
        Task<Response<int>> UpdateStudentAsync(AddStudentDto updateStudentDto);
        Task<Response<GetStudentResultDto>> GetStudentResultAsync(string userId);
        Task<Response<List<GetAllStudentsInSemesterDto>>> GetAllStudentsInSemesterAsync(int semesterId);

    }
}
