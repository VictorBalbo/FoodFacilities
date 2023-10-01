using System.Linq.Expressions;
using FoodFacilities.Data.Repositories;
using FoodFacilities.Models;
using FoodFacilitiesAPI.Providers;
using IConfiguration = FoodFacilitiesAPI.Configurations.IConfiguration;

namespace FoodFacilitiesAPI.Managers;

public class PermitManager : IPermitManager
{
    private readonly TimeSpan _cacheDuration;
    private readonly IPermitProvider _permitProvider;
    private readonly IPermitRepository _permitRepository;
    private readonly IDateTimeOffsetProvider _dateTimeOffsetProvider;

    public PermitManager(IPermitProvider permitProvider, IConfiguration configuration,
        IPermitRepository permitRepository, IDateTimeOffsetProvider dateTimeOffsetProvider)
    {
        _cacheDuration = configuration.CacheDuration;
        _permitProvider = permitProvider;
        _permitRepository = permitRepository;
        _dateTimeOffsetProvider = dateTimeOffsetProvider;
    }

    public async Task<IAsyncEnumerable<Permit>> GetByApplicantAsync(string applicantName, string? status,
        CancellationToken cancellationToken)
    {
        Expression<Func<Permit, bool>> filter;
        if (string.IsNullOrEmpty(status))
        {
            filter = (p) => p.Applicant.Equals(applicantName, StringComparison.OrdinalIgnoreCase);
        }
        else
        {
            filter = (p) =>
                p.Applicant.Equals(applicantName, StringComparison.OrdinalIgnoreCase) &&
                p.Status.Equals(status, StringComparison.OrdinalIgnoreCase);
        }

        return await GetPermitAsync(filter, cancellationToken);
    }

    public async Task<IAsyncEnumerable<Permit>> GetByAddressAsync(string address, CancellationToken cancellationToken)
    {
        Expression<Func<Permit, bool>> filter = (p) =>
            p.Address.Contains(address, StringComparison.OrdinalIgnoreCase);

        return await GetPermitAsync(filter, cancellationToken);
    }

    public async Task<IAsyncEnumerable<Permit>> GetByNearestAsync(double latitude, double longitude, string status,
        int take,
        CancellationToken cancellationToken)
    {
        Expression<Func<Permit, bool>> filter = (p) =>
            p.Status.Equals(status, StringComparison.OrdinalIgnoreCase);

        Expression<Func<Permit, double>> orderBy = (p) =>
            ((p.Latitude - latitude) * (p.Latitude - latitude)) +
            ((p.Longitude - longitude) * (p.Longitude - longitude));

        await EnsureCacheIsUpToDate(cancellationToken);
        return _permitRepository.GetOrderedAsync(filter, orderBy, take, cancellationToken);
    }

    private async Task<IAsyncEnumerable<Permit>> GetPermitAsync(Expression<Func<Permit, bool>> filter,
        CancellationToken cancellationToken)
    {
        await EnsureCacheIsUpToDate(cancellationToken);
        return _permitRepository.GetAsync(filter);
    }

    private async Task EnsureCacheIsUpToDate(CancellationToken cancellationToken)
    {
        if (_permitProvider.LastCacheUpdate.Add(_cacheDuration) < _dateTimeOffsetProvider.Now)
        {
            var permits = await _permitProvider.FetchPermitAsync(cancellationToken);

            await _permitRepository.RemoveAllAsync(cancellationToken);
            await _permitRepository.AddRangeAsync(permits, cancellationToken);
            await _permitRepository.CommitAsync(cancellationToken);
        }
    }
}