using Microsoft.AspNetCore.Mvc;
using Phone_mvc.Common;
using Phone_mvc.Dtos;
using Phone_mvc.Extensions;
using Phone_mvc.Services;

namespace Phone_mvc.Controllers
{
    public class UserController(IUserService _userService) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
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
