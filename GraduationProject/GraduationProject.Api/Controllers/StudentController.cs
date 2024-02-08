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
    }
}
