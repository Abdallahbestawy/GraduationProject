using GraduationProject.Service.DataTransferObject.AssessMethodDto;
using GraduationProject.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssessMethodController : ControllerBase
    {
        private readonly IAssessMethodService _assessMethodService;
        public AssessMethodController(IAssessMethodService assessMethodService)
        {
            _assessMethodService = assessMethodService;
        }
        [HttpGet("{Id:int}")]
        public async Task<IActionResult> GetAssessMethodById([FromRoute] int Id)
        {
            if (Id.Equals(null))
            {
                return BadRequest("Please Enter Id Valid");
            }
            var assessMethod = await _assessMethodService.GetAssessMethodByIdAsync(Id);
            return Ok(assessMethod);
        }
        [HttpGet]
        public async Task<IActionResult> GetAssessMethods()
        {
            var assessMethods = await _assessMethodService.GetAssessMethodAsync();
            return Ok(assessMethods);
        }
        [HttpPost]
        public async Task<IActionResult> AddBand(AssessMethodDto addAssessMethodDto)
        {
            await _assessMethodService.AddAssessMethodAsync(addAssessMethodDto);
            return Ok("Add AssessMethod Success");
        }
        [HttpPut("{Id:int}")]
        public async Task<IActionResult> UpdateAssessMethod([FromRoute] int Id, [FromBody] AssessMethodDto updateAssessMethodDto)
        {
            if (Id != updateAssessMethodDto.Id)
            {
                return BadRequest("the Id not Valid");
            }
            await _assessMethodService.UpdateAssessMethodAsync(updateAssessMethodDto);
            return Ok("the update Success");
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteAssessMethod([FromRoute] int Id)
        {
            await _assessMethodService.DeleteAssessMethodAsync(Id);
            return Ok("Delete AssessMethod Success");
        }
    }
}
