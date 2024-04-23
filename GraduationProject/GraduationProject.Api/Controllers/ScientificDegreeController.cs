using GraduationProject.Service.DataTransferObject.ScientificDegreeDto;
using GraduationProject.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScientificDegreeController : ControllerBase
    {
        private readonly IScientificDegreeService _scientificDegreeService;
        public ScientificDegreeController(IScientificDegreeService scientificDegreeService)
        {
            _scientificDegreeService = scientificDegreeService;
        }

        [HttpGet("{Id:int}")]
        public async Task<IActionResult> GetScientificDegreeById([FromRoute] int Id)
        {
            if (Id.Equals(null))
            {
                return BadRequest("Please Enter Id Valid");
            }
            var response = await _scientificDegreeService.GetScientificDegreeByIdAsync(Id);

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetScientificDegrees()
        {
            var response = await _scientificDegreeService.GetScientificDegreeAsync();

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("ByBylawId")]
        public async Task<IActionResult> GetScientificDegrees([FromQuery] int bylawId, [FromQuery] int type)
        {
            var response = await _scientificDegreeService.GetScientificDegreeByBylawIdForSpecificTypeAsync(bylawId, type);

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("GetAllSemesters")]
        public async Task<IActionResult> GetAllSemesters()
        {
            var response = await _scientificDegreeService.GetSemsetersByBylawIdAsync();

            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> AddScientificDegrees(ScientificDegreeDto addScientificDegreesDto)
        {
            var response = await _scientificDegreeService.AddScientificDegreeAsync(addScientificDegreesDto);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateScientificDegrees([FromBody] ScientificDegreeDto updateScientificDegreesDto)
        {
            if (updateScientificDegreesDto == null)
            {
                return BadRequest("Please Enter Valid Model");
            }
            var response = await _scientificDegreeService.UpdateScientificDegreeAsync(updateScientificDegreesDto);

            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteScientificDegrees([FromRoute] int Id)
        {
            var response = await _scientificDegreeService.DeleteScientificDegreeAsync(Id);

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("GetDetails{Id:int}")]
        public async Task<IActionResult> GetDetails(int Id)
        {
            var response = await _scientificDegreeService.GetDetailsByParentIdAsync(Id);

            return StatusCode(response.StatusCode, response);
        }
    }
}
