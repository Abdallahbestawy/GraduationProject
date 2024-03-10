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

        [HttpPost("AddStaff")]
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

        [HttpPost("AddStaffSemester")]
        public async Task<IActionResult> AddStaffSemester(AddStaffSemesterDto addStaffSemesterDto)
        {
            if (ModelState.IsValid)
            {
                var response = await _StaffService.AddStaffSemesterAsync(addStaffSemesterDto);

                return StatusCode(response.StatusCode, response);
            }
            else
            {
                return BadRequest("please enter Vaild Model");
            }
        }

        [HttpGet("GetCourseStaffSemester{staffId:int}")]
        public async Task<IActionResult> GetCourseStaffSemester(int staffId)
        {
            var response = await _StaffService.Test(staffId);

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("GetStaff")]
        public async Task<IActionResult> GetStaff()
        {
            string userId = "3ed1410b-286c-4064-9193-35b792b8aebf";

            var response = await _StaffService.GetStaffByUserId(userId);

            return StatusCode(response.StatusCode, response);
        }
    }
}
