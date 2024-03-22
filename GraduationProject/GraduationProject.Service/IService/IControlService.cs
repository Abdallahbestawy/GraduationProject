using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.SemesterDto;

namespace GraduationProject.Service.IService
{
    public interface IControlService
    {
        Task<Response<bool>> RaisingGradesSemesterAsync(int semesterId);
        Task<Response<bool>> RaisingGradesCourseAsync(int courseId);
        Task<Response<List<GetAllSemesterCurrentDto>>> GetAllSemesterCurrentAsync();
        Task Test();
    }
}
