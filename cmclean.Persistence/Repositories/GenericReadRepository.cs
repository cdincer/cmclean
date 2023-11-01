using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using cmclean.Application.Interfaces.Repositories;
using cmclean.Domain.Entities;

namespace cmclean.Persistence.Repositories;

public class GenericReadRepository<T> : IGenericReadRepository<T> where T : BaseEntity
{
    private readonly CmcleanDbContext dbContext;

    public GenericReadRepository(CmcleanDbContext dbContext)
    {
        this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public virtual Task<List<T?>> Get(bool asNoTracking = false, Expression<Func<T?, bool>>? filter = null, params Expression<Func<T, object?>>[] includes)
    {
        return Get(asNoTracking, filter, null, includes);
    }

    public virtual async Task<List<T?>> Get(bool asNoTracking = false, Expression<Func<T?, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, params Expression<Func<T, object?>>[] includes)
    {
        IQueryable<T> query = dbContext.Set<T>();


        foreach (var include in includes)
        {
            query = query.Include(include)!;
        }


        if (filter != null)
        {
            query = query.Where(filter)!;
        }

        if (orderBy != null)
        {
            query = orderBy(query)!;
        }

        if (asNoTracking)
        {
            return (await query.AsNoTracking().ToListAsync())!;
        }

        return (await query!.ToListAsync())!;
    }

    public virtual async Task<List<T?>> GetAll(bool asNoTracking = false)
    {
        try
        {
            if (asNoTracking)
            {
                return (await dbContext.Set<T>().AsNoTracking().ToListAsync())!;
            }

            return (await dbContext.Set<T>().ToListAsync())!;

        }
        catch (Exception ex)
        {
            Console.WriteLine("Database connection or table creation failed" + ex.Message);
            return new List<T>();
        }
    }

    public virtual async Task<T?> GetByIdAsync(Guid id, bool asNoTracking = false, params Expression<Func<T, object?>>[] includes)
    {
        IQueryable<T> query = dbContext.Set<T>();

        foreach (var include in includes)
        {
            query = query.Include(include)!;
        }

        if (asNoTracking)
        {
            return await query.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
        }

        return await query.FirstOrDefaultAsync(i => i.Id == id);
    }

    public virtual async Task<T?> GetSingleAsync(Expression<Func<T, bool>> filter, bool asNoTracking = false, params Expression<Func<T, object?>>[] includes)
    {
        IQueryable<T> query = dbContext.Set<T>();

        foreach (var include in includes)
        {
            query = query.Include(include!);
        }

        if (asNoTracking)
        {
            return await query.Where(filter).AsNoTracking().SingleOrDefaultAsync();
        }

        return await query.Where(filter).SingleOrDefaultAsync();
    }
}