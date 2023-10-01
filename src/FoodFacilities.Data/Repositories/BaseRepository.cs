using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FoodFacilities.Data.Repositories;

public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class 
{
    private readonly FoodFacilitiesDbContext _dbContext;
    private readonly DbSet<TEntity> _dbSet;

    protected BaseRepository(FoodFacilitiesDbContext dbContext, DbSet<TEntity> dbSet)
    {
        _dbContext = dbContext;
        _dbSet = dbSet;
    }

    public IAsyncEnumerable<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter)
    {
        return _dbSet.Where(filter).AsAsyncEnumerable();
    }
    
    public IAsyncEnumerable<TEntity> GetAllAsync()
    {
        return _dbSet.AsAsyncEnumerable();
    }

    public Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        return _dbSet.AddRangeAsync(entities, cancellationToken);
    }

    public Task RemoveAllAsync(CancellationToken cancellationToken)
    {
        var entities = GetAllAsync().ToBlockingEnumerable(cancellationToken);
        _dbSet.RemoveRange(entities);
        return Task.CompletedTask;
    }

    public Task CommitAsync(CancellationToken cancellationToken)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}