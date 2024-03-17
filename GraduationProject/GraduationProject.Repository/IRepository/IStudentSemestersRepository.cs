using GraduationProject.Data.Entity;

namespace GraduationProject.Repository.IRepository
{
    public interface IStudentSemestersRepository : IGeneralRepository<StudentSemester>
    {
        // Task semTest(List<int> courseId);
        Task<bool> RaisingGradesCourseAsync(int courseId);
        Task<bool> RaisingGradesSemesterAsync(int SemesterId);
        Task<List<StudentSemester>> GetAllSemesterCurrentAsync();

    }
}
