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
            var scientificDegree = await _scientificDegreeService.GetScientificDegreeByIdAsync(Id);
            return Ok(scientificDegree);
        }
        [HttpGet]
        public async Task<IActionResult> GetScientificDegrees()
        {
            var scientificDegree = await _scientificDegreeService.GetScientificDegreeAsync();
            return Ok(scientificDegree);
        }
        [HttpPost]
        public async Task<IActionResult> AddScientificDegrees(ScientificDegreeDto addScientificDegreesDto)
        {
            await _scientificDegreeService.AddScientificDegreeAsync(addScientificDegreesDto);
            return Ok("Add ScientificDegrees Success");
        }
        [HttpPut("{Id:int}")]
        public async Task<IActionResult> UpdateScientificDegrees([FromRoute] int Id, [FromBody] ScientificDegreeDto updateScientificDegreesDto)
        {
            if (Id != updateScientificDegreesDto.Id)
            {
                return BadRequest("the Id not Valid");
            }
            await _scientificDegreeService.UpdateScientificDegreeAsync(updateScientificDegreesDto);
            return Ok("the update Success");
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteScientificDegrees([FromRoute] int Id)
        {
            await _scientificDegreeService.DeleteScientificDegreeAsync(Id);
            return Ok("Delete ScientificDegrees Success");
        }
    }
}
