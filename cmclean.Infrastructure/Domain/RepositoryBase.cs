using System.Linq.Expressions;
using cmclean.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace cmclean.Infrastructure.Domain
{

    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private readonly CleanDbContext _context;

        public RepositoryBase(CleanDbContext context)
        {
            _context = context;
        }

        public IQueryable<T> FindAll()
        {
            return _context.Set<T>();
        }

        public async Task<List<T>> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().Where(expression).ToListAsync();
        }

        public async Task Create(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }
    }
}