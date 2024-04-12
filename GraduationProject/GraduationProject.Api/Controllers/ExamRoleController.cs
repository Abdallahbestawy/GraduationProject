using GraduationProject.Service.DataTransferObject.ExamRolesDto;
using GraduationProject.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamRoleController : ControllerBase
    {
        private readonly IExamRoleService _examRoleService;

        public ExamRoleController(IExamRoleService examRoleService)
        {
            _examRoleService = examRoleService;
        }

        [HttpGet("{Id:int}")]
        public async Task<IActionResult> GetExamRoleById([FromRoute] int Id)
        {
            if (Id.Equals(null))
            {
                return BadRequest("Please Enter Id Valid");
            }
            var response = await _examRoleService.GetExamRoleByIdAsync(Id);

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetExamRoles()
        {
            var response = await _examRoleService.GetExamRoleAsync();

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("ByFacultyId/{facultyId:int}")]
        public async Task<IActionResult> GetExamByFacultyIdRoles(int facultyId)
        {
            var response = await _examRoleService.GetExamRoleByFacultyIdAsync(facultyId);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> AddExamRoles(ExamRolesDto addExamRolesDto)
        {
            var response = await _examRoleService.AddExamRoleAsync(addExamRolesDto);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateExamRoles([FromBody] ExamRolesDto updateExamRolesDto)
        {
            if (updateExamRolesDto == null)
            {
                return BadRequest("Please Enter Valid Model");
            }
            var response = await _examRoleService.UpdateExamRoleAsync(updateExamRolesDto);

            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteExamRoles([FromRoute] int Id)
        {
            var response = await _examRoleService.DeleteExamRoleAsync(Id);

            return StatusCode(response.StatusCode, response);
        }
    }
}
