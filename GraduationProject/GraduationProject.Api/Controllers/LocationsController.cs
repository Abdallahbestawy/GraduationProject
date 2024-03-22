using GraduationProject.Service.DataTransferObject.LocationsDto;
using GraduationProject.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private readonly ILocationsService _locationsService;

        public LocationsController(ILocationsService locationsService)
        {
            _locationsService = locationsService;
        }

        [HttpPost("AddCountry")]
        public async Task<IActionResult> AddCountry(CountryDto AddCountryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Please enter a valid model", Data = AddCountryDto });
            }
            var response = await _locationsService.AddCountryAsync(AddCountryDto);

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("GetCountry")]
        public async Task<IActionResult> GetCountry()
        {
            var response = await _locationsService.GetCountryAsync();

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("GetCountry{Id:int}")]
        public async Task<IActionResult> GetCountryById(int Id)
        {
            var response = await _locationsService.GetCountryByIdAsync(Id);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("UpdateCountry")]
        public async Task<IActionResult> UpdateCountry(CountryDto AddCountryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Please enter a valid model", Data = AddCountryDto });
            }
            var response = await _locationsService.UpdateCountryAsync(AddCountryDto);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("AddGovernorate")]
        public async Task<IActionResult> AddGovernorate(List<GovernorateDto> AddGovernorateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Please enter a valid model", Data = AddGovernorateDto });
            }
            var response = await _locationsService.AddGovernorateAsync(AddGovernorateDto);

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("GetGovernorate")]
        public async Task<IActionResult> GetGovernorate()
        {
            var response = await _locationsService.GetGovernorateAsync();

            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("AddCity")]
        public async Task<IActionResult> AddCity(List<CityDto> addCityDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Please enter a valid model", Data = addCityDto });
            }
            var response = await _locationsService.AddCityAsync(addCityDto);

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("GetCity")]
        public async Task<IActionResult> GetCity()
        {
            var response = await _locationsService.GetCityAsync();

            return StatusCode(response.StatusCode, response);
        }
    }
}
