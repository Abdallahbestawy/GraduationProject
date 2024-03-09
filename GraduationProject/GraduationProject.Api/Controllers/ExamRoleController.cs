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
        [HttpPost]
        public async Task<IActionResult> AddExamRoles(ExamRolesDto addExamRolesDto)
        {
            var response = await _examRoleService.AddExamRoleAsync(addExamRolesDto);

            return StatusCode(response.StatusCode, response);
        }
        [HttpPut("{Id:int}")]
        public async Task<IActionResult> UpdateExamRoles([FromRoute] int Id, [FromBody] ExamRolesDto updateExamRolesDto)
        {
            if (Id != updateExamRolesDto.Id)
            {
                return BadRequest("the Id not Valid");
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
