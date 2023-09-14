using System.Linq.Expressions;

namespace cmclean.Domain.Repositories
{

    public interface IRepositoryBase<T>
    {
        IQueryable<T> FindAll();
        Task<List<T>> FindByCondition(Expression<Func<T, bool>> expression);
        Task Create(T entity);
        void Update(T entity);
        void Delete(T entity);

    }
}