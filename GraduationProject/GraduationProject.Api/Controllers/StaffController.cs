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
                var response = await _StaffService.AddStaffSemesterAsync(addStaffSemesterDto, 1);

                return StatusCode(response.StatusCode, response);
            }
            else
            {
                return BadRequest("please enter Vaild Model");
            }
        }
        [Authorize(Roles = nameof(UserType.Administration))]
        [HttpPost("AssignCourseSe")]
        public async Task<IActionResult> AddTeacherAssistantSemester([FromBody] List<AddStaffSemesterDto> addStaffSemesterDto)
        {
            if (!addStaffSemesterDto.Any())
            {
                return BadRequest("please enter Vaild Model");
            }
            if (ModelState.IsValid)
            {
                var response = await _StaffService.AddStaffSemesterAsync(addStaffSemesterDto, 2);

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
        [Authorize(Roles = nameof(UserType.Administration))]
        [HttpGet("CSSA/{staffId:int}")]
        public async Task<IActionResult> GetCourseStaffSemesterAdministration(int staffId)
        {
            if (staffId == 0 || staffId == null)
            {
                return BadRequest("Please Enter Valid StaffId");
            }

            var response = await _StaffService.GetCourseStaffSemesterAdministrationAsync(staffId);

            return StatusCode(response.StatusCode, response);
        }
        [Authorize(Roles = nameof(UserType.TeacherAssistant) + "," + nameof(UserType.Teacher) + "," + nameof(UserType.Administration) + "," + nameof(UserType.Staff) + "," + nameof(UserType.ControlMembers))]
        [HttpGet("BasicData")]
        public async Task<IActionResult> GetStaff()
        {
            var currentUser = await _accountService.GetUser(User);
            if (currentUser == null)
            {
                return Unauthorized();
            }

            var response = await _StaffService.GetStaffByUserIdAsync(currentUser.Id);

            return StatusCode(response.StatusCode, response);
        }
        //[Authorize(Roles = nameof(UserType.Administration))]
        [HttpGet("GetAllStaff/{FacultyId:int}")]
        public async Task<IActionResult> GetAllStaff(int FacultyId)
        {

            var response = await _StaffService.GetAllStaffsAsync(FacultyId);
            if (response != null)
            {
                return Ok(response);
            }
            else
            {
                return NotFound("There are not Staff");
            }
        }
        [Authorize(Roles = nameof(UserType.Administration))]
        [HttpDelete("staffSemester/{staffSemesterId:int}")]
        public async Task<IActionResult> DeleteStaffSemester(int staffSemesterId)
        {
            if (staffSemesterId == 0 || staffSemesterId == null)
            {
                return BadRequest("Please Enter Valid StaffSemesterId");
            }
            var respone = await _StaffService.DeleteStaffSemesterAsync(staffSemesterId);

            return StatusCode(respone.StatusCode, respone);
        }
        [Authorize(Roles = nameof(UserType.Administration))]
        [HttpDelete("Delete/{Id:int}")]
        public async Task<IActionResult> DeleteStaff(int Id)
        {
            if (Id == 0 || Id == null)
            {
                return BadRequest("please enter valid SatffId");
            }
            var respone = await _StaffService.DeleteAsync(Id);

            return StatusCode(respone.StatusCode, respone);
        }
        [Authorize(Roles = nameof(UserType.Administration))]
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
        [Authorize(Roles = nameof(UserType.Administration))]
        [HttpGet("InfoData/{staffId:int}")]
        public async Task<IActionResult> GetStafftInfo(int staffId)
        {
            var response = await _StaffService.GetStaffInfoByStaffIdAsync(staffId);

            return StatusCode(response.StatusCode, response);
        }
        [Authorize]
        [HttpGet("F")]
        public async Task<IActionResult> GetDetailsByUserId()
        {
            var currentUser = await _accountService.GetUser(User);
            if (currentUser == null)
            {
                return Unauthorized();
            }
            var response = await _StaffService.GetDetailsByUserIdAsync(currentUser.Id);

            return StatusCode(response.StatusCode, response);
        }
        [Authorize(Roles = nameof(UserType.Administration))]
        [HttpGet("FA/{facultyId:int}")]
        public async Task<IActionResult> GetAllByFacultyId(int facultyId)
        {
            var response = await _StaffService.GetAllByFacultyIdAsync(facultyId);

            return StatusCode(response.StatusCode, response);
        }
    }
}
