using GraduationProject.Identity.Enum;
using GraduationProject.Identity.IService;
using GraduationProject.Service.DataTransferObject.StaffDto;
using GraduationProject.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly IStaffService _StaffService;
        private readonly IAccountService _accountService;
        public StaffController(IStaffService StaffService, IAccountService accountService)
        {
            _StaffService = StaffService;
            _accountService = accountService;
        }
        [Authorize(Roles = nameof(UserType.Administration))]
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
        [Authorize(Roles = nameof(UserType.Administration))]
        [HttpPost("AssignCourseStaff")]
        public async Task<IActionResult> AddStaffSemester([FromBody] List<AddStaffSemesterDto> addStaffSemesterDto)
        {
            if (!addStaffSemesterDto.Any())
            {
                return BadRequest("please enter Vaild Model");
            }
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
        [Authorize(Roles = nameof(UserType.Teacher) + "," + nameof(UserType.TeacherAssistant))]
        [HttpGet("CSS")]
        public async Task<IActionResult> GetCourseStaffSemester()
        {
            var currentUser = await _accountService.GetUser(User);
            if (currentUser == null)
            {
                return Unauthorized();
            }
            var response = await _StaffService.GetCourseStaffSemesterAsync(currentUser.Id);

            return StatusCode(response.StatusCode, response);
        }
        [Authorize(Roles = nameof(UserType.TeacherAssistant) + "," + nameof(UserType.Teacher))]
        [HttpGet("BasicData")]
        public async Task<IActionResult> GetStaff()
        {
            string userId = "63d3ab54-6da1-429d-b8f7-7f9e56fa75fc";

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

        [HttpGet("InfoData{staffId:int}")]
        public async Task<IActionResult> GetStudentInfo(int staffId)
        {
            var response = await _StaffService.GetStaffInfoByStaffIdAsync(staffId);

            return StatusCode(response.StatusCode, response);
        }
    }
}
