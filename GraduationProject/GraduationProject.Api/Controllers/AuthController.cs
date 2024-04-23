using GraduationProject.Identity.IService;
using GraduationProject.Identity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginUserModel loginUserModel)
        {
            var result = await _authService.LoginAsync(loginUserModel);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            //this part gets the host of the request 
            string? requestHost = HttpContext.Request.Headers["Referer"];
            if (requestHost == null)
                requestHost = "localhost";
            Uri uri = new Uri(requestHost);
            requestHost = uri.Host;

            if (!string.IsNullOrEmpty(result.RefreshToken))
            SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration, requestHost);

            return Ok(result);
        }

        [HttpGet("refreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            var result = await _authService.RefreshTokenAsync(refreshToken);

            if (!result.IsAuthenticated)
                return BadRequest(result);

            //this part gets the host of the request 
            string? requestHost = HttpContext.Request.Headers["Referer"];
            if (requestHost == null)
                requestHost = "localhost";
            Uri uri = new Uri(requestHost);
            requestHost = uri.Host;

            SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration, requestHost);

            return Ok(result);
        }

        [HttpPost("revokeToken")]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeToken revokeToken)
        {
            var token = revokeToken.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest("Token is required!");

            var result = await _authService.RevokeTokenAsync(token);

            if (!result)
                return BadRequest("Token is invalid!");

            return Ok();
        }
        private void SetRefreshTokenInCookie(string refreshToken, DateTime expires,string domain)
        {
            var cookieOptions = new CookieOptions
            {
                Domain = domain,
                Path = "/",
                HttpOnly = false,
                Expires = expires.ToLocalTime(),
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.None
            };
            Response.Headers.Add("Access-Control-Allow-Credentials", "true");
            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
        {
            string baseURL = $"{Request.Scheme}://{Request.Host}";

            var response = await _authService.ForgotPassword(model,baseURL);

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromQuery] ResetPasswordModel model)
        {
            var response = await _authService.ResetPassword(model);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("ChangeUserRoles")]
        public async Task<IActionResult> ChangeUserRoles([FromBody] UserRolesDto model)
        {

            var response = await _authService.ChangeUserRolesAsync(model);

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("GetUserRoles/{userId}")]
        public async Task<IActionResult> GetUserRoles(string userId)
        {

            var response = await _authService.GetUserRolesAsync(userId);

            return StatusCode(response.StatusCode, response);
        }
    }
}
