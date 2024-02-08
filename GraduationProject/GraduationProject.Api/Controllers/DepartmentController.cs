using GraduationProject.Service.DataTransferObject.DepartmentDto;
using GraduationProject.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private IDepartmentService _service;
        public DepartmentController(IDepartmentService service)
        {
            _service = service;
        }
        [HttpPost]
        public async Task<IActionResult> AddDepartment(DepartmentDto departmentDto)
        {
            await _service.AddDepartmentAsync(departmentDto);
            return Ok("Add Department Success");
        }
    }
}
