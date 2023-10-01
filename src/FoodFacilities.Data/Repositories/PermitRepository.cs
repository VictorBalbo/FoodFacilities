using System.Linq.Expressions;
using FoodFacilities.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodFacilities.Data.Repositories;

public class PermitRepository : BaseRepository<Permit>, IPermitRepository
{
    private readonly FoodFacilitiesDbContext _context;
    private readonly DbSet<Permit> _dbSet;

    public PermitRepository(FoodFacilitiesDbContext context)
        : base(context, context.Permits)
    {
        _context = context;
        _dbSet = context.Permits;
    }

    public IAsyncEnumerable<Permit> GetOrderedAsync(Expression<Func<Permit, bool>> filter,
        Expression<Func<Permit, double>> orderBy, int take, CancellationToken cancellationToken)
    {
        return _dbSet
            .Where(filter)
            .OrderBy(orderBy)
            .Take(take)
            .AsAsyncEnumerable();
    }
}