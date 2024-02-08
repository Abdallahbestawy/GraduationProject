using GraduationProject.Service.DataTransferObject.StaffDto;
using GraduationProject.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly IStaffService _StaffService;
        public StaffController(IStaffService StaffService)
        {
            _StaffService = StaffService;
        }
        [HttpPost]
        public async Task<IActionResult> AddStaff(AddStaffDto addStaffDto)
        {
            if (ModelState.IsValid)
            {
                int raw = await _StaffService.AddStAffAsync(addStaffDto);
                if (raw == 1)
                {
                    return Ok("Staff Add");
                }
                else
                {
                    return BadRequest("please enter valid Model");
                }
            }
            else
            {
                return BadRequest("please enter valid Model");
            }
        }
    }
}
