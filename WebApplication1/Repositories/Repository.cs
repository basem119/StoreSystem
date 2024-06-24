using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebApplication1.Context;

namespace WebApplication1.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly StoreContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(StoreContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }
        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }
        public async Task UpdateAsync(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }
        public async Task<T> GetByIdWithDetailsAsync(int id, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            
            // Get the primary key property name
            var keyName = _context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties
                .Select(x => x.Name).Single();
            
            // Create expression for filtering by id
            var parameter = Expression.Parameter(typeof(T));
            var property = Expression.Property(parameter, keyName);
            var equals = Expression.Equal(property, Expression.Constant(id));
            var predicate = Expression.Lambda<Func<T, bool>>(equals, parameter);
            
            return await query.FirstOrDefaultAsync(predicate);        
        }
    public async Task<IEnumerable<T>> GetAllWithDetailsAsync(params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = _dbSet;
        foreach (var include in includes)
        {
            query = query.Include(include);
        }
        return await query.ToListAsync();
    }
        }

  

}
