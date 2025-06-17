using Phone_mvc.Common;
using Phone_mvc.Enums;
using System.ComponentModel.DataAnnotations;

namespace Phone_mvc.Dtos
{
    /// <summary>
    /// Đại diện cho thông tin chi tiết của một điện thoại để trả về.
    /// </summary>
    public class PhoneDto
    {
        /// <summary>
        /// Định danh duy nhất của điện thoại.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Tên mẫu điện thoại.
        /// </summary>
        public string Model { get; set; } = null!;

        /// <summary>
        /// Giá của điện thoại.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Số lượng tồn kho của điện thoại.
        /// </summary>
        public int Stock { get; set; }

        /// <summary>
        /// Ngày và giờ điện thoại được tạo.
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Ngày và giờ điện thoại được sửa lần cuối (không bắt buộc).
        /// </summary>
        public DateTime? LastModified { get; set; }

        /// <summary>
        /// Trạng thái kiểm duyệt của điện thoại.
        /// </summary>
        public ModerationStatus ModerationStatus { get; set; }

        /// <summary>
        /// Văn bản đại diện cho trạng thái kiểm duyệt.
        /// </summary>
        public string? ModerationStatusTxt { get; set; }

        /// <summary>
        /// ID của thương hiệu liên kết với điện thoại (không bắt buộc).
        /// </summary>
        public Guid? BrandId { get; set; }

        /// <summary>
        /// Tên của thương hiệu liên kết với điện thoại.
        /// </summary>
        public string BrandName { get; set; } = null!;
    }
    /// <summary>
    /// Đại diện cho yêu cầu tạo một điện thoại mới.
    /// </summary>
    public class CreatePhoneRequest
    {
        /// <summary>
        /// Tên mẫu điện thoại (bắt buộc).
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(AppResources), ErrorMessageResourceName = "EmptyModel")]
        public string Model { get; set; } = null!;

        /// <summary>
        /// Giá của điện thoại (phải lớn hơn 0).
        /// </summary>
        [Range(0.01, double.MaxValue, ErrorMessageResourceType = typeof(AppResources), ErrorMessageResourceName = "InvalidPrice")]
        public decimal Price { get; set; }

        /// <summary>
        /// Số lượng tồn kho của điện thoại (phải không âm).
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessageResourceType = typeof(AppResources), ErrorMessageResourceName = "InvalidStock")]
        public int Stock { get; set; }

        /// <summary>
        /// ID của thương hiệu liên kết với điện thoại (không bắt buộc).
        /// </summary>
        public Guid? BrandId { get; set; }
    }
    /// <summary>
    /// Đại diện cho yêu cầu cập nhật một điện thoại hiện có.
    /// </summary>
    public class UpdatePhoneRequest
    {
        /// <summary>
        /// Tên mẫu điện thoại (bắt buộc).
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(AppResources), ErrorMessageResourceName = "EmptyModel")]
        public string Model { get; set; } = null!;

        /// <summary>
        /// Giá của điện thoại (phải lớn hơn 0).
        /// </summary>
        [Range(0.01, double.MaxValue, ErrorMessageResourceType = typeof(AppResources), ErrorMessageResourceName = "InvalidPrice")]
        public decimal Price { get; set; }

        /// <summary>
        /// Số lượng tồn kho của điện thoại (phải không âm).
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessageResourceType = typeof(AppResources), ErrorMessageResourceName = "InvalidStock")]
        public int Stock { get; set; }

        /// <summary>
        /// ID của thương hiệu liên kết với điện thoại (không bắt buộc).
        /// </summary>
        public Guid? BrandId { get; set; }
    }
}
