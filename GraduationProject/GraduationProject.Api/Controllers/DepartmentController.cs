using GraduationProject.Identity.Enum;
using GraduationProject.Service.DataTransferObject.DepartmentDto;
using GraduationProject.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject.Api.Controllers
{
    [Authorize(Roles = nameof(UserType.Administration))]
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private IDepartmentService _departmentService;
        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }
        [HttpPost]
        public async Task<IActionResult> AddDepartment(DepartmentDto addDepartmentDto)
        {
            var response = await _departmentService.AddDepartmentAsync(addDepartmentDto);

            return StatusCode(response.StatusCode, response);
        }
        [HttpGet("All")]
        public async Task<IActionResult> GetAllDepartment()
        {
            var response = await _departmentService.GetAllDepartmentsAsync();
            if (response != null)
            {
                return Ok(response);
            }
            return NotFound("There are not Department");
        }
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateDepartment(DepartmentDto updateDepartmentDto)
        {
            if (updateDepartmentDto == null)
            {
                return BadRequest("Please Enter Valid Model");
            }
            var response = await _departmentService.UpdateDepartmentAsync(updateDepartmentDto);

            return StatusCode(response.StatusCode, response);
        }
        [HttpGet("Get/{Id:int}")]
        public async Task<IActionResult> GetDepartmentById([FromRoute] int Id)
        {
            var response = await _departmentService.GetDepartmentByIdAsync(Id);
            if (response != null)
            {
                return Ok(response);
            }
            return NotFound("There are not Department");
        }
        [HttpDelete("Delete/{Id:int}")]
        public async Task<IActionResult> DeleteDepartment(int Id)
        {
            var response = await _departmentService.DeleteDepartmentAsync(Id);

            return StatusCode(response.StatusCode, response);
        }
    }
}
