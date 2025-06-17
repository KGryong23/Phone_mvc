﻿using Phone_mvc.Dtos;
using System.ComponentModel.DataAnnotations;

namespace Phone_mvc.Models
{
    /// <summary>
    /// ViewModel cho form tạo mới và cập nhật điện thoại
    /// </summary>
    public class CreateOrUpdatePhoneViewModel
    {
        /// <summary>
        /// Tên mẫu điện thoại (bắt buộc).
        /// </summary>
        [Required(ErrorMessage = "Tên mẫu điện thoại là bắt buộc")]
        [StringLength(100, ErrorMessage = "Tên mẫu không được vượt quá 100 ký tự")]
        [Display(Name = "Tên mẫu điện thoại")]
        public string Model { get; set; } = string.Empty;

        /// <summary>
        /// Giá của điện thoại (phải lớn hơn 0).
        /// </summary>
        [Required(ErrorMessage = "Giá là bắt buộc")]
        [Range(1, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0")]
        [Display(Name = "Giá")]
        public decimal Price { get; set; }

        /// <summary>
        /// Số lượng tồn kho của điện thoại (phải không âm).
        /// </summary>
        [Required(ErrorMessage = "Số lượng tồn kho là bắt buộc")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng tồn kho phải lớn hơn 0")]
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
        public List<BrandDto> Brands { get; set; } = new();
    }
}
