using GraduationProject.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private IDepartment _service;
        public DepartmentController(IDepartment service)
        {
            _service = service;
        }
        [HttpPost]
        public IActionResult AddDept(Data.Entity.Department entity)
        {
            return Ok(_service.AddDepartmentAsync(entity));
        }
    }
}
