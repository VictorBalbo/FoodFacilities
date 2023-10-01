using FoodFacilities.Models;

namespace FoodFacilitiesAPI.Managers;

/// <summary>
/// Responsible to manage <see cref="Permit"/> business logic
/// </summary>
public interface IPermitManager
{
    /// <summary>
    /// Get a <see cref="Permit"/> filtered by Applicant Name and Status
    /// </summary>
    /// <param name="applicantName"></param>
    /// <param name="status"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<IAsyncEnumerable<Permit>> GetByApplicantAsync(string applicantName, string? status, CancellationToken cancellationToken);
    
    /// <summary>
    /// Get a <see cref="Permit"/> filtered by Address 
    /// </summary>
    /// <param name="address"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<IAsyncEnumerable<Permit>> GetByAddressAsync(string address, CancellationToken cancellationToken);
    
    /// <summary>
    /// Get a <see cref="Permit"/> filtered by Status and ordered by distance to a specified coorditate
    /// </summary>
    /// <param name="latitude"></param>
    /// <param name="longitude"></param>
    /// <param name="status"></param>
    /// <param name="take"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<IAsyncEnumerable<Permit>> GetByNearestAsync(double latitude, double longitude, string status, int take, CancellationToken cancellationToken);
}