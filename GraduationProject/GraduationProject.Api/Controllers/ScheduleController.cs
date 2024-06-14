using GraduationProject.Service.DataTransferObject.ScheduleDto;
using GraduationProject.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleIService _scheduleIService;
        public ScheduleController(IScheduleIService scheduleIService)
        {
            _scheduleIService = scheduleIService;
        }
        [HttpPost]
        public async Task<IActionResult> AddSchedule(ScheduleDto addScheduleDto)
        {

            var response = await _scheduleIService.AddScheduleAsync(addScheduleDto);
            return Ok();
            //return StatusCode(response.StatusCode, response);
        }
        [HttpGet("T")]
        public async Task<IActionResult> GetSchedulesForStaffByUserId()
        {
            string userId = "63d3ab54-6da1-429d-b8f7-7f9e56fa75fc";
            var response = await _scheduleIService.GetSchedulesForStaffByUserIdAsync(userId);
            return Ok(response);
        }
        [HttpGet("All/{factlyId:int}/{semesterId:int}")]
        public async Task<IActionResult> GetScheduleBySemesterId(int factlyId, int semesterId)
        {
            var response = await _scheduleIService.GetScheduleBySemesterIdAsync(semesterId, factlyId);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateSchedule(ScheduleDto updateScheduleDto)
        {

            var response = await _scheduleIService.UpdateScheduleAsync(updateScheduleDto);
            if (response == 0)
            {
                return BadRequest();
            }
            return Ok();
            //return StatusCode(response.StatusCode, response);
        }
        [HttpDelete("{Id:int}")]
        public async Task<IActionResult> DeleteSchedule([FromRoute] int Id)
        {
            var response = await _scheduleIService.DeleteScheduleAsync(Id);
            return Ok();

            //return StatusCode(response.StatusCode, response);
        }
    }
}
