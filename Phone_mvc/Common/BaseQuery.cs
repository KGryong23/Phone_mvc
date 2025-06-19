using System.ComponentModel.DataAnnotations;

namespace Phone_mvc.Common
{
    public class BaseQuery
    {
        /// <summary>
        /// Từ khóa tìm kiếm để lọc danh sách điện thoại (tùy chọn).
        /// </summary>
        public string? Keyword { get; set; }

        /// <summary>
        /// Số bản ghi bỏ qua (dùng cho phân trang). Phải lớn hơn hoặc bằng 0.
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessageResourceType = typeof(AppResources), ErrorMessageResourceName = nameof(AppResources.SkipInvalid))]
        public int Skip { get; set; }

        /// <summary>
        /// Số bản ghi cần lấy (dùng cho phân trang). Phải lớn hơn 0.
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(AppResources), ErrorMessageResourceName = nameof(AppResources.TakeInvalid))]
        public int Take { get; set; }

        /// <summary>
        /// Số thứ tự của request (dùng cho DataTable).
        /// </summary>
        public int Draw { get; set; }

        /// <summary>
        /// Trường dùng để sắp xếp (mặc định là "Id").
        /// </summary>
        public string SortField { get; set; } = "Id";

        /// <summary>
        /// Hướng sắp xếp: "asc" hoặc "desc" (mặc định là "desc").
        /// </summary>
        [RegularExpression("^(asc|desc)$", ErrorMessageResourceType = typeof(AppResources))]
        public string SortDirection { get; set; } = "desc";

        /// <summary>
        /// ID của người tạo để lọc (tùy chọn).
        /// </summary>
        public Guid? CreatedBy { get; set; }
    }
}
