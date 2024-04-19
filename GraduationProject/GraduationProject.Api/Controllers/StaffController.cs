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
                var response = await _StaffService.AddStAffAsync(addStaffDto);

                return StatusCode(response.StatusCode, response);
            }
            else
            {
                return BadRequest("please enter valid Model");
            }
        }

        [HttpPost("AssignCourseStaff")]
        public async Task<IActionResult> AddStaffSemester([FromBody] List<AddStaffSemesterDto> addStaffSemesterDto)
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

        [HttpGet("CSS")]
        public async Task<IActionResult> GetCourseStaffSemester()
        {
            string userId = "63d3ab54-6da1-429d-b8f7-7f9e56fa75fc";
            var response = await _StaffService.GetCourseStaffSemesterAsync(userId);

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("GetStaff")]
        public async Task<IActionResult> GetStaff(string? uId)
        {
            string userId = uId ?? "63d3ab54-6da1-429d-b8f7-7f9e56fa75fc";

            var response = await _StaffService.GetStaffByUserIdAsync(userId);

            return StatusCode(response.StatusCode, response);
        }
        [HttpGet("GetAllStaff")]
        public async Task<IActionResult> GetAllStaff()
        {

            var response = await _StaffService.GetAllStaffsAsync();
            if (response != null)
            {
                return Ok(response);
            }
            else
            {
                return NotFound("There are not Staff");
            }
        }
        [HttpDelete("staffSemester{staffSemesterId:int}")]
        public async Task<IActionResult> DeleteStaffSemester(int staffSemesterId)
        {
            if (staffSemesterId == 0 || staffSemesterId == null)
            {
                return BadRequest("please enter valid staffSemesterId");
            }
            var respone = await _StaffService.DeleteStaffSemesterAsync(staffSemesterId);

            return StatusCode(respone.StatusCode, respone);
        }
        [HttpDelete("Delete{Id:int}")]
        public async Task<IActionResult> DeleteStaff(int Id)
        {
            if (Id == 0 || Id == null)
            {
                return BadRequest("please enter valid SatffId");
            }
            var respone = await _StaffService.DeleteAsync(Id);

            return StatusCode(respone.StatusCode, respone);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateStaff([FromBody] UpdateStaffDto updateStaffDto)
        {
            if (updateStaffDto == null)
            {
                return BadRequest("Please enter valid Model");
            }
            var respone = await _StaffService.UpdateStaffAsync(updateStaffDto);

            return StatusCode(respone.StatusCode, respone);
        }
    }
}
