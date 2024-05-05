using GraduationProject.Data.Entity;

namespace GraduationProject.Repository.IRepository
{
    public interface IStudentSemestersRepository : IGeneralRepository<StudentSemester>
    {
        Task<bool> RaisingGradesCourseAsync(int courseId);
        Task<bool> RaisingGradesSemesterAsync(int SemesterId);
        Task<List<StudentSemester>> GetAllSemesterCurrentAsync();
        Task<List<StudentSemester>> GetAllSemesterActiveAsync(int academyYearId);
        Task<(List<StudentSemester>, List<StudentSemester>, bool)> EndSemesterAsync(int scientificDegreeId);
        Task<List<object>> GetTheCurrentSemesterWithStudents();
        Task<List<StudentSemester>> GetAllAcdemyYearGraduatesAsync();

    }
}
