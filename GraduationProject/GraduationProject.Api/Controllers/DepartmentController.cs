using GraduationProject.Service.DataTransferObject.DepartmentDto;
using GraduationProject.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject.Api.Controllers
{
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
        public async Task<IActionResult> AddDepartment(DepartmentDto departmentDto)
        {
            var response = await _departmentService.AddDepartmentAsync(departmentDto);

            return StatusCode(response.StatusCode, response);
        }
    }
}
