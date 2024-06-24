using Store_Sys.Modules;
using Store_Sys.Repositories;
using WebApplication1.Modules;
using WebApplication1.Repositories;

namespace WebApplication1.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Product> Products { get; }
        ICustomerRepository Customers { get; }
        IRepository<Bill> Bills { get; }
        IRepository<BillItem> BillItems { get; }
        IRepository<Transaction> Transactions { get; }
        IRepository<Employee> Employees { get; }
        Task<int> CompleteAsync();

    }
}
