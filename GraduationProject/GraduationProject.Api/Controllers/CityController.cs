using GraduationProject.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly ICityService _cityService;

        public CityController(ICityService cityService)
        {
            _cityService = cityService;
        }

        [HttpGet("GetByGovernorateId/{governorateId:int}")]
        public async Task<IActionResult> GetByGovernorateId([FromRoute]int governorateId)
        {
            var response = await _cityService.GetCitiesByGovernorateId(governorateId);

            return StatusCode(response.StatusCode, response);
        }
    }
}
