using GraduationProject.Data.Entity;

namespace GraduationProject.Repository.IRepository
{
    public interface IStudentSemesterAssessMethodRepository : IGeneralRepository<StudentSemesterAssessMethod>
    {
        Task<IQueryable<StudentSemesterAssessMethod>> GetStudentSemesterAssessMethods(int courseId);
    }
}
