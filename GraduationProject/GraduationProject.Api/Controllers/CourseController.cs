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
            var course = await _courseService.GetCourseByIdAsync(Id);
            return Ok(course);
        }
        [HttpGet]
        public async Task<IActionResult> GetCourses()
        {
            var courses = await _courseService.GetCoursesAsync();
            return Ok(courses);
        }
        [HttpPost]
        public async Task<IActionResult> AddCourse(CourseDto addCourseDto)
        {
            await _courseService.AddCourseAsync(addCourseDto);
            return Ok("Add Course Success");
        }
        [HttpPut("{Id:int}")]
        public async Task<IActionResult> UpdateCourse([FromRoute] int Id, [FromBody] CourseDto updateCourseDto)
        {
            if (Id != updateCourseDto.Id)
            {
                return BadRequest("the Id not Valid");
            }
            await _courseService.UpdateCourseAsync(updateCourseDto);
            return Ok("the update Success");
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteCourse([FromRoute] int Id)
        {
            await _courseService.DeleteCourseAsync(Id);
            return Ok("Delete Course Success");
        }
    }
}
