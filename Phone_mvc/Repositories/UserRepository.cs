using Phone_mvc.Data;
using Phone_mvc.Entities;

namespace Phone_mvc.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(PhoneContext context) : base(context)
        {
        }
    }
}
