using GraduationProject.Service.DataTransferObject.BylawDto;
using GraduationProject.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BylawController : ControllerBase
    {
        private readonly IBylawService _bylawService;
        public BylawController(IBylawService bylawService)
        {
            _bylawService = bylawService;
        }
        [HttpGet("{Id:int}")]
        public async Task<IActionResult> GetBylawById([FromRoute] int Id)
        {
            if (Id.Equals(null))
            {
                return BadRequest("Please Enter Id Valid");
            }
            var bylaw = await _bylawService.GetBylawByIdAsync(Id);
            return Ok(bylaw);
        }
        [HttpGet]
        public async Task<IActionResult> GetBylaws()
        {
            var bylaws = await _bylawService.GetBylawAsync();
            return Ok(bylaws);
        }
        [HttpPost]
        public async Task<IActionResult> AddBylaw(BylawDto addBylawDto)
        {
            await _bylawService.AddBylawAsync(addBylawDto);
            return Ok("Add Bylaw Success");
        }
        [HttpPut("{Id:int}")]
        public async Task<IActionResult> UpdateBylaw([FromRoute] int Id, [FromBody] BylawDto updateBylawDto)
        {
            if (Id != updateBylawDto.Id)
            {
                return BadRequest("the Id not Valid");
            }
            await _bylawService.UpdateBylawAsync(updateBylawDto);
            return Ok("the update Success");
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteBylaw([FromRoute] int Id)
        {
            await _bylawService.DeleteBylawAsync(Id);
            return Ok("Delete Bylaw Success");
        }
    }
}
