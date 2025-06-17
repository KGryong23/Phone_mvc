﻿namespace Phone_mvc.Entities
{
    /// <summary>
    /// Đại diện cho một vai trò trong hệ thống, dùng để phân quyền.
    /// </summary>
    public class Role : BaseDomainEntity
    {
        private string _name = null!;

        /// <summary>
        /// Tên của vai trò, phải là duy nhất.
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Role name cannot be empty.");
                _name = value;
            }
        }
        /// <summary>
        /// Danh sách người dùng thuộc vai trò này, đại diện cho mối quan hệ nhiều-nhiều.
        /// </summary>
        public ICollection<UserRole>? UserRoles { get; set; }

        /// <summary>
        /// Danh sách quyền (Permission) được gán cho vai trò này, đại diện cho mối quan hệ nhiều-nhiều.
        /// </summary>
        public ICollection<RolePermission>? RolePermissions { get; set; }
    }
}
