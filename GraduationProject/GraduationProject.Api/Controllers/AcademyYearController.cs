using GraduationProject.ResponseHandler.Model;
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
            var response = await _academyYearService.GetAcademyYearByIdAsync(Id);

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAcademyYears()
        {
            var response = await _academyYearService.GetAcademyYearAsync();
            
            return StatusCode(response.StatusCode,response);
        }

        [HttpPost("AddAcademyYear")]
        public async Task<IActionResult> AddAcademyYear(AcademyYearDto addAcademyYearDto)
        {
            var response = await _academyYearService.AddAcademyYearAsync(addAcademyYearDto);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("UpdateAcademyYear{Id:int}")]
        public async Task<IActionResult> UpdateAcademyYear([FromRoute] int Id, [FromBody] AcademyYearDto updateAcademyYearDto)
        {
            if (Id != updateAcademyYearDto.Id)
            {
                return BadRequest("the Id not Valid");
            }
            var response = await _academyYearService.UpdateAcademyYearAsync(updateAcademyYearDto);

            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("DeleteAcademyYear")]
        public async Task<IActionResult> DeleteAcademyYear([FromRoute] int Id)
        {
            var response = await _academyYearService.DeleteAcademyYearAsync(Id);

            return StatusCode(response.StatusCode, response);
        }
    }
}
