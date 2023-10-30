using cmclean.Domain.Entities;

namespace cmclean.Application.Interfaces.Repositories;

public interface IGenericWriteRepository<T> where T : BaseEntity
{
    Task<T> AddAsync(T entity);
    T Update(T entity);
    bool Remove(T entity);
    Task<int> SaveChangesAsync();
}