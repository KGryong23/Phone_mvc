using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Phone_mvc.Repositories;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace Phone_mvc.Filters
{
    /// <summary>
    /// Attribute kiểm tra quyền dựa trên JWT token và PermissionMap.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class RequirePermissionAttribute() : AuthorizeAttribute, IAuthorizationFilter
    {
        /// <summary>
        /// Sử dụng attribute này để yêu cầu quyền truy cập cho các endpoint cụ thể.
        /// </summary>
        public async void OnAuthorization(AuthorizationFilterContext context)
        {
            var userIdClaim = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                context.Result = new ForbidResult();
                return;
            }

            using var scope = context.HttpContext.RequestServices.CreateScope();
            var cache = scope.ServiceProvider.GetRequiredService<IMemoryCache>();
            var permissionRepository = scope.ServiceProvider.GetRequiredService<IPermissionRepository>();
            var userRoleRepository = scope.ServiceProvider.GetRequiredService<IUserRoleRepository>();

            var endpoint = NormalizeEndpoint(context.HttpContext.Request.Path.Value!);
            var method = context.HttpContext.Request.Method;
            var requiredPermission = $"{endpoint}:{method}";

            if (!cache.TryGetValue($"user_{userId}", out List<Guid>? roleIds))
            {
                roleIds = (await userRoleRepository.FindAllAsync(ur => ur.UserId == userId))
                    .Select(ur => ur.RoleId)
                    .ToList();

                cache.Set($"user_{userId}", roleIds, new MemoryCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromHours(1)
                });
            }

            if (roleIds is null || !roleIds.Any())
            {
                context.Result = new ForbidResult();
                return;
            }

            foreach (var roleId in roleIds)
            {
                if (!cache.TryGetValue($"role_{roleId}", out List<string>? rolePermissions))
                {
                    rolePermissions = await GetRolePermissionsAsync(permissionRepository, roleId);
                    cache.Set($"role_{roleId}", rolePermissions, new MemoryCacheEntryOptions
                    {
                        SlidingExpiration = TimeSpan.FromHours(1)
                    });
                }

                if (rolePermissions?.Contains(requiredPermission, StringComparer.OrdinalIgnoreCase) == true)
                    return;
            }

            context.Result = new ForbidResult();
        }

        private static string NormalizeEndpoint(string endpoint)
        {
            var pattern = @"^/(\w+)/(\w+)(?:/([0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}|guid\.empty)?)?$";
            var match = Regex.Match(endpoint, pattern);
            if (match.Success)
            {
                var controller = match.Groups[1].Value;
                var action = match.Groups[2].Value;
                return $"/{controller}/{action}";
            }
            return endpoint;
        }

        private async Task<List<string>> GetRolePermissionsAsync(IPermissionRepository permissionRepository, Guid roleId)
        {
            return (await permissionRepository.FindAllAsync(
                p => p.RolePermissions!.Any(rp => rp.RoleId == roleId),
                p => p.RolePermissions!))
                .Select(p => $"{p.Endpoint}:{p.Method}")
                .ToList();
        }
    }

}
