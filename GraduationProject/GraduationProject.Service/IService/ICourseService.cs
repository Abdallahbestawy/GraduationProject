using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.CourseDto;

namespace GraduationProject.Service.IService
{
    public interface ICourseService
    {
        Task<Response<int>> AddCourseAsync(CourseDto addCourseDto);
        Task<Response<int>> UpdateCourseAsync(CourseDto updateCourseDto);
        Task<Response<int>> DeleteCourseAsync(int CourseId);
        Task<Response<CourseDto>> GetCourseByIdAsync(int CourseId);
        Task<Response<IQueryable<CourseDto>>> GetCoursesAsync();
        Task<Response<int>> AddCourseAssessMethodAsync(CourseAssessMethodDto addCourseAssessMethodDto);
        Task<IQueryable<CourseDto>> GetCoursesByScientificDegreeIdAsync(int scientificDegreeId);
        Task<CourseAssessMethodDto> GetAssessMethodsByCoursesIdAsync(int courseId);
        Task<Response<CourseStudentsAssessMethodDto>> GetStudentSemesterAssessMethodsBySpecificCourseAndControlStatus(int courseId, bool isControlStatus);
    }
}
