using GraduationProject.Service.DataTransferObject.BandDto;
using GraduationProject.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject.Api.Controllers
{
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

        [HttpGet]
        public async Task<IActionResult> GetBands()
        {
            var response = await _bandService.GetBandAsync();

            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> AddBand(BandDto addBandDto)
        {
            var response = await _bandService.AddBandAsync(addBandDto);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("{Id:int}")]
        public async Task<IActionResult> UpdateBand([FromRoute] int Id, [FromBody] BandDto updateBandDto)
        {
            if (Id != updateBandDto.Id)
            {
                return BadRequest("the Id not Valid");
            }
            var response = await _bandService.UpdateBandAsync(updateBandDto);

            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBand([FromRoute] int Id)
        {
            var response = await _bandService.DeleteBandAsync(Id);
            return StatusCode(response.StatusCode, response);
        }
    }
}
