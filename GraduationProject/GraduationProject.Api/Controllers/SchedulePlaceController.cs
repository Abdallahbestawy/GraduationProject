using GraduationProject.Service.DataTransferObject.SchedulePlaceDto;
using GraduationProject.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchedulePlaceController : ControllerBase
    {
        private readonly ISchedulePlaceService _schedulePlaceService;
        public SchedulePlaceController(ISchedulePlaceService schedulePlaceService)
        {
            _schedulePlaceService = schedulePlaceService;
        }
        [HttpGet("{Id:int}")]
        public async Task<IActionResult> GetSchedulePlaceById([FromRoute] int Id)
        {
            if (Id.Equals(null))
            {
                return BadRequest("Please Enter Id Valid");
            }
            var response = await _schedulePlaceService.GetSchedulePlaceByIdAsync(Id);

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("All/{facultyId:int}")]
        public async Task<IActionResult> GetSchedulePlaces(int facultyId)
        {
            var response = await _schedulePlaceService.GetSchedulePlaceByFacultyIdAsync(facultyId);

            return StatusCode(response.StatusCode, response);
        }
        [HttpPost]
        public async Task<IActionResult> AddSchedulePlace(SchedulePlaceDto addSchedulePlaceDto)
        {

            var response = await _schedulePlaceService.AddSchedulePlaceAsync(addSchedulePlaceDto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateSchedulePlace([FromBody] SchedulePlaceDto updateSchedulePlaceDto)
        {
            if (updateSchedulePlaceDto == null)
            {
                return BadRequest("please emter valid model");
            }
            var response = await _schedulePlaceService.UpdateSchedulePlaceAsync(updateSchedulePlaceDto);

            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("{Id:int}")]
        public async Task<IActionResult> DeleteSchedulePlace([FromRoute] int Id)
        {
            var response = await _schedulePlaceService.DeleteSchedulePlaceAsync(Id);

            return StatusCode(response.StatusCode, response);
        }
    }
}
