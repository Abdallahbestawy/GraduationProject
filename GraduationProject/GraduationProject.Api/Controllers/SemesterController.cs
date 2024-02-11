using GraduationProject.Service.DataTransferObject.SemesterDto;
using GraduationProject.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject.Api.Controllers
{
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
            var semester = await _semesterService.GetSemesterByIdAsync(Id);
            return Ok(semester);

        }
        [HttpGet]
        public async Task<IActionResult> GetSemesters()
        {
            var semesters = await _semesterService.GetSemesterAsync();
            return Ok(semesters);
        }
        [HttpPost]
        public async Task<IActionResult> AddSemester(SemesterDto addSemesterDto)
        {
            await _semesterService.AddSemesterAsync(addSemesterDto);
            return Ok("Add Semesters Success");
        }
        [HttpPut("{Id:int}")]
        public async Task<IActionResult> UpdateSemester([FromRoute] int Id, [FromBody] SemesterDto updateSemesterDto)
        {
            if (Id != updateSemesterDto.Id)
            {
                return BadRequest("the Id not Valid");
            }
            await _semesterService.UpdateSemesterAsync(updateSemesterDto);
            return Ok("the Semesters Success");
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteSemester(int Id)
        {
            await _semesterService.DeleteSemesterAsync(Id);
            return Ok("Delete Semester Success");
        }
    }
}
