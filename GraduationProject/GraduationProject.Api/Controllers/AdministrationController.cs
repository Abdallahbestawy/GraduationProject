using GraduationProject.Identity.Enum;
using GraduationProject.Service.DataTransferObject.StaffDto;
using GraduationProject.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject.Api.Controllers
{
    [Authorize(Roles = nameof(UserType.Administration))]
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
                var response = await _administrationService.AddAdministrationAsync(addAdministrationDto, User);

                return StatusCode(response.StatusCode, response);
            }
            else
            {
                return BadRequest("please enter valid Model");
            }

        }

        [HttpGet("GetAllAdministration/{FacultyId:int}")]
        public async Task<IActionResult> GetAllAdministration(int FacultyId)
        {

            var response = await _administrationService.GetAllAdministrationsAsync(FacultyId);

            return StatusCode(response.StatusCode, response);
        }

    }
}