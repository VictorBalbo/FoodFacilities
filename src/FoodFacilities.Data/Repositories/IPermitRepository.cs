using System.Linq.Expressions;
using FoodFacilities.Models;

namespace FoodFacilities.Data.Repositories;

public interface IPermitRepository : IRepository<Permit>
{
    /// <summary>
    /// Get a list of permits filtered and ordered by a specified criterias
    /// </summary>
    /// <param name="filter">Filter expression to be applied to the list</param>
    /// <param name="orderBy">Order expression to be applied to the list</param>
    /// <param name="take">Amount of permits to be returned</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public IAsyncEnumerable<Permit> GetOrderedAsync(Expression<Func<Permit, bool>> filter,
        Expression<Func<Permit, double>> orderBy, int take, CancellationToken cancellationToken);
}