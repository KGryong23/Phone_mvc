using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.Routing;
using Phone_mvc.Controllers;
using Phone_mvc.Dtos;
using Phone_mvc.Entities;
using Phone_mvc.Repositories;
using System.Reflection;

namespace Phone_mvc.Services
{
    /// <summary>
    /// Dịch vụ quản lý quyền, sử dụng IPermissionRepository.
    /// </summary>
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository;

        /// <summary>
        /// Khởi tạo PermissionService với repository.
        /// </summary>
        /// <param name="permissionRepository">Repository cho Permission.</param>
        public PermissionService(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        /// <summary>
        /// Lấy danh sách tất cả quyền.
        /// </summary>
        /// <returns>Danh sách PermissionDto.</returns>
        public async Task<IEnumerable<PermissionDto>> GetAllAsync()
        {
            var permissions = await _permissionRepository.GetAllAsync();
            return permissions.Select(p => new PermissionDto
            {
                Id = p.Id,
                Endpoint = p.Endpoint,
                Method = p.Method
            });
        }

        public async Task SyncPermissionsAsync()
        {
            var existingPermissions = await _permissionRepository.GetAllAsync();
            var controllerTypes = new[] {
                typeof(PhoneController),
                typeof(RoleController),
                typeof(BrandController),
                typeof(PermissionController)
            }; // Danh sách controller, có thể mở rộng
            var allPermissions = GetPermissionsFromControllers(controllerTypes);

            foreach (var permission in allPermissions)
            {
                if (!existingPermissions.Any(p => p.Controller == permission.Controller &&
                                                p.Endpoint == permission.Endpoint &&
                                                p.Method == permission.Method))
                {
                    await _permissionRepository.AddAsync(permission);
                }
            }
            await _permissionRepository.SaveChangesAsync();
        }

        private List<Permission> GetPermissionsFromControllers(Type[] controllerTypes)
        {
            var permissions = new List<Permission>();

            foreach (var controllerType in controllerTypes)
            {
                var methods = controllerType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                    .Where(m => !m.IsSpecialName); // Loại bỏ getter, setter, v.v.

                foreach (var method in methods)
                {
                    var routeAttribute = method.GetCustomAttributes(typeof(RouteAttribute), true)
                        .FirstOrDefault() as RouteAttribute;
                    var httpMethodAttribute = method.GetCustomAttributes(true)
                        .OfType<HttpMethodAttribute>()
                        .FirstOrDefault();

                    string controllerName = controllerType.Name.Replace("Controller", "").ToLower();
                    string endpoint;
                    // Xử lý đặc biệt cho Index: đặt thành /phone (hoặc controller tương ứng)
                    if (method.Name.ToLower() == "index" && routeAttribute == null)
                    {
                        endpoint = $"/{controllerName}"; // Chỉ /phone cho Index
                    }
                    else
                    {
                        endpoint = routeAttribute?.Template?.ToLower() ?? $"/{controllerName}/{method.Name.ToLower()}";
                    }
                    string methodName = httpMethodAttribute?.HttpMethods.FirstOrDefault()?.ToLower() ?? "get";

                    var permission = new Permission
                    {
                        Controller = controllerName,
                        Endpoint = endpoint,
                        Method = methodName
                    };
                    permissions.Add(permission);
                }
            }

            return permissions;
        }
    }
}
