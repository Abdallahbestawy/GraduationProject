using GraduationProject.Service.DataTransferObject.StaffDto;
using GraduationProject.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherService _teacherService;
        public TeacherController(ITeacherService TeacherService)
        {
            _teacherService = TeacherService;
        }
        [HttpPost]
        public async Task<IActionResult> AddTeacher(AddStaffDto addTeacherDto)
        {
            if (ModelState.IsValid)
            {
                var response = await _teacherService.AddTeacheAsync(addTeacherDto);

                return StatusCode(response.StatusCode, response);
            }
            else
            {
                return BadRequest("please enter valid Model");
            }

        }
        [HttpGet("GetAllTeacher")]
        public async Task<IActionResult> GetAllTeacher()
        {

            var response = await _teacherService.GetAllTeachersAsync();

            return StatusCode(response.StatusCode, response);
        }
    }
}
