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
                await _StaffService.AddStaffSemesterAsync(addStaffSemesterDto);
                return Ok("Add Staff Semester Succes");
            }
            else
            {
                return BadRequest("please enter Vaild Model");
            }
        }
        [HttpPost("GetCourseStaffSemester{staffId:int}")]
        public async Task<IActionResult> GetCourseStaffSemester(int staffId)
        {
            var entity = await _StaffService.Test(staffId);
            if (entity != null)
            {
                return Ok(entity);
            }
            else
            {
                return BadRequest("not course have doctor");
            }
        }
    }
}
