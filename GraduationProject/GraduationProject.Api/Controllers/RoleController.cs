using GraduationProject.Identity.Enum;
using GraduationProject.Identity.IService;
using GraduationProject.Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject.Api.Controllers
{
    [Authorize(Roles = nameof(UserType.Administration))]
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
            var response = await _roleService.GetAllRoles();

            return StatusCode(response.StatusCode, response);
        }
        [HttpPost]
        public async Task<IActionResult> AddRole(RoleModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _roleService.AddRole(model);

                return StatusCode(response.StatusCode, response);
            }
            else
                return BadRequest("please enter Valid Model");
        }
    }
}
