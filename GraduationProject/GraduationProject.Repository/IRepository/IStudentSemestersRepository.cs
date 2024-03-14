using GraduationProject.Data.Entity;

namespace GraduationProject.Repository.IRepository
{
    public interface IStudentSemestersRepository : IGeneralRepository<StudentSemester>
    {
        Task Test(int courseId);
    }
}
