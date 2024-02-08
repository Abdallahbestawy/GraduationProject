using GraduationProject.Service.DataTransferObject.StaffDto;
using GraduationProject.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherAssistantController : ControllerBase
    {
        private readonly ITeacherAssistantService _TeacherAssistantService;
        public TeacherAssistantController(ITeacherAssistantService TeacherAssistantService)
        {
            _TeacherAssistantService = _TeacherAssistantService;
        }
        [HttpPost]
        public async Task<IActionResult> AddTeacherAssistant(AddStaffDto addTeacherAssistantDto)
        {
            if (ModelState.IsValid)
            {
                int raw = await _TeacherAssistantService.AddTeacherAssistantAsync(addTeacherAssistantDto);
                if (raw == 1)
                {
                    return Ok("TeacherAssistant Add");
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
