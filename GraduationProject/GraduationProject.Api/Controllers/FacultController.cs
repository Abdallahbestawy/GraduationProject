using GraduationProject.Service.DataTransferObject.FacultyDto;
using GraduationProject.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacultController : ControllerBase
    {
        private IFacultService _facultService;
        public FacultController(IFacultService facultService)
        {
            _facultService = facultService;
        }

        [HttpPost]
        public async Task<IActionResult> AddFacult(FacultyDto facultyDto)
        {
            var response = await _facultService.AddFacultAsync(facultyDto);

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("Faculty")]
        public async Task<IActionResult> GetFaculty()
        {
            string userId = "3ed1410b-286c-4064-9193-35b792b8aebf";
            var response = await _facultService.GetFacultByUserIdAsync(userId);

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("GetFacultyDetails{facultyId:int}")]
        public async Task<IActionResult> GetFacultyDetails(int facultyId)
        {
            var response = await _facultService.GetFacultyDetailsAsync(facultyId);

            return StatusCode(response.StatusCode, response);
        }
    }
}
