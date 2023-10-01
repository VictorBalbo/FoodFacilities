using FoodFacilities.Models;

namespace FoodFacilitiesAPI.Providers;

/// <summary>
/// Provider responsible for fetching Permits data from external source 
/// </summary>
public interface IPermitProvider
{
    /// <summary>
    /// Last DateTime the cache was updated
    /// </summary>
    public DateTimeOffset LastCacheUpdate { get; }

    /// <summary>
    /// Fetch <see cref="Permit"/> from external source 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>List of Permit</returns>
    public Task<IEnumerable<Permit>> FetchPermitAsync(CancellationToken cancellationToken);
}