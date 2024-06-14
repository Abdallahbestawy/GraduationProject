using GraduationProject.Identity.Enum;
using GraduationProject.Service.DataTransferObject.StaffDto;
using GraduationProject.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject.Api.Controllers
{
    [Authorize(Roles = nameof(UserType.Administration))]
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
        [HttpGet("GetAllTeacher/{FacultyId:int}")]
        public async Task<IActionResult> GetAllTeacher(int FacultyId)
        {

            var response = await _teacherService.GetAllTeachersAsync(FacultyId);

            return StatusCode(response.StatusCode, response);
        }
        [HttpGet("GetAllL/{courseId:int}")]
        public async Task<IActionResult> GetAllTeacherAssistants(int courseId)
        {

            var response = await _teacherService.GetCurrentStaffByCourseIdAsync(courseId, 1);

            return StatusCode(response.StatusCode, response);
        }

    }
}
