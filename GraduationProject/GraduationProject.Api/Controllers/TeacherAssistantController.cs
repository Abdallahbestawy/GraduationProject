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
    public class TeacherAssistantController : ControllerBase
    {
        private readonly ITeacherAssistantService _teacherAssistantService;
        private readonly ITeacherService _teacherService;
        public TeacherAssistantController(ITeacherAssistantService TeacherAssistantService, ITeacherService teacherService)
        {
            _teacherAssistantService = TeacherAssistantService;
            _teacherService = teacherService;
        }
        [HttpPost]
        public async Task<IActionResult> AddTeacherAssistant(AddStaffDto addTeacherAssistantDto)
        {
            if (ModelState.IsValid)
            {
                var response = await _teacherAssistantService.AddTeacherAssistantAsync(addTeacherAssistantDto);

                return StatusCode(response.StatusCode, response);
            }
            else
            {
                return BadRequest("please enter valid Model");
            }

        }
        [HttpGet("GetAllTeacherAssistant/{FacultyId:int}")]
        public async Task<IActionResult> GetAllTeacherAssistant(int FacultyId)
        {
            if (_teacherAssistantService == null)
            {
                return BadRequest();
            }

            var response = await _teacherAssistantService.GetAllTeacherAssistantsAsync(FacultyId);

            return StatusCode(response.StatusCode, response);
        }
        [HttpGet("GetAllL/{courseId:int}")]
        public async Task<IActionResult> GetAllTeacherAssistants(int courseId)
        {

            var response = await _teacherService.GetCurrentStaffByCourseIdAsync(courseId, 2);

            return StatusCode(response.StatusCode, response);
        }
    }
}