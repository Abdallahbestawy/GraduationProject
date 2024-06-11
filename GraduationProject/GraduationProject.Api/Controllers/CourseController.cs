using GraduationProject.Identity.Enum;
using GraduationProject.Service.DataTransferObject.CourseDto;
using GraduationProject.Service.IService;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = nameof(UserType.Administration) + "," + nameof(UserType.Teacher) + "," + nameof(UserType.TeacherAssistant) + "," + nameof(UserType.ControlMembers))]
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
        [Authorize(Roles = nameof(UserType.Administration))]
        [HttpGet("All/{facultId:int}")]
        public async Task<IActionResult> GetCourses(int facultId)
        {
            var response = await _courseService.GetCoursesAsync(facultId);

            return StatusCode(response.StatusCode, response);
        }
        [Authorize(Roles = nameof(UserType.Administration))]
        [HttpPost("AddCourse")]
        public async Task<IActionResult> AddCourse(CourseDto addCourseDto)
        {
            var response = await _courseService.AddCourseAsync(addCourseDto);

            return StatusCode(response.StatusCode, response);
        }
        [Authorize(Roles = nameof(UserType.Administration))]
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateCourse([FromBody] CourseDto updateCourseDto)
        {
            if (updateCourseDto.Id == null)
            {
                return BadRequest("Please Enter Valid Model");
            }
            var response = await _courseService.UpdateCourseAsync(updateCourseDto);

            return StatusCode(response.StatusCode, response);
        }
        [Authorize(Roles = nameof(UserType.Administration))]
        [HttpDelete("{Id:int}")]
        public async Task<IActionResult> DeleteCourse(int Id)
        {
            var response = await _courseService.DeleteCourseAsync(Id);

            return StatusCode(response.StatusCode, response);
        }
        [Authorize(Roles = nameof(UserType.Administration))]
        [HttpPost("AddCourseAssessMethod")]
        public async Task<IActionResult> AddCourseAssessMethod(CourseAssessMethodDto addCourseAssessMethodDto)
        {
            var response = await _courseService.AddCourseAssessMethodAsync(addCourseAssessMethodDto);

            return StatusCode(response.StatusCode, response);
        }
        [Authorize(Roles = nameof(UserType.Teacher) + "," + nameof(UserType.TeacherAssistant) + "," + nameof(UserType.ControlMembers))]
        [HttpGet("GetStudentSemesterAssessMethodsBySpecificCourse/{courseId:int}")]
        public async Task<IActionResult> GetStudentSemesterAssessMethodsBySpecificCourse(int courseId)
        {
            var response = await _courseService.GetStudentSemesterAssessMethodsBySpecificCourseAndControlStatus(courseId, false);

            return StatusCode(response.StatusCode, response);
        }
        [Authorize(Roles = nameof(UserType.Administration))]
        [HttpGet("CoursePrerequisite/{courseId:int}")]
        public async Task<IActionResult> GetCoursePrerequisite(int courseId)
        {
            var response = await _courseService.GetCoursePrerequisiteAsync(courseId);

            return StatusCode(response.StatusCode, response);
        }
        [Authorize(Roles = nameof(UserType.Teacher) + "," + nameof(UserType.ControlMembers))]
        [HttpPut("EditDegree")]
        public async Task<IActionResult> UpdateCourseStudentsAssessMethod(List<UpdateCourseStudentsAssessMethodDto> updateCourseStudentsAssessMethodDto)
        {
            var response = await _courseService.UpdateCourseStudentsAssessMethodAsync(updateCourseStudentsAssessMethodDto);

            return StatusCode(response.StatusCode, response);
        }
        [Authorize(Roles = nameof(UserType.Teacher) + "," + nameof(UserType.TeacherAssistant))]
        [HttpGet("SCInfo/{courseId:int}")]
        public async Task<IActionResult> GetStudentCourseInfo(int courseId)
        {
            var response = await _courseService.GetStudentCourseInfoAsync(courseId);

            return StatusCode(response.StatusCode, response);
        }
        [Authorize(Roles = nameof(UserType.Teacher) + "," + nameof(UserType.TeacherAssistant))]
        [HttpPut("UCInfo")]
        public async Task<IActionResult> UpdateStudentCourseInfo(List<UpdateStudentCourseInfoDto> updateStudentCourseInfoDtos)
        {
            var response = await _courseService.UpdateStudentCourseInfoAsync(updateStudentCourseInfoDtos);

            return StatusCode(response.StatusCode, response);
        }
        [Authorize(Roles = nameof(UserType.Administration))]
        [HttpDelete("CoursePrerequisites/{Id:int}")]
        public async Task<IActionResult> DeleteCoursePrerequisites(int Id)
        {
            var response = await _courseService.DeleteCoursePrerequisitesAsync(Id);

            return StatusCode(response.StatusCode, response);
        }
        [Authorize(Roles = nameof(UserType.Administration))]
        [HttpGet("CourseAssessMethod/{courseId:int}")]
        public async Task<IActionResult> GetCourseAssessMethod(int courseId)
        {
            var response = await _courseService.GetCourseAssessMethodAsync(courseId);

            return StatusCode(response.StatusCode, response);
        }
        [Authorize(Roles = nameof(UserType.Administration))]
        [HttpDelete("CourseAssessMethod/{AssessMethodId:int}")]
        public async Task<IActionResult> DeleteCourseAssessMethod(int AssessMethodId)
        {
            var response = await _courseService.DeleteCourseAssessMethodAsync(AssessMethodId);

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("GetExcelFileForSpecificCourse/{courseId:int}/{inculdeOldData:bool}")]
        public async Task<IActionResult> GenerateExcelFileForSpecificCourseByCourseId(int courseId, bool inculdeOldData)
        {
            var response = await _courseService.GenerateExcelFileForStudentSemesterAssessMethodsBySpecificCourse(courseId, User, inculdeOldData);

            if(!response.Succeeded)
                return StatusCode(response.StatusCode, response);

            var fileName = "EduWay-AssessMethods.xlsx";
            var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            return File(response.Data.ToArray(), contentType, fileName);
        }
    }
}
