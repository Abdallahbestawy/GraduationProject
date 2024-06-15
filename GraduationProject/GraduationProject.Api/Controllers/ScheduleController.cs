using GraduationProject.Identity.Enum;
using GraduationProject.Identity.IService;
using GraduationProject.Service.DataTransferObject.ScheduleDto;
using GraduationProject.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleIService _scheduleIService;
        private readonly IAccountService _accountService;
        public ScheduleController(IScheduleIService scheduleIService, IAccountService accountService = null)
        {
            _scheduleIService = scheduleIService;
            _accountService = accountService;
        }
        [Authorize(Roles = nameof(UserType.Administration))]
        [HttpPost]
        public async Task<IActionResult> AddSchedule(ScheduleDto addScheduleDto)
        {
            var response = await _scheduleIService.AddScheduleAsync(addScheduleDto);

            return StatusCode(response.StatusCode, response);
        }
        [Authorize(Roles = nameof(UserType.Teacher) + "," + nameof(UserType.TeacherAssistant))]
        [HttpGet("T")]
        public async Task<IActionResult> GetSchedulesForStaffByUserId()
        {
            var currentUser = await _accountService.GetUser(User);
            if (currentUser == null)
            {
                return Unauthorized();
            }
            var response = await _scheduleIService.GetSchedulesForStaffByUserIdAsync(currentUser.Id);

            return StatusCode(response.StatusCode, response);
        }
        [Authorize(Roles = nameof(UserType.Administration))]
        [HttpGet("All/{factlyId:int}/{semesterId:int}")]
        public async Task<IActionResult> GetScheduleBySemesterId(int factlyId, int semesterId)
        {
            var response = await _scheduleIService.GetScheduleBySemesterIdAsync(semesterId, factlyId);

            return StatusCode(response.StatusCode, response);
        }
        [Authorize(Roles = nameof(UserType.Administration))]
        [HttpPut]
        public async Task<IActionResult> UpdateSchedule(ScheduleDto updateScheduleDto)
        {
            var response = await _scheduleIService.UpdateScheduleAsync(updateScheduleDto);

            return StatusCode(response.StatusCode, response);
        }
        [Authorize(Roles = nameof(UserType.Administration))]
        [HttpDelete("{Id:int}")]
        public async Task<IActionResult> DeleteSchedule([FromRoute] int Id)
        {
            var response = await _scheduleIService.DeleteScheduleAsync(Id);

            return StatusCode(response.StatusCode, response);
        }
    }
}
