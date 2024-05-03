using GraduationProject.Identity.Enum;
using GraduationProject.Service.DataTransferObject.AcademyYearDto;
using GraduationProject.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject.Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AcademyYearController : ControllerBase
    {
        private readonly IAcademyYearService _academyYearService;
        public AcademyYearController(IAcademyYearService academyYearService)
        {
            _academyYearService = academyYearService;
        }
        [Authorize(Roles = nameof(UserType.Administration))]
        [HttpGet("Get/{Id:int}")]
        public async Task<IActionResult> GetAcademyYearById([FromRoute] int Id)
        {
            if (Id.Equals(null))
            {
                return BadRequest("Please Enter Id Valid");
            }
            var response = await _academyYearService.GetAcademyYearByIdAsync(Id);

            return StatusCode(response.StatusCode, response);
        }
        [Authorize(Roles = nameof(UserType.Administration))]
        [HttpGet("All/{facultId:int}")]
        public async Task<IActionResult> GetAcademyYears(int facultId)
        {
            var response = await _academyYearService.GetAcademyYearAsync(facultId);

            return StatusCode(response.StatusCode, response);
        }
        [Authorize(Roles = nameof(UserType.Administration))]
        [HttpPost("Add")]
        public async Task<IActionResult> AddAcademyYear(AcademyYearDto addAcademyYearDto)
        {
            var response = await _academyYearService.AddAcademyYearAsync(addAcademyYearDto, User);

            return StatusCode(response.StatusCode, response);
        }
        [Authorize(Roles = nameof(UserType.Administration))]
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateAcademyYear([FromBody] AcademyYearDto updateAcademyYearDto)
        {
            if (updateAcademyYearDto == null)
            {
                return BadRequest("please enter valid model");
            }
            var response = await _academyYearService.UpdateAcademyYearAsync(updateAcademyYearDto, User);

            return StatusCode(response.StatusCode, response);
        }
        [Authorize(Roles = nameof(UserType.Administration))]
        [HttpDelete("Delete/{Id}")]
        public async Task<IActionResult> DeleteAcademyYear([FromRoute] int Id)
        {
            var response = await _academyYearService.DeleteAcademyYearAsync(Id, User);

            return StatusCode(response.StatusCode, response);
        }
        [Authorize(Roles = nameof(UserType.Administration))]
        [HttpGet("GetCurrent/{facultId:int}")]
        public async Task<IActionResult> GetCurrentAcademyYear(int facultId)
        {
            var response = await _academyYearService.GetCurrentAcademyYearAsync(facultId);
            if (response != null)
            {
                return Ok(response);
            }
            return BadRequest("Not Get Current AcademyYear Exist");
        }
        [Authorize(Roles = nameof(UserType.Administration) + "," + nameof(UserType.ControlMembers))]
        [HttpGet("N")]
        public async Task<IActionResult> GetAcademyYearName()
        {
            var response = await _academyYearService.GetAcademyYearNameAsync();

            return StatusCode(response.StatusCode, response);
        }
    }
}
