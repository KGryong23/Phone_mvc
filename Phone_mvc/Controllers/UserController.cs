using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Phone_mvc.Common;
using Phone_mvc.Dtos;
using Phone_mvc.Extensions;
using Phone_mvc.Services;
using System.Security.Claims;

namespace Phone_mvc.Controllers
{
    public class UserController(IUserService _userService) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [IgnoreAntiforgeryToken]
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
        {
            if (!TryValidateModel(request))
            {
                var errors = ControllerBaseExtensions.GetValidationErrors(ModelState);
                return BadRequest(ApiResponse<object>.ErrorResult("Dữ liệu không hợp lệ.", errors));
            }

            var userId = await _userService.CreateAsync(request);
            return CreatedAtAction(nameof(Create), new { id = userId }, ApiResponse<object>.SuccessResult(request, "Tạo người dùng thành công."));
        }
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            if (!TryValidateModel(request))
            {
                var errors = ControllerBaseExtensions.GetValidationErrors(ModelState);
                return BadRequest(ApiResponse<object>.ErrorResult("Dữ liệu không hợp lệ.", errors));
            }
            var response = await _userService.LoginAsync(request);
            if (response != null)
            {
                // Tạo claims cho user
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, response.Id.ToString()),
                    };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true, // Remember me
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1) // Cookie expires in 1 days
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return Ok(ApiResponse<UserDto>.SuccessResult(response, "Đăng nhập thành công."));
            }
            else
            {
                return BadRequest(ApiResponse<object>.ErrorResult("Email hoặc mật khẩu không đúng."));
            }
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(ApiResponse<bool>.SuccessResult(true, "Đăng xuất thành công."));
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
            {
                var errors = ControllerBaseExtensions.GetValidationErrors(ModelState);
                return BadRequest(ApiResponse<object>.ErrorResult("Dữ liệu không hợp lệ.", errors));
            }

            var result = await _userService.GetAllAsync();
            return Ok(ApiResponse<IEnumerable<UserDto>>.SuccessResult(result));
        }
    }
}
