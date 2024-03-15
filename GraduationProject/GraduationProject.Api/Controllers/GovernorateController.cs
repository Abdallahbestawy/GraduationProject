using GraduationProject.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GovernorateController : ControllerBase
    {
        private readonly IGovernorateService _governorateService;

        public GovernorateController(IGovernorateService governorateService)
        {
            _governorateService = governorateService;
        }

        [HttpGet("GetByCountryId/{countryId:int}")]
        public async Task<IActionResult> GetByCountryId([FromRoute]int countryId)
        {
            var response = await _governorateService.GetByCountyId(countryId);

            return StatusCode(response.StatusCode,response);
        }
    }
}
