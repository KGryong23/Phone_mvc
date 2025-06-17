using Phone_mvc.Data;
using Phone_mvc.Entities;

namespace Phone_mvc.Repositories
{
    public class UserRoleRepository : Repository<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(PhoneContext context) : base(context)
        {
        }
    }
}
