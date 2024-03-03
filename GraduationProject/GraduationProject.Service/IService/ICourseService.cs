using GraduationProject.Service.DataTransferObject.CourseDto;

namespace GraduationProject.Service.IService
{
    public interface ICourseService
    {
        Task AddCourseAsync(CourseDto addCourseDto);
        Task UpdateCourseAsync(CourseDto updateCourseDto);
        Task DeleteCourseAsync(int CourseId);
        Task<CourseDto> GetCourseByIdAsync(int CourseId);
        Task<IQueryable<CourseDto>> GetCoursesAsync();
        Task AddCourseAssessMethodAsync(CourseAssessMethodDto addCourseAssessMethodDto);
        Task<IQueryable<CourseDto>> GetCoursesByScientificDegreeIdAsync(int scientificDegreeId);
        Task<CourseAssessMethodDto> GetAssessMethodsByCoursesIdAsync(int courseId);
        Task GetTest();
    }
}
