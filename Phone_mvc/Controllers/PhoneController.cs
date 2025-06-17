using Microsoft.AspNetCore.Mvc;
using Phone_mvc.Common;
using Phone_mvc.Dtos;
using Phone_mvc.Extensions;
using Phone_mvc.Filters;
using Phone_mvc.Models;
using Phone_mvc.Services;

namespace Phone_mvc.Controllers
{
    [RequirePermission]
    public class PhoneController : Controller
    {
        private readonly IPhoneService _phoneService;
        private readonly IBrandService _brandService;
        public PhoneController(IPhoneService phoneService, IBrandService brandService)
        {
            _phoneService = phoneService;
            _brandService = brandService;
        }

        /// <summary>
        /// Hiển thị trang danh sách điện thoại
        /// </summary>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Lấy danh sách điện thoại phân trang với tùy chọn tìm kiếm và sắp xếp.
        /// </summary>
        /// <param name="query">Tham số truy vấn cho phân trang, tìm kiếm và sắp xếp.</param>
        [HttpGet]
        public async Task<IActionResult> GetPaged(BaseQuery query)
        {
            if (!ModelState.IsValid)
            {
                var errors = ControllerBaseExtensions.GetValidationErrors(ModelState);
                return BadRequest(ApiResponse<object>.ErrorResult("Dữ liệu không hợp lệ.", errors));
            }

            var result = await _phoneService.GetPagedAsync(query);
            return Ok(ApiResponse<PagedResult<PhoneDto>>.SuccessResult(result));
        }

        /// <summary>
        /// Xem chi tiết điện thoại
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {
            var phone = await _phoneService.GetByIdAsync(id);
            if (phone == null)
            {
                return NotFound();
            }
            return PartialView("_ViewPhone", phone);
        }

        /// <summary>
        /// Hiển thị form thêm mới/chỉnh sửa điện thoại
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var brands = await _brandService.GetAllAsync();

            if (id == Guid.Empty)
            {
                // Thêm mới
                var model = new CreateOrUpdatePhoneViewModel
                {
                    Brands = brands.ToList()
                };
                ViewBag.IsEdit = false;
                return View(model);
            }
            else
            {
                // Chỉnh sửa
                var phone = await _phoneService.GetByIdAsync(id);
                if (phone == null)
                {
                    return NotFound();
                }

                var model = new CreateOrUpdatePhoneViewModel
                {
                    Model = phone.Model,
                    Price = phone.Price,
                    Stock = phone.Stock,
                    BrandId = phone.BrandId,
                    Brands = brands.ToList()
                };

                ViewBag.IsEdit = true;
                ViewBag.PhoneId = id;
                return View(model);
            }
        }

        /// <summary>
        /// Xử lý thêm mới điện thoại
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateOrUpdatePhoneViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                var brands = await _brandService.GetAllAsync();
                viewModel.Brands = brands.ToList();
                ViewBag.IsEdit = false;
                return View("Edit", viewModel);
            }

            var request = new CreatePhoneRequest
            {
                Model = viewModel.Model,
                Price = viewModel.Price,
                Stock = viewModel.Stock,
                BrandId = viewModel.BrandId
            };

            var result = await _phoneService.AddAsync(request);
            if (result)
            {
                TempData["SuccessMessage"] = "Thêm mới điện thoại thành công";
                TempData["FromPost"] = true;
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi thêm mới điện thoại");
                var brands = await _brandService.GetAllAsync();
                viewModel.Brands = brands.ToList();
                ViewBag.IsEdit = false;
                return View("Edit", viewModel);
            }
        }

        /// <summary>
        /// Xử lý cập nhật điện thoại
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Guid id, CreateOrUpdatePhoneViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                var brands = await _brandService.GetAllAsync();
                viewModel.Brands = brands.ToList();
                ViewBag.IsEdit = true;
                ViewBag.PhoneId = id;
                return View("Edit", viewModel);
            }

            var request = new UpdatePhoneRequest
            {
                Model = viewModel.Model,
                Price = viewModel.Price,
                Stock = viewModel.Stock,
                BrandId = viewModel.BrandId
            };

            var result = await _phoneService.UpdateAsync(id, request);
            if (result)
            {
                TempData["SuccessMessage"] = "Cập nhật điện thoại thành công";
                TempData["FromPost"] = true;
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi cập nhật điện thoại");
                var brands = await _brandService.GetAllAsync();
                viewModel.Brands = brands.ToList();
                ViewBag.IsEdit = true;
                ViewBag.PhoneId = id;
                return View("Edit", viewModel);
            }
        }
        /// <summary>
        /// API: Xóa điện thoại
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
                return this.BadRequestForInvalidId();

            var success = await _phoneService.DeleteAsync(id);
            if (!success)
                return NotFound(ApiResponse<bool>.ErrorResult(AppResources.PhoneNotFound));

            return Ok(ApiResponse<bool>.SuccessResult(true, AppResources.DeletePhoneSuccess));
        }
        /// <summary>
        /// API: Duyệt điện thoại
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Approve(Guid id)
        {
            var result = await _phoneService.Approve(id);

            if (!result)
            {
                return BadRequest(ApiResponse<bool>.ErrorResult("Có lỗi xảy ra khi duyệt điện thoại"));
            }

            return Ok(ApiResponse<bool>.SuccessResult(true, "Duyệt điện thoại thành công"));
        }

        /// <summary>
        /// API: Hủy duyệt điện thoại
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Reject(Guid id)
        {
            var result = await _phoneService.Reject(id);

            if (!result)
            {
                return BadRequest(ApiResponse<bool>.ErrorResult("Có lỗi xảy ra khi hủy duyệt điện thoại"));
            }

            return Ok(ApiResponse<bool>.SuccessResult(true, "Hủy duyệt điện thoại thành công"));
        }
    }
}
