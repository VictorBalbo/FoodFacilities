using FoodFacilities.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FoodFacilities.Data;

/// <summary>
/// Responsible for registering dependencies for Dependency Injection pattern
/// </summary>
public static class Registrable
{
    public static void RegisterDataDependencies(this IServiceCollection services, string databaseName)
    {
        services.AddScoped<IPermitRepository, PermitRepository>();
        services.AddDbContext<FoodFacilitiesDbContext>(options => options.UseInMemoryDatabase(databaseName));
    }
}