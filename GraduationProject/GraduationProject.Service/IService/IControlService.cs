using GraduationProject.Service.DataTransferObject.SemesterDto;

namespace GraduationProject.Service.IService
{
    public interface IControlService
    {
        Task<bool> RaisingGradesSemesterAsync(int semesterId);
        Task<bool> RaisingGradesCourseAsync(int courseId);
        Task<List<GetAllSemesterCurrentDto>> GetAllSemesterCurrentAsync();
    }
}
