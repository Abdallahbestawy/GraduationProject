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
            var examRole = await _examRoleService.GetExamRoleByIdAsync(Id);
            return Ok(examRole);
        }
        [HttpGet]
        public async Task<IActionResult> GetExamRoles()
        {
            var examRole = await _examRoleService.GetExamRoleAsync();
            return Ok(examRole);
        }
        [HttpPost]
        public async Task<IActionResult> AddExamRoles(ExamRolesDto addExamRolesDto)
        {
            await _examRoleService.AddExamRoleAsync(addExamRolesDto);
            return Ok("Add ExamRoles Success");
        }
        [HttpPut("{Id:int}")]
        public async Task<IActionResult> UpdateExamRoles([FromRoute] int Id, [FromBody] ExamRolesDto updateExamRolesDto)
        {
            if (Id != updateExamRolesDto.Id)
            {
                return BadRequest("the Id not Valid");
            }
            await _examRoleService.UpdateExamRoleAsync(updateExamRolesDto);
            return Ok("the update Success");
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteExamRoles([FromRoute] int Id)
        {
            await _examRoleService.DeleteExamRoleAsync(Id);
            return Ok("Delete ExamRoles Success");
        }
    }
}
