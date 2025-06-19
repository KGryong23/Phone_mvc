using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using Phone_mvc.Repositories;

namespace Phone_mvc.Helpers
{
    public static class PermissionHtmlHelper
    {
        public static bool HasPermission(this IHtmlHelper htmlHelper, string endpoint)
        {
            var httpContext = htmlHelper.ViewContext.HttpContext;
            var userIdClaim = httpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                return false;
            }

            var memoryCache = httpContext.RequestServices.GetRequiredService<IMemoryCache>();
            var userRoleRepository = httpContext.RequestServices.GetRequiredService<IUserRoleRepository>();
            var permissionRepository = httpContext.RequestServices.GetRequiredService<IPermissionRepository>();

            var roleIds = GetRoleIdsAsync(userId, memoryCache, userRoleRepository).Result;
            if (roleIds == null || !roleIds.Any())
            {
                return false;
            }

            var method = GetHttpMethod(endpoint); // Xác định phương thức dựa trên endpoint
            var requiredPermission = $"{endpoint.ToLower()}:{method.ToLower()}";
            return HasPermissionAsync(roleIds, requiredPermission, memoryCache, permissionRepository).Result;
        }

        private static string GetHttpMethod(string endpoint)
        {
            var lowerEndpoint = endpoint.ToLower();
            if (lowerEndpoint.Contains("create") || lowerEndpoint.Contains("update") ||
                lowerEndpoint.Contains("approve") || lowerEndpoint.Contains("reject") ||
                lowerEndpoint.Contains("delete"))
            {
                return "post";
            }
            return "get"; // Mặc định là get
        }

        private static async Task<List<Guid>> GetRoleIdsAsync(Guid userId, IMemoryCache memoryCache, IUserRoleRepository userRoleRepository)
        {
            if (!memoryCache.TryGetValue($"user_{userId}", out List<Guid>? roleIds))
            {
                roleIds = (await userRoleRepository.FindAllAsync(ur => ur.UserId == userId))
                    ?.Select(ur => ur.RoleId)
                    .ToList() ?? new List<Guid>();

                memoryCache.Set($"user_{userId}", roleIds, new MemoryCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromHours(1)
                });
            }
            return roleIds ?? [];
        }

        private static async Task<bool> HasPermissionAsync(List<Guid> roleIds, string requiredPermission, IMemoryCache memoryCache, IPermissionRepository permissionRepository)
        {
            foreach (var roleId in roleIds)
            {
                if (!memoryCache.TryGetValue($"role_{roleId}", out List<string>? rolePermissions))
                {
                    rolePermissions = (await permissionRepository.FindAllAsync(
                        p => p.RolePermissions!.Any(rp => rp.RoleId == roleId),
                        p => p.RolePermissions!))
                        ?.Select(p => $"{p.Endpoint}:{p.Method}")
                        .ToList() ?? new List<string>();

                    memoryCache.Set($"role_{roleId}", rolePermissions, new MemoryCacheEntryOptions
                    {
                        SlidingExpiration = TimeSpan.FromHours(1)
                    });
                }

                if (rolePermissions?.Contains(requiredPermission, StringComparer.OrdinalIgnoreCase) == true)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
