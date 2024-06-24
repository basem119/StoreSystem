using Store_Sys.Modules;
using WebApplication1.Modules;
using WebApplication1.Repositories;

namespace Store_Sys.Repositories
{
    public interface ICustomerRepository: IRepository<Customer> 
    {
        Task<CustomerBalance> GetCustomerBalanceAsync(int customerId);
    }
}
