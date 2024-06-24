using Microsoft.EntityFrameworkCore;
using Store_Sys.Modules;
using WebApplication1.Context;
using WebApplication1.Modules;
using WebApplication1.Repositories;

namespace Store_Sys.Repositories
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        private readonly StoreContext _context;

        public CustomerRepository(StoreContext context) : base(context)
        {
            _context = context;
        }

        public async Task<CustomerBalance> GetCustomerBalanceAsync(int customerId)
        {
            return await _context.CustomerBalances.FirstOrDefaultAsync(cb => cb.Id == customerId);
        }
    }
}
