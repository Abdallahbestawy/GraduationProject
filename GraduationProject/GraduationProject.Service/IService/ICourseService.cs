using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.CourseDto;

namespace GraduationProject.Service.IService
{
    public interface ICourseService
    {
        Task<Response<int>> AddCourseAsync(CourseDto addCourseDto);
        Task<Response<int>> UpdateCourseAsync(CourseDto updateCourseDto);
        Task<Response<int>> DeleteCourseAsync(int CourseId);
        Task<Response<GetCourseByIdDto>> GetCourseByIdAsync(int CourseId);
        Task<Response<IQueryable<CourseDto>>> GetCoursesAsync(int facultId);
        Task<Response<int>> AddCourseAssessMethodAsync(CourseAssessMethodDto addCourseAssessMethodDto);
        Task<IQueryable<CourseDto>> GetCoursesByScientificDegreeIdAsync(int scientificDegreeId);
        Task<CourseAssessMethodDto> GetAssessMethodsByCoursesIdAsync(int courseId);
        Task<Response<List<GetCourseDto>>> GetCourseBySemesterIdAsync(int semesterId);
        Task<Response<List<GetCourseDto>>> GetCourseMandatoryByfacultIdAsync(int facultId);
        Task<Response<List<GetCourseDto>>> GetCoursePrerequisiteAsync(int courseId);
        Task<Response<bool>> UpdateCourseStudentsAssessMethodAsync(List<UpdateCourseStudentsAssessMethodDto> updateCourseStudentsAssessMethodDto);
        Task<Response<CourseStudentsAssessMethodDto>> GetStudentSemesterAssessMethodsBySpecificCourseAndControlStatus(int courseId, bool isControlStatus);
        Task<Response<GetStudentCourseInfoDto>> GetStudentCourseInfoAsync(int courseId);
        Task<Response<bool>> UpdateStudentCourseInfoAsync(List<UpdateStudentCourseInfoDto> updateStudentCourseInfoDtos);
        Task<Response<int>> DeleteCoursePrerequisitesAsync(int coursePrerequisitesId);
        Task<Response<GetCourseAssessMethodDto>> GetCourseAssessMethodAsync(int courseId);
        Task<Response<bool>> DeleteCourseAssessMethodAsync(int AssessMethodId);
    }
}
