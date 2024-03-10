using GraduationProject.Service.DataTransferObject.StaffDto;
using GraduationProject.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministrationController : ControllerBase
    {
        private readonly IAdministrationService _AdministrationService;
        public AdministrationController(IAdministrationService AdministrationService)
        {
            _AdministrationService = AdministrationService;
        }
        [HttpPost]
        public async Task<IActionResult> AddAdministration(AddStaffDto addAdministrationDto)
        {
            if (ModelState.IsValid)
            {
                int raw = await _AdministrationService.AddAdministrationAsync(addAdministrationDto);
                if (raw == 1)
                {
                    return Ok("Administration Add");
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
        [HttpGet("GetAllAdministration")]
        public async Task<IActionResult> GetAllAdministration()
        {

            var response = await _AdministrationService.GetAllAdministrationsAsync();
            if (response != null)
            {
                return Ok(response);
            }
            else
            {
                return NotFound("There are not Administration");
            }
        }
    }
}
