using Phone_mvc.Data;
using Phone_mvc.Entities;

namespace Phone_mvc.Repositories
{
    /// <summary>
    /// Repository cho Phone, kế thừa Generic Repository
    /// </summary>
    public class PhoneRepository : Repository<Phone>, IPhoneRepository
    {
        public PhoneRepository(PhoneContext context) : base(context)
        {
        }
    }
}
