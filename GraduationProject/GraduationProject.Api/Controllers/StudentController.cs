using GraduationProject.Service.DataTransferObject.StudentDto;
using GraduationProject.Service.DataTransferObject.StudentSemester;
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
        [HttpPost]
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
        [HttpPost("{studentId:int}")]
        public async Task<IActionResult> AddStudentSemester([FromRoute] int studentId, [FromBody] AddStudentSemesterDto addStudentSemesterDto)
        {
            if (studentId != addStudentSemesterDto.StudentId)
            {
                return BadRequest("Please Enter Vaild StudentId");
            }
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
    }
}
