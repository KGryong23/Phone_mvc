using Phone_mvc.Dtos;
using System.ComponentModel.DataAnnotations;

namespace Phone_mvc.Models
{
    /// <summary>
    /// ViewModel cho form tạo mới điện thoại
    /// </summary>
    public class CreatePhoneViewModel : PhoneViewModelBase
    {
        /// <summary>
        /// Tên mẫu điện thoại (bắt buộc).
        /// </summary>
        [Required(ErrorMessage = "Tên mẫu điện thoại là bắt buộc")]
        [Display(Name = "Tên mẫu điện thoại")]
        public string Model { get; set; } = string.Empty;

        /// <summary>
        /// Giá của điện thoại (phải lớn hơn 0).
        /// </summary>
        [Range(0.01, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0")]
        [Display(Name = "Giá")]
        public decimal Price { get; set; }

        /// <summary>
        /// Số lượng tồn kho của điện thoại (phải không âm).
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng tồn kho không được âm")]
        [Display(Name = "Số lượng tồn kho")]
        public int Stock { get; set; }

        /// <summary>
        /// ID của thương hiệu liên kết với điện thoại (không bắt buộc).
        /// </summary>
        [Display(Name = "Thương hiệu")]
        public Guid? BrandId { get; set; }

        /// <summary>
        /// Danh sách thương hiệu để hiển thị trong dropdown
        /// </summary>
        public IEnumerable<BrandDto> Brands { get; set; } = [];
    }
}
