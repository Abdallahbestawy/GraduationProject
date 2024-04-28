using GraduationProject.Identity.Enum;
using GraduationProject.Service.DataTransferObject.AssessMethodDto;
using GraduationProject.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject.Api.Controllers
{
    [Authorize(Roles = nameof(UserType.Administration))]
    [Route("api/[controller]")]
    [ApiController]
    public class AssessMethodController : ControllerBase
    {
        private readonly IAssessMethodService _assessMethodService;
        public AssessMethodController(IAssessMethodService assessMethodService)
        {
            _assessMethodService = assessMethodService;
        }

        [HttpGet("{Id:int}")]
        public async Task<IActionResult> GetAssessMethodById([FromRoute] int Id)
        {
            if (Id.Equals(null))
            {
                return BadRequest("Please Enter Valid Id");
            }
            var response = await _assessMethodService.GetAssessMethodByIdAsync(Id);

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("All{facultyId:int}")]
        public async Task<IActionResult> GetAssessMethods(int facultyId)
        {
            var response = await _assessMethodService.GetAssessMethodAsync(facultyId);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> AddAssessMethods(AssessMethodDto addAssessMethodDto)
        {
            var response = await _assessMethodService.AddAssessMethodAsync(addAssessMethodDto);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateAssessMethod([FromBody] AssessMethodDto updateAssessMethodDto)
        {
            if (updateAssessMethodDto == null)
            {
                return BadRequest("Please Enter Valid Model");
            }
            var response = await _assessMethodService.UpdateAssessMethodAsync(updateAssessMethodDto);

            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAssessMethod([FromRoute] int Id)
        {
            var response = await _assessMethodService.DeleteAssessMethodAsync(Id);

            return StatusCode(response.StatusCode, response);
        }
    }
}
