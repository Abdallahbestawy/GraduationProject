using GraduationProject.Service.DataTransferObject.AcademyYearDto;
using GraduationProject.Service.IService;
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
        [HttpGet("GetAcademyYearById{Id:int}")]
        public async Task<IActionResult> GetAcademyYearById([FromRoute] int Id)
        {
            if (Id.Equals(null))
            {
                return BadRequest("Please Enter Id Valid");
            }
            var academyYear = await _academyYearService.GetAcademyYearByIdAsync(Id);
            return Ok(academyYear);
        }
        [HttpGet]
        public async Task<IActionResult> GetAcademyYears()
        {
            var academyYears = await _academyYearService.GetAcademyYearAsync();
            return Ok(academyYears);
        }
        [HttpPost("AddAcademyYear")]
        public async Task<IActionResult> AddAcademyYear(AcademyYearDto addAcademyYearDto)
        {
            await _academyYearService.AddAcademyYearAsync(addAcademyYearDto);
            return Ok("Add AcademyYear Success");
        }
        [HttpPut("UpdateAcademyYear{Id:int}")]
        public async Task<IActionResult> UpdateAcademyYear([FromRoute] int Id, [FromBody] AcademyYearDto updateAcademyYearDto)
        {
            if (Id != updateAcademyYearDto.Id)
            {
                return BadRequest("the Id not Valid");
            }
            await _academyYearService.UpdateAcademyYearAsync(updateAcademyYearDto);
            return Ok("the update Success");
        }
        [HttpDelete("DeleteAcademyYear")]
        public async Task<IActionResult> DeleteAcademyYear([FromRoute] int Id)
        {
            await _academyYearService.DeleteAcademyYearAsync(Id);
            return Ok("Delete AcademyYear Success");
        }
    }
}
