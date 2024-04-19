using GraduationProject.Service.DataTransferObject.StaffDto;
using GraduationProject.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministrationController : ControllerBase
    {
        private readonly IAdministrationService _administrationService;
        public AdministrationController(IAdministrationService AdministrationService)
        {
            _administrationService = AdministrationService;
        }

        [HttpPost]
        public async Task<IActionResult> AddAdministration(AddStaffDto addAdministrationDto)
        {
            if (ModelState.IsValid)
            {
                var response = await _administrationService.AddAdministrationAsync(addAdministrationDto);

                return StatusCode(response.StatusCode, response);
            }
            else
            {
                return BadRequest("please enter valid Model");
            }

        }

        [HttpGet("GetAllAdministration")]
        public async Task<IActionResult> GetAllAdministration()
        {

            var response = await _administrationService.GetAllAdministrationsAsync();

            return StatusCode(response.StatusCode, response);
        }

    }
}