using GraduationProject.Data.Entity;
using GraduationProject.Data.Models;

namespace GraduationProject.Repository.IRepository
{
    public interface IStudentSemesterAssessMethodRepository : IGeneralRepository<StudentSemesterAssessMethod>
    {
        Task<IEnumerable<SpGetStudentSemesterAssessMethodsBySpecificCourseAndControlStatus>> GetStudentSemesterAssessMethods(int courseId, bool isControlStatus);
    }
}
