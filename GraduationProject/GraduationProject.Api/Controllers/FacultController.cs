using GraduationProject.Identity.Enum;
using GraduationProject.Identity.IService;
using GraduationProject.Service.DataTransferObject.FacultyDto;
using GraduationProject.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject.Api.Controllers
{
    [Authorize(Roles = nameof(UserType.Administration))]
    [Route("api/[controller]")]
    [ApiController]
    public class FacultController : ControllerBase
    {
        private IFacultService _facultService;
        private readonly IAccountService _accountService;
        public FacultController(IFacultService facultService, IAccountService accountService)
        {
            _facultService = facultService;
            _accountService = accountService;
        }

        [HttpPost]
        public async Task<IActionResult> AddFacult(FacultyDto facultyDto)
        {
            var currentUser = await _accountService.GetUser(User);
            if (currentUser == null)
            {
                return Unauthorized();
            }
            var response = await _facultService.AddFacultAsync(facultyDto, currentUser.Id);

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("Faculty")]
        public async Task<IActionResult> GetFaculty()
        {
            var currentUser = await _accountService.GetUser(User);
            if (currentUser == null)
            {
                return Unauthorized();
            }
            var response = await _facultService.GetFacultByUserIdAsync(currentUser.Id);

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("GetFacultyDetails{facultyId:int}")]
        public async Task<IActionResult> GetFacultyDetails(int facultyId)
        {
            var response = await _facultService.GetFacultyDetailsAsync(facultyId);

            return StatusCode(response.StatusCode, response);
        }
        [HttpDelete("{Id:int}")]
        public async Task<IActionResult> DeleteFaculty(int Id)
        {
            var response = await _facultService.DeleteFacultyAsync(Id);

            return StatusCode(response.StatusCode, response);
        }
    }
}
