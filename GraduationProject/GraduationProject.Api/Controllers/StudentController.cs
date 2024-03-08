using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.StudentDto;
using GraduationProject.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }
        [HttpPost("AddStudent")]
        public async Task<IActionResult> AddStudent(AddStudentDto addStudentDto)
        {
            if (ModelState.IsValid)
            {
                int raw = await _studentService.AddStudentAsync(addStudentDto);
                if (raw == 1)
                {
                    return Ok("StudentAdd");
                }
                else
                {
                    return BadRequest("please enter valid Model");
                }
            }
            else
            {
                return BadRequest("please enter valid Model");
            }
        }
        [HttpPost("AddStudentSemester")]
        public async Task<IActionResult> AddStudentSemester([FromBody] AddStudentSemesterDto addStudentSemesterDto)
        {
            if (ModelState.IsValid)
            {
                await _studentService.AddStudentSemesterAsync(addStudentSemesterDto);
                return Ok("Add Success");
            }
            else
            {
                return BadRequest("please enter Vaild Model");
            }
        }
        [HttpGet("GetStudent")]
        public async Task<IActionResult> GetStudent()
        {
            string userId = "af88e91d-7241-4149-bbd4-ebb2a30dd247";

            var response = await _studentService.GetStudentByUserId(userId);

            if (response.StatusCode == 500)
                return StatusCode(response.StatusCode, response);

            return Ok(response);
        }
    }
}
