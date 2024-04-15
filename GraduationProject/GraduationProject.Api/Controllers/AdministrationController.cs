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

        [HttpDelete("Delete{Id:int}")]
        public async Task<IActionResult> DeleteStaff(int Id)
        {
            if (Id == 0 || Id == null)
            {
                return BadRequest("please enter valid AdministrationId");
            }
            var respone = await _administrationService.DeleteAsync(Id);

            return StatusCode(respone.StatusCode, respone);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateStaff([FromBody] AddStaffDto updateStaffDto)
        {
            if (updateStaffDto == null)
            {
                return BadRequest("Please enter valid Model");
            }
            var respone = await _administrationService.UpdateStaffAsync(updateStaffDto);

            return StatusCode(respone.StatusCode, respone);
        }
    }
}