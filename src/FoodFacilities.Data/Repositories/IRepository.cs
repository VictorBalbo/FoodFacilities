using System.Linq.Expressions;

namespace FoodFacilities.Data.Repositories;

public interface IRepository<TEntity> where TEntity : class 
{
    /// <summary>
    /// Get a list of <see cref="TEntity"/> filtered by a specific criteria
    /// </summary>
    /// <param name="filter">Filter expression to be applied to the list</param>
    /// <returns></returns>
    public IAsyncEnumerable<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter);
    
    /// <summary>
    /// Get a list of all <see cref="TEntity"/>
    /// </summary>
    /// <returns></returns>
    public IAsyncEnumerable<TEntity> GetAllAsync();
    
    /// <summary>
    /// Add a list of <see cref="TEntity"/> to the database
    /// </summary>
    /// <param name="entities">List of <see cref="TEntity"/> to be added</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);
    
    /// <summary>
    /// Remove all <see cref="TEntity"/> from the database
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task RemoveAllAsync(CancellationToken cancellationToken);
    
    /// <summary>
    /// Save the changes on the database
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task CommitAsync(CancellationToken cancellationToken);


}