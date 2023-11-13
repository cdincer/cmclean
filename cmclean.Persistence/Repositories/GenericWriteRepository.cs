using cmclean.Application.Interfaces.Repositories;
using cmclean.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace cmclean.Persistence.Repositories;

public class GenericWriteRepository<T> : IGenericWriteRepository<T> where T : BaseEntity
{
    private readonly CmcleanDbContext dbContext;
    private readonly IConfiguration _configuration;
    public GenericWriteRepository(CmcleanDbContext dbContext, IConfiguration configuration)
    {
        this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        await dbContext.Set<T>().AddAsync(entity);
        return entity;
    }

    public virtual T Update(T entity)
    {
        dbContext.Set<T>().Update(entity);
        return entity;
    }

    public virtual bool Remove(T entity)
    {
        dbContext.Set<T>().Remove(entity);
        return true;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await dbContext.SaveChangesAsync();
    }

    public Task<T> CreateAsync(T entity)
    {
        throw new NotImplementedException();
    }
}