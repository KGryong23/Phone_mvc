using Phone_mvc.Dtos;

namespace Phone_mvc.Services
{
    /// <summary>
    /// Giao diện định nghĩa phương thức lấy tất cả quyền.
    /// </summary>
    public interface IPermissionService
    {
        /// <summary>
        /// Lấy danh sách tất cả quyền.
        /// </summary>
        /// <returns>Danh sách PermissionDto.</returns>
        Task<IEnumerable<PermissionDto>> GetAllAsync();
        Task SyncPermissionsAsync();
    }
}
