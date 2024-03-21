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
            bool response = await _controlService.RaisingGradesCourseAsync(courseId);
            if (response)
            {
                return Ok("the Raising Grades Course Success");
            }
            return BadRequest();
        }
        [HttpGet("RaisingGradesSemester{semesterId:int}")]
        public async Task<IActionResult> RaisingGradesSemester(int semesterId)
        {
            bool response = await _controlService.RaisingGradesSemesterAsync(semesterId);
            if (response)
            {
                return Ok("the Raising Grades Semester Success");
            }
            return BadRequest();
        }
        [HttpGet("GetAllSemester")]
        public async Task<IActionResult> GetAllSemester()
        {
            var response = await _controlService.GetAllSemesterCurrentAsync();
            if (response != null)
            {
                return Ok(response);
            }
            return NotFound("Not Semester Exixt");
        }
        [HttpGet("GetAllCourse{semesterId:int}")]
        public async Task<IActionResult> GetAllCourseBySemesterId(int semesterId)
        {
            var response = await _courseService.GetCourseBySemesterIdAsync(semesterId);
            if (response != null)
            {
                return Ok(response);
            }
            return NotFound("Not Course in Semester Exixt");
        }
        [HttpGet("Test")]
        public async Task<IActionResult> Test()
        {
            await _controlService.Test();
            return Ok();
        }

    }
}
