using Phone_mvc.Data;
using Phone_mvc.Entities;

namespace Phone_mvc.Repositories
{
    public class RolePermissionRepository : Repository<RolePermission>, IRolePermissionRepository
    {
        public RolePermissionRepository(PhoneContext context) : base(context)
        {
        }
    }
}
