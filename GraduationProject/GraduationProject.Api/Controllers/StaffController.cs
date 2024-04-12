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

        [HttpGet("GetCourseStaffSemester{staffId:int}")]
        public async Task<IActionResult> GetCourseStaffSemester(int staffId)
        {
            var response = await _StaffService.GetCourseStaffSemesterAsync(staffId);

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("GetStaff")]
        public async Task<IActionResult> GetStaff()
        {
            string userId = "3ed1410b-286c-4064-9193-35b792b8aebf";

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
            bool respone = await _StaffService.DeleteStaffSemesterAsync(staffSemesterId);
            if (respone)
            {
                return Ok("Delete Staff Semester Success");
            }
            return BadRequest("please enter vaild Staff Semester and Try again");
        }
    }
}
