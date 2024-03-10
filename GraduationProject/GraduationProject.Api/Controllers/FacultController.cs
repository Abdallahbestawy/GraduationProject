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
    }
}
