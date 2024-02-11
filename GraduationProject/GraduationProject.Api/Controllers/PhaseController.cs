using GraduationProject.Service.DataTransferObject.PhaseDto;
using GraduationProject.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhaseController : ControllerBase
    {
        private readonly IPhaseService _phaseService;
        public PhaseController(IPhaseService phaseService)
        {
            _phaseService = phaseService;
        }
        [HttpGet("{Id:int}")]
        public async Task<IActionResult> GetPhaseById([FromRoute] int Id)
        {
            if (Id.Equals(null))
            {
                return BadRequest("Please Enter Id Valid");
            }
            var phase = await _phaseService.GetPhaseByIdAsync(Id);
            return Ok(phase);
        }
        [HttpGet]
        public async Task<IActionResult> GetPhases()
        {
            var phases = await _phaseService.GetPhaseAsync();
            return Ok(phases);
        }
        [HttpPost]
        public async Task<IActionResult> AddPhase(PhaseDto addPhaseDto)
        {
            await _phaseService.AddPhaseAsync(addPhaseDto);
            return Ok("Add Phase Success");
        }
        [HttpPut("{Id:int}")]
        public async Task<IActionResult> UpdatePhase([FromRoute] int Id, [FromBody] PhaseDto updatePhaseDto)
        {
            if (Id != updatePhaseDto.Id)
            {
                return BadRequest("the Id not Valid");
            }
            await _phaseService.UpdatePhaseAsync(updatePhaseDto);
            return Ok("the update Success");
        }
        [HttpDelete]
        public async Task<IActionResult> DeletePhase([FromRoute] int Id)
        {
            await _phaseService.DeletePhaseAsync(Id);
            return Ok("Delete Phase Success");
        }

    }
}
