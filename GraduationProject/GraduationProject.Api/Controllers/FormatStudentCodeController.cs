using GraduationProject.Identity.Enum;
using GraduationProject.Service.DataTransferObject.FormatStudentCodeDto;
using GraduationProject.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject.Api.Controllers
{
    [Authorize(Roles = nameof(UserType.Administration))]
    [Route("api/[controller]")]
    [ApiController]
    public class FormatStudentCodeController : ControllerBase
    {
        private readonly IFormatStudentCodeService _formatStudentCodeService;
        public FormatStudentCodeController(IFormatStudentCodeService formatStudentCodeService)
        {
            _formatStudentCodeService = formatStudentCodeService;
        }
        [HttpGet("{Id:int}")]
        public async Task<IActionResult> GetBylawById([FromRoute] int Id)
        {
            if (Id.Equals(null))
            {
                return BadRequest("Please Enter Id Valid");
            }
            var response = await _formatStudentCodeService.GetFormatStudentCodeByIdAsync(Id);

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("ByFacultyId/{facultyId:int}")]
        public async Task<IActionResult> GetFormatStudentCodeByFacultyId(int facultyId)
        {
            if (facultyId.Equals(null))
            {
                return BadRequest("Please Enter Valid Id");
            }
            var response = await _formatStudentCodeService.GetFormatStudentCodeByFacultyIdAsync(facultyId);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> AddFormatStudentCode(FormatStudentCodeDto addFormatStudentCodeDto)
        {
            var response = await _formatStudentCodeService.AddFormatStudentCodeAsync(addFormatStudentCodeDto);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateFormatStudentCode([FromBody] FormatStudentCodeDto updateFormatStudentCodeDto)
        {
            if (updateFormatStudentCodeDto == null)
            {
                return BadRequest("Please Enter Valid Model");
            }
            var response = await _formatStudentCodeService.UpdateFormatStudentCodeAsync(updateFormatStudentCodeDto);

            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("{Id:int}")]
        public async Task<IActionResult> DeleteFormatStudentCode([FromRoute] int Id)
        {
            var response = await _formatStudentCodeService.DeleteFormatStudentCodeAsync(Id);

            return StatusCode(response.StatusCode, response);
        }
    }
}
