using GraduationProject.Identity.Enum;
using GraduationProject.Service.DataTransferObject.BandDto;
using GraduationProject.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject.Api.Controllers
{
    //[Authorize(Roles = nameof(UserType.Administration))]
    [Route("api/[controller]")]
    [ApiController]
    public class BandController : ControllerBase
    {
        private readonly IBandService _bandService;
        public BandController(IBandService bandService)
        {
            _bandService = bandService;
        }

        [HttpGet("{Id:int}")]
        public async Task<IActionResult> GetBandById([FromRoute] int Id)
        {
            if (Id.Equals(null))
            {
                return BadRequest("Please Enter Id Valid");
            }
            var response = await _bandService.GetBandByIdAsync(Id);

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("All/{facultyId:int}")]
        public async Task<IActionResult> GetBands(int facultyId)
        {
            var response = await _bandService.GetBandByFacultyIdAsync(facultyId);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> AddBand(BandDto addBandDto)
        {

            var response = await _bandService.AddBandAsync(addBandDto, User);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateBand([FromBody] BandDto updateBandDto)
        {
            if (updateBandDto == null)
            {
                return BadRequest("please emter valid model");
            }
            var response = await _bandService.UpdateBandAsync(updateBandDto,User);

            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("{Id:int}")]
        public async Task<IActionResult> DeleteBand([FromRoute] int Id)
        {
            var response = await _bandService.DeleteBandAsync(Id, User);

            return StatusCode(response.StatusCode, response);
        }
    }
}
