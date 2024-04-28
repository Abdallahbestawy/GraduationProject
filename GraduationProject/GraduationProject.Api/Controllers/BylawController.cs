using GraduationProject.Identity.Enum;
using GraduationProject.Service.DataTransferObject.BylawDto;
using GraduationProject.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject.Api.Controllers
{
    [Authorize(Roles = nameof(UserType.Administration))]
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
            var response = await _bylawService.GetBylawByIdAsync(Id);

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("ByFacultyId/{facultyId:int}")]
        public async Task<IActionResult> GetBylawByFacultyId(int facultyId)
        {
            if (facultyId.Equals(null))
            {
                return BadRequest("Please Enter Valid Id");
            }
            var response = await _bylawService.GetBylawByFacultyIdAsync(facultyId);

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetBylaws()
        {
            var response = await _bylawService.GetBylawAsync();

            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> AddBylaw(BylawDto addBylawDto)
        {
            var response = await _bylawService.AddBylawAsync(addBylawDto);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateBylaw([FromBody] BylawDto updateBylawDto)
        {
            if (updateBylawDto == null)
            {
                return BadRequest("Please Enter Valid Model");
            }
            var response = await _bylawService.UpdateBylawAsync(updateBylawDto);

            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBylaw([FromRoute] int Id)
        {
            var response = await _bylawService.DeleteBylawAsync(Id);

            return StatusCode(response.StatusCode, response);
        }
    }
}
