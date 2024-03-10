using GraduationProject.Service.DataTransferObject.StaffDto;
using GraduationProject.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherAssistantController : ControllerBase
    {
        private readonly ITeacherAssistantService _teacherAssistantService;
        public TeacherAssistantController(ITeacherAssistantService TeacherAssistantService)
        {
            _teacherAssistantService = TeacherAssistantService;
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
        [HttpGet("GetAllTeacherAssistant")]
        public async Task<IActionResult> GetAllTeacherAssistant()
        {
            if (_teacherAssistantService == null)
            {
                return BadRequest();
            }

            var response = await _teacherAssistantService.GetAllTeacherAssistantsAsync();

            return StatusCode(response.StatusCode, response);
        }
    }
}