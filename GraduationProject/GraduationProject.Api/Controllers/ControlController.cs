using GraduationProject.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControlController : ControllerBase
    {
        private readonly ICourseService _courseService;
        public ControlController(ICourseService courseService)
        {
            _courseService = courseService;
        }
        [HttpGet("GetStudentSemesterAssessMethodsBySpecificCourseControlMembers{courseId:int}")]
        public async Task<IActionResult> GetStudentSemesterAssessMethodsBySpecificCourseControlMembers(int courseId)
        {
            var response = await _courseService.GetStudentSemesterAssessMethodsBySpecificCourseAndControlStatus(courseId, true);

            return StatusCode(response.StatusCode, response);
        }
    }
}
