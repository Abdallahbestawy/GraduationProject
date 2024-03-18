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
            bool response = await _locationsService.AddCountryAsync(AddCountryDto);
            if (response)
            {
                return Ok("The Add Country Success");
            }
            return BadRequest(new { Message = "Please enter a valid model", Data = AddCountryDto });
        }
        [HttpGet("GetCountry")]
        public async Task<IActionResult> GetCountry()
        {
            var response = await _locationsService.GetCountryAsync();
            if (response != null)
            {
                return Ok(response);
            }
            return NotFound("the are not Contry");
        }
        [HttpGet("GetCountry{Id:int}")]
        public async Task<IActionResult> GetCountryById(int Id)
        {
            var response = await _locationsService.GetCountryByIdAsync(Id);
            if (response != null)
            {
                return Ok(response);
            }
            return NotFound("please enter Valid Id");
        }
        [HttpPut("UpdateCountry")]
        public async Task<IActionResult> UpdateCountry(CountryDto AddCountryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Please enter a valid model", Data = AddCountryDto });
            }
            bool response = await _locationsService.UpdateCountryAsync(AddCountryDto);
            if (response)
            {
                return Ok("The Add Country Success");
            }
            return BadRequest(new { Message = "Please enter a valid model", Data = AddCountryDto });
        }
        [HttpPost("AddGovernorate")]
        public async Task<IActionResult> AddGovernorate(List<GovernorateDto> AddGovernorateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Please enter a valid model", Data = AddGovernorateDto });
            }
            bool response = await _locationsService.AddGovernorateAsync(AddGovernorateDto);
            if (response)
            {
                return Ok("The Add Governorate Success");
            }
            return BadRequest(new { Message = "Please enter a valid model", Data = AddGovernorateDto });
        }
        [HttpGet("GetGovernorate")]
        public async Task<IActionResult> GetGovernorate()
        {
            var response = await _locationsService.GetGovernorateAsync();
            if (response != null)
            {
                return Ok(response);
            }
            return NotFound("the are not Governorate");
        }
        [HttpPost("AddCity")]
        public async Task<IActionResult> AddCity(List<CityDto> addCityDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Please enter a valid model", Data = addCityDto });
            }
            bool response = await _locationsService.AddCityAsync(addCityDto);
            if (response)
            {
                return Ok("The Add City Success");
            }
            return BadRequest(new { Message = "Please enter a valid model", Data = addCityDto });
        }
        [HttpGet("GetCity")]
        public async Task<IActionResult> GetCity()
        {
            var response = await _locationsService.GetCityAsync();
            if (response != null)
            {
                return Ok(response);
            }
            return NotFound("the are not City");
        }
    }
}
