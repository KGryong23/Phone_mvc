namespace Phone_mvc.Entities
{
    /// <summary>
    /// Đại diện cho một quyền trong hệ thống, xác định endpoint và phương thức HTTP.
    /// </summary>
    public class Permission : BaseDomainEntity
    {
        private string _controller = null!;
        /// <summary>
        /// Tên của quyền, thường là tên của controller trong MVC.
        /// </summary>
        public string Controller
        {
            get => _controller;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Controller cannot be empty.", nameof(value));
                _controller = value;
            }
        }
        private string _endpoint = null!;
        /// <summary>
        /// Endpoint mà quyền này áp dụng, thường là URL hoặc tên API.   
        /// </summary>
        public string Endpoint
        {
            get => _endpoint;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Endpoint cannot be empty.", nameof(value));
                _endpoint = value;
            }
        }
        private string _method = null!;
        /// <summary>
        /// Mô tả về quyền, giúp nhận diện dễ dàng hơn.
        /// </summary>
        public string Method
        {
            get => _method;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Method cannot be empty.", nameof(value));
                _method = value;
            }
        }
        private bool _isApi = true;
        /// <summary>
        /// Xác định xem quyền này có phải là API hay không. Nếu là true, quyền này sẽ được sử dụng cho các API.
        /// </summary>
        public bool IsApi
        {
            get => _isApi;
            set => _isApi = value;
        }
        /// <summary>
        /// Danh sách vai trò được gán quyền này, đại diện cho mối quan hệ nhiều-nhiều.
        /// </summary>
        public ICollection<RolePermission>? RolePermissions { get; set; }
    }
}
