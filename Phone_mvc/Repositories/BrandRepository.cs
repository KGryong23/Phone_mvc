using Phone_mvc.Data;
using Phone_mvc.Entities;

namespace Phone_mvc.Repositories
{
    /// <summary>
    /// Repository cho Brand, kế thừa Generic Repository
    /// </summary>
    public class BrandRepository : Repository<Brand>, IBrandRepository
    {
        public BrandRepository(PhoneContext context) : base(context)
        {
        }
    }
}
