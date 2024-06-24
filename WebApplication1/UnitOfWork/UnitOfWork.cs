using Store_Sys.Modules;
using Store_Sys.Repositories;
using WebApplication1.Context;
using WebApplication1.Modules;
using WebApplication1.Repositories;

namespace WebApplication1.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _context;
        public IRepository<Product> Products { get; private set; }
        public ICustomerRepository Customers { get; private set; }
        public IRepository<Bill> Bills { get; private set; }
        public IRepository<BillItem> BillItems { get; private set; }
        public IRepository<Transaction> Transactions { get; private set; }
        public IRepository<Employee> Employees { get; private set; }

        public UnitOfWork(StoreContext context)
        {
            _context = context;
            Products = new Repository<Product>(context);
            Customers = new CustomerRepository(context);
            Bills = new Repository<Bill>(context);
            BillItems = new Repository<BillItem>(context);
            Transactions = new Repository<Transaction>(context);
            Employees = new Repository<Employee>(context);
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
       
    }
}
