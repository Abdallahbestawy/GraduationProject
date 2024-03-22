using GraduationProject.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControlController : ControllerBase
    {
        private readonly ICourseService _courseService;
        private readonly IControlService _controlService;
        public ControlController(ICourseService courseService, IControlService controlService)
        {
            _courseService = courseService;
            _controlService = controlService;
        }

        [HttpGet("GetStudentSemesterAssessMethodsBySpecificCourseControlMembers{courseId:int}")]
        public async Task<IActionResult> GetStudentSemesterAssessMethodsBySpecificCourseControlMembers(int courseId)
        {
            var response = await _courseService.GetStudentSemesterAssessMethodsBySpecificCourseAndControlStatus(courseId, true);

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("RaisingGradesCourse{courseId:int}")]
        public async Task<IActionResult> RaisingGradesCourse(int courseId)
        {
            var response = await _controlService.RaisingGradesCourseAsync(courseId);

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("RaisingGradesSemester{semesterId:int}")]
        public async Task<IActionResult> RaisingGradesSemester(int semesterId)
        {
            var response = await _controlService.RaisingGradesSemesterAsync(semesterId);

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("GetAllSemester")]
        public async Task<IActionResult> GetAllSemester()
        {
            var response = await _controlService.GetAllSemesterCurrentAsync();

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("GetAllCourse{semesterId:int}")]
        public async Task<IActionResult> GetAllCourseBySemesterId(int semesterId)
        {
            var response = await _courseService.GetCourseBySemesterIdAsync(semesterId);

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("Test")]
        public async Task<IActionResult> Test()
        {
            await _controlService.Test();
            return Ok();
        }

    }
}
