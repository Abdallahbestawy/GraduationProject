using GraduationProject.Service.DataTransferObject.StaffDto;
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
        private readonly IStudentService _studentService;
        public ControlController(ICourseService courseService, IControlService controlService, IStudentService studentService)
        {
            _courseService = courseService;
            _controlService = controlService;
            _studentService = studentService;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddControlMembers(AddStaffDto addControlMembersDto)
        {
            if (ModelState.IsValid)
            {
                var response = await _controlService.AddControlMembersAsync(addControlMembersDto);

                return StatusCode(response.StatusCode, response);
            }
            else
            {
                return BadRequest("please enter valid Model");
            }

        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllControlMember()
        {
            var response = await _controlService.GetAllControlMembersAsync();

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("GetStudentSemesterAssessMethodsBySpecificCourseControlMembers{courseId:int}")]
        public async Task<IActionResult> GetStudentSemesterAssessMethodsBySpecificCourseControlMembers(int courseId)
        {
            var response = await _courseService.GetStudentSemesterAssessMethodsBySpecificCourseAndControlStatus(courseId, true);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("RaisingGradesCourse{courseId:int}")]
        public async Task<IActionResult> RaisingGradesCourse(int courseId)
        {
            var response = await _controlService.RaisingGradesCourseAsync(courseId);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("RaisingGradesSemester{semesterId:int}")]
        public async Task<IActionResult> RaisingGradesSemester(int semesterId)
        {
            var response = await _controlService.RaisingGradesSemesterAsync(semesterId);

            return StatusCode(response.StatusCode, response);
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

        [HttpPost("EndSemester{semesterId:int}")]
        public async Task<IActionResult> EndSemester([FromRoute] int semesterId)
        {
            var response = await _controlService.EndSemesterAsync(semesterId);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("AssignCourses")]
        public async Task<IActionResult> AssignCoursesToStudents()
        {
            var response = await _studentService.AssignCoursesToStudents();

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("SA{academyYearId:int}")]
        public async Task<IActionResult> GetAllSemesterActive(int academyYearId)
        {
            var response = await _controlService.GetAllSemesterActiveAsync(academyYearId);

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("SSR{semesterId:int},{acedemyYearId:int}")]
        public async Task<IActionResult> GetStudentsSemesterResult(int semesterId, int acedemyYearId)
        {
            var response = await _controlService.GetStudentsSemesterResultAsync(semesterId, acedemyYearId);

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("SISR{studentSemesterId:int}")]
        public async Task<IActionResult> GetStudentInSemesterResul(int studentSemesterId)
        {
            var response = await _controlService.GetStudentInSemesterResulAsync(studentSemesterId);

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("SISC{semesterId:int},{acedemyYearId:int},{courseId:int}")]
        public async Task<IActionResult> GetAllStudentInCourseResult(int semesterId, int acedemyYearId, int courseId)
        {
            var response = await _controlService.GetAllStudentInCourseResultAsync(semesterId, acedemyYearId, courseId);

            return StatusCode(response.StatusCode, response);
        }

    }
}
