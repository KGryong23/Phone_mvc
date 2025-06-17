using Phone_mvc.Data;
using Phone_mvc.Entities;

namespace Phone_mvc.Repositories
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(PhoneContext context) : base(context)
        {
        }
    }
}
