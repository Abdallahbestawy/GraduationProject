using GraduationProject.Service.DataTransferObject.CourseDto;
using GraduationProject.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;
        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }
        [HttpGet("{Id:int}")]
        public async Task<IActionResult> GetCourseById([FromRoute] int Id)
        {
            if (Id.Equals(null))
            {
                return BadRequest("Please Enter Id Valid");
            }
            var response = await _courseService.GetCourseByIdAsync(Id);

            return StatusCode(response.StatusCode, response);
        }
        [HttpGet]
        public async Task<IActionResult> GetCourses()
        {
            var response = await _courseService.GetCoursesAsync();

            return StatusCode(response.StatusCode, response);
        }
        [HttpPost("AddCourse")]
        public async Task<IActionResult> AddCourse(CourseDto addCourseDto)
        {
            var response = await _courseService.AddCourseAsync(addCourseDto);
            
            return StatusCode(response.StatusCode, response);
        }
        [HttpPut("{Id:int}")]
        public async Task<IActionResult> UpdateCourse([FromRoute] int Id, [FromBody] CourseDto updateCourseDto)
        {
            if (Id != updateCourseDto.Id)
            {
                return BadRequest("the Id not Valid");
            }
            var response = await _courseService.UpdateCourseAsync(updateCourseDto);

            return StatusCode(response.StatusCode, response);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteCourse([FromRoute] int Id)
        {
            var response = await _courseService.DeleteCourseAsync(Id);

            return StatusCode(response.StatusCode, response);
        }
        [HttpPost("AddCourseAssessMethod")]
        public async Task<IActionResult> AddCourseAssessMethod(CourseAssessMethodDto addCourseAssessMethodDto)
        {
            var response = await _courseService.AddCourseAssessMethodAsync(addCourseAssessMethodDto);

            return StatusCode(response.StatusCode, response);
        }
        [HttpGet("GetStudentSemesterAssessMethodsBySpecificCourse{courseId:int}")]
        public async Task<IActionResult> GetStudentSemesterAssessMethodsBySpecificCourse(int courseId)
        {
            var response = await _courseService.GetStudentSemesterAssessMethodsBySpecificCourseAndControlStatus(courseId, false);

            return StatusCode(response.StatusCode, response);
        }
        [HttpGet("GetStudentSemesterAssessMethodsBySpecificCourseControlMembers{courseId:int}")]
        public async Task<IActionResult> GetStudentSemesterAssessMethodsBySpecificCourseControlMembers(int courseId)
        {
            var response = await _courseService.GetStudentSemesterAssessMethodsBySpecificCourseAndControlStatus(courseId, true);

            return StatusCode(response.StatusCode, response);
        }
    }
}
