using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Caching.Memory;
using Phone_mvc.Repositories;
using System.Security.Claims;

namespace Phone_mvc.Helpers
{
    [HtmlTargetElement("authorize-menu")]
    public class AuthorizeMenuTagHelper(IHttpContextAccessor httpContextAccessor, IMemoryCache memoryCache) : TagHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IMemoryCache _memoryCache = memoryCache;

        [HtmlAttributeName("asp-endpoint")]
        public string Endpoint { get; set; } = "";

        [HtmlAttributeName("asp-method")]
        public string Method { get; set; } = "get";

        [ViewContext]
        [HtmlAttributeNotBound]
        public required ViewContext ViewContext { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                output.SuppressOutput(); // Ẩn menu nếu không có userId
                return;
            }

            // Lấy roleIds từ cache
            if (!_memoryCache.TryGetValue($"user_{userId}", out List<Guid>? roleIds))
            {
                using var scope = _httpContextAccessor.HttpContext!.RequestServices.CreateScope();
                var userRoleRepository = scope.ServiceProvider.GetRequiredService<IUserRoleRepository>();
                roleIds = (await userRoleRepository.FindAllAsync(ur => ur.UserId == userId))
                    .Select(ur => ur.RoleId)
                    .ToList();

                _memoryCache.Set($"user_{userId}", roleIds ?? new List<Guid>(), new MemoryCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromHours(1)
                });
            }

            if (roleIds == null || !roleIds.Any())
            {
                output.SuppressOutput(); // Ẩn menu nếu không có role
                return;
            }

            // Kiểm tra quyền
            var requiredPermission = $"{Endpoint.ToLower()}:{Method.ToLower()}";
            bool hasPermission = false;

            foreach (var roleId in roleIds)
            {
                if (!_memoryCache.TryGetValue($"role_{roleId}", out List<string>? rolePermissions))
                {
                    using var scope = _httpContextAccessor.HttpContext!.RequestServices.CreateScope();
                    var permissionRepository = scope.ServiceProvider.GetRequiredService<IPermissionRepository>();
                    rolePermissions = (await permissionRepository.FindAllAsync(
                        p => p.RolePermissions!.Any(rp => rp.RoleId == roleId),
                        p => p.RolePermissions!))
                        ?.Select(p => $"{p.Endpoint}:{p.Method}")
                        .ToList();

                    _memoryCache.Set($"role_{roleId}", rolePermissions ?? new List<string>(), new MemoryCacheEntryOptions
                    {
                        SlidingExpiration = TimeSpan.FromHours(1)
                    });
                }

                if (rolePermissions?.Contains(requiredPermission, StringComparer.OrdinalIgnoreCase) == true)
                {
                    hasPermission = true;
                    break;
                }
            }

            if (!hasPermission)
            {
                output.SuppressOutput(); // Ẩn menu nếu không có quyền
            }
        }
    }
}
