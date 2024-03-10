using GraduationProject.Service.DataTransferObject.StaffDto;
using GraduationProject.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherService _TeacherService;
        public TeacherController(ITeacherService TeacherService)
        {
            _TeacherService = TeacherService;
        }
        [HttpPost]
        public async Task<IActionResult> AddTeacher(AddStaffDto addTeacherDto)
        {
            if (ModelState.IsValid)
            {
                int raw = await _TeacherService.AddTeacheAsync(addTeacherDto);
                if (raw == 1)
                {
                    return Ok("Teacher Add");
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
        [HttpGet("GetAllTeacher")]
        public async Task<IActionResult> GetAllTeacher()
        {

            var response = await _TeacherService.GetAllTeachersAsync();
            if (response != null)
            {
                return Ok(response);
            }
            else
            {
                return NotFound("There are not Teacher");
            }
        }
    }
}
