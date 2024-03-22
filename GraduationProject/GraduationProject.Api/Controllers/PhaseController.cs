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
            var response = await _phaseService.GetPhaseByIdAsync(Id);
            
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetPhases()
        {
            var response = await _phaseService.GetPhaseAsync();

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("ByFacultyId/{facultyId:int}")]
        public async Task<IActionResult> GetPhasesByFacultyId(int facultyId)
        {
            var response = await _phaseService.GetPhaseByFacultyIdAsync(facultyId);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> AddPhase(PhaseDto addPhaseDto)
        {
            var response = await _phaseService.AddPhaseAsync(addPhaseDto);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("{Id:int}")]
        public async Task<IActionResult> UpdatePhase([FromRoute] int Id, [FromBody] PhaseDto updatePhaseDto)
        {
            if (Id != updatePhaseDto.Id)
            {
                return BadRequest("the Id not Valid");
            }
            var response = await _phaseService.UpdatePhaseAsync(updatePhaseDto);

            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePhase([FromRoute] int Id)
        {
            var response = await _phaseService.DeletePhaseAsync(Id);
            
            return StatusCode(response.StatusCode, response);
        }

    }
}
