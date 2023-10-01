using FoodFacilities.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodFacilities.Data;

/// <summary>
/// Database Context to be used by Repositories
/// </summary>
public sealed class FoodFacilitiesDbContext : DbContext
{
    public DbSet<Permit> Permits { get; set; }

    public FoodFacilitiesDbContext(DbContextOptions options)
        : base(options)
    {
        Permits = Set<Permit>();
    }
}