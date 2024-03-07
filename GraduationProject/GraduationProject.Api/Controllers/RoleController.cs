using GraduationProject.Identity.IService;
using GraduationProject.Identity.Models;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        [HttpGet]
        public async Task<IActionResult> GetRole()
        {
            var roles = await _roleService.GetAllRoles();
            return Ok(roles);
        }
        [HttpPost]
        public async Task<IActionResult> AddRole(RoleModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _roleService.AddRole(model);
                if (response.StatusCode ==200)
                    return Ok(response);
                else
                    return StatusCode(response.StatusCode,response);
            }
            else 
                return BadRequest("please enter Valid Model");
        }
    }
}
