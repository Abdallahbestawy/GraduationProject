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
            var band = await _bandService.GetBandByIdAsync(Id);
            return Ok(band);
        }
        [HttpGet]
        public async Task<IActionResult> GetBands()
        {
            var bands = await _bandService.GetBandAsync();
            return Ok(bands);
        }
        [HttpPost]
        public async Task<IActionResult> AddBand(BandDto addBandDto)
        {
            await _bandService.AddBandAsync(addBandDto);
            return Ok("Add Band Success");
        }
        [HttpPut("{Id:int}")]
        public async Task<IActionResult> UpdateBand([FromRoute] int Id, [FromBody] BandDto updateBandDto)
        {
            if (Id != updateBandDto.Id)
            {
                return BadRequest("the Id not Valid");
            }
            await _bandService.UpdateBandAsync(updateBandDto);
            return Ok("the update Success");
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteBand([FromRoute] int Id)
        {
            await _bandService.DeleteBandAsync(Id);
            return Ok("Delete Band Success");
        }
    }
}
