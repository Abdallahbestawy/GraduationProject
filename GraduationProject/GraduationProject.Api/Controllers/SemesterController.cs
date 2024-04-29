using GraduationProject.Identity.Enum;
using GraduationProject.Service.DataTransferObject.SemesterDto;
using GraduationProject.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject.Api.Controllers
{
    [Authorize(Roles = nameof(UserType.Administration))]
    [Route("api/[controller]")]
    [ApiController]
    public class SemesterController : ControllerBase
    {
        private readonly ISemesterService _semesterService;
        public SemesterController(ISemesterService semesterService)
        {
            _semesterService = semesterService;
        }

        [HttpGet("{Id:int}")]
        public async Task<IActionResult> GetSemesterById([FromRoute] int Id)
        {
            if (Id.Equals(null))
            {
                return BadRequest("Please Enter Id Valid");
            }
            var response = await _semesterService.GetSemesterByIdAsync(Id);

            return StatusCode(response.StatusCode, response);

        }

        [HttpGet]
        public async Task<IActionResult> GetSemesters()
        {
            var response = await _semesterService.GetSemesterAsync();

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("ByFacultyId/{facultyId:int}")]
        public async Task<IActionResult> GetSemestersByFacultyId(int facultyId)
        {
            var response = await _semesterService.GetSemesterByFacultyIdAsync(facultyId);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> AddSemester(SemesterDto addSemesterDto)
        {
            var response = await _semesterService.AddSemesterAsync(addSemesterDto);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateSemester([FromBody] SemesterDto updateSemesterDto)
        {
            if (updateSemesterDto == null)
            {
                return BadRequest("Please Enter Valid Model");
            }
            var response = await _semesterService.UpdateSemesterAsync(updateSemesterDto);

            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("{Id:int}")]
        public async Task<IActionResult> DeleteSemester(int Id)
        {
            var response = await _semesterService.DeleteSemesterAsync(Id);

            return StatusCode(response.StatusCode, response);
        }
    }
}
