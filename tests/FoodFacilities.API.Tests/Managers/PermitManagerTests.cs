using System.Linq.Expressions;
using FoodFacilities.Data.Repositories;
using FoodFacilities.Models;
using FoodFacilitiesAPI.Configurations;
using FoodFacilitiesAPI.Managers;
using FoodFacilitiesAPI.Providers;
using NSubstitute;

namespace FoodFacilities.API.Tests.Managers;

[TestClass]
public class PermitManagerTests
{
    private IConfiguration _configuration;
    private IPermitRepository _permitRepository;
    private IPermitProvider _permitProvider;
    private IDateTimeOffsetProvider _dateTimeOffsetProvider;
    private IPermitManager _permitManager;
    private CancellationToken _cancellationToken;

    [TestInitialize]
    public void InitializeTest()
    {
        _configuration = new Configuration()
        {
            CacheDuration = TimeSpan.FromMinutes(5),
            DatabaseName = "Test",
        };
        _permitProvider = Substitute.For<IPermitProvider>();
        _permitRepository = Substitute.For<IPermitRepository>();
        _dateTimeOffsetProvider = Substitute.For<IDateTimeOffsetProvider>();
        
        _cancellationToken = CancellationToken.None;
        _permitManager = new PermitManager(_permitProvider, _configuration, _permitRepository, _dateTimeOffsetProvider);
    }

    [TestMethod, TestCategory(nameof(PermitManager.GetByApplicantAsync))]
    public async Task GetByApplicantAsync_GetPermitWithoutCacheShouldFetchFromProvider()
    {
        // Arrange
        var cacheUpdated = default(DateTimeOffset);
        var now = new DateTimeOffset(2023, 09, 23, 15, 00, 00, TimeSpan.Zero);
        _permitProvider.LastCacheUpdate.Returns(cacheUpdated);
        _dateTimeOffsetProvider.Now.Returns(now);

        const string applicantName = "Name1";
        const string? status = null;
        
        // Act
        await _permitManager.GetByApplicantAsync(applicantName, status, _cancellationToken);
        
        // Assert
        await _permitProvider.Received(1).FetchPermitAsync(_cancellationToken);
        await _permitRepository.Received(1).CommitAsync(_cancellationToken);
    }
    
    [TestMethod, TestCategory(nameof(PermitManager.GetByApplicantAsync))]
    public async Task GetByApplicantAsync_GetPermitWithCacheOutdatedShouldFetchFromProvider()
    {
        // Arrange
        var cacheUpdated = new DateTimeOffset(2023, 09, 23, 15, 00, 00, TimeSpan.Zero);
        var now = cacheUpdated.AddMinutes(_configuration.CacheDuration.Minutes + 1);
        _dateTimeOffsetProvider.Now.Returns(now);
        _permitProvider.LastCacheUpdate.Returns(cacheUpdated);
        const string applicantName = "Name1";
        const string? status = null;
        
        // Act
        await _permitManager.GetByApplicantAsync(applicantName, status, _cancellationToken);
        
        // Assert
        await _permitProvider.Received(1).FetchPermitAsync(_cancellationToken);
        await _permitRepository.Received(1).CommitAsync(_cancellationToken);
    }
    
    [TestMethod, TestCategory(nameof(PermitManager.GetByApplicantAsync))]
    public async Task GetByApplicantAsync_GetPermitWithCacheUpdatedShouldNotFetchFromProvider()
    {
        // Arrange
        var cacheUpdated = new DateTimeOffset(2023, 09, 23, 15, 00, 00, TimeSpan.Zero);
        var now = cacheUpdated.AddMinutes(_configuration.CacheDuration.Minutes - 1);
        _dateTimeOffsetProvider.Now.Returns(now);
        _permitProvider.LastCacheUpdate.Returns(cacheUpdated);
        const string applicantName = "Name1";
        const string? status = null;
        
        // Act
        await _permitManager.GetByApplicantAsync(applicantName, status, _cancellationToken);
        
        // Assert
        await _permitProvider.Received(0).FetchPermitAsync(_cancellationToken);
        await _permitRepository.Received(0).CommitAsync(_cancellationToken);
    }
    [TestMethod, TestCategory(nameof(PermitManager.GetByApplicantAsync))]
    public async Task GetByApplicantAsync_GetPermitWithoutStatusShouldSucceed()
    {
        // Arrange
        const string applicantName = "Name1";
        const string? status = null;
        
        // Act
        await _permitManager.GetByApplicantAsync(applicantName, status, _cancellationToken);
        
        // Assert
        _permitRepository.Received(1).GetAsync(Arg.Any<Expression<Func<Permit, bool>>>());
    }
    [TestMethod, TestCategory(nameof(PermitManager.GetByApplicantAsync))]
    public async Task GetByApplicantAsync_GetPermitWithStatusShouldSucceed()
    {
        // Arrange
        const string applicantName = "Name1";
        const string? status = "Approved";
        
        // Act
        await _permitManager.GetByApplicantAsync(applicantName, status, _cancellationToken);
        
        // Assert
        _permitRepository.Received(1).GetAsync(Arg.Any<Expression<Func<Permit, bool>>>());
    }
    
    [TestMethod, TestCategory(nameof(PermitManager.GetByAddressAsync))]
    public async Task GetByAddressAsync_GetPermitWithoutCacheShouldFetchFromProvider()
    {
        // Arrange
        var cacheUpdated = default(DateTimeOffset);
        var now = new DateTimeOffset(2023, 09, 23, 15, 00, 00, TimeSpan.Zero);
        _permitProvider.LastCacheUpdate.Returns(cacheUpdated);
        _dateTimeOffsetProvider.Now.Returns(now);

        const string address = "Address1";
        
        // Act
        await _permitManager.GetByAddressAsync(address, _cancellationToken);
        
        // Assert
        await _permitProvider.Received(1).FetchPermitAsync(_cancellationToken);
        await _permitRepository.Received(1).CommitAsync(_cancellationToken);
    }
    
    [TestMethod, TestCategory(nameof(PermitManager.GetByAddressAsync))]
    public async Task GetByAddressAsync_GetPermitWithCacheOutdatedShouldFetchFromProvider()
    {
        // Arrange
        var cacheUpdated = new DateTimeOffset(2023, 09, 23, 15, 00, 00, TimeSpan.Zero);
        var now = cacheUpdated.AddMinutes(_configuration.CacheDuration.Minutes + 1);
        _dateTimeOffsetProvider.Now.Returns(now);
        _permitProvider.LastCacheUpdate.Returns(cacheUpdated);
        const string address = "Address1";
        
        // Act
        await _permitManager.GetByAddressAsync(address, _cancellationToken);
        
        // Assert
        await _permitProvider.Received(1).FetchPermitAsync(_cancellationToken);
        await _permitRepository.Received(1).CommitAsync(_cancellationToken);
    }
    
    [TestMethod, TestCategory(nameof(PermitManager.GetByAddressAsync))]
    public async Task GetByAddressAsync_GetPermitWithCacheUpdatedShouldNotFetchFromProvider()
    {
        // Arrange
        var cacheUpdated = new DateTimeOffset(2023, 09, 23, 15, 00, 00, TimeSpan.Zero);
        var now = cacheUpdated.AddMinutes(_configuration.CacheDuration.Minutes - 1);
        _dateTimeOffsetProvider.Now.Returns(now);
        _permitProvider.LastCacheUpdate.Returns(cacheUpdated);
        const string address = "Address1";
        
        // Act
        await _permitManager.GetByAddressAsync(address, _cancellationToken);
        
        // Assert
        await _permitProvider.Received(0).FetchPermitAsync(_cancellationToken);
        await _permitRepository.Received(0).CommitAsync(_cancellationToken);
    }
    [TestMethod, TestCategory(nameof(PermitManager.GetByAddressAsync))]
    public async Task GetByAddressAsync_GetPermitShouldSucceed()
    {
        // Arrange
        const string address = "Address1";
        
        // Act
        await _permitManager.GetByAddressAsync(address, _cancellationToken);
        
        // Assert
        _permitRepository.Received(1).GetAsync(Arg.Any<Expression<Func<Permit, bool>>>());
    }
    
    // -----------------------
    [TestMethod, TestCategory(nameof(PermitManager.GetByNearestAsync))]
    public async Task GetByNearestAsync_GetPermitWithoutCacheShouldFetchFromProvider()
    {
        // Arrange
        var cacheUpdated = default(DateTimeOffset);
        var now = new DateTimeOffset(2023, 09, 23, 15, 00, 00, TimeSpan.Zero);
        _permitProvider.LastCacheUpdate.Returns(cacheUpdated);
        _dateTimeOffsetProvider.Now.Returns(now);

        const double latitude = 0.0;
        const double longitude = 0.0;
        const string status = "Approved";
        const int take = 5;
        
        // Act
        await _permitManager.GetByNearestAsync(latitude, longitude, status, take, _cancellationToken);
        
        // Assert
        await _permitProvider.Received(1).FetchPermitAsync(_cancellationToken);
        await _permitRepository.Received(1).CommitAsync(_cancellationToken);
    }
    
    [TestMethod, TestCategory(nameof(PermitManager.GetByNearestAsync))]
    public async Task GetByNearestAsync_GetPermitWithCacheOutdatedShouldFetchFromProvider()
    {
        // Arrange
        var cacheUpdated = new DateTimeOffset(2023, 09, 23, 15, 00, 00, TimeSpan.Zero);
        var now = cacheUpdated.AddMinutes(_configuration.CacheDuration.Minutes + 1);
        _dateTimeOffsetProvider.Now.Returns(now);
        _permitProvider.LastCacheUpdate.Returns(cacheUpdated);
        const double latitude = 0.0;
        const double longitude = 0.0;
        const string status = "Approved";
        const int take = 5;
        
        // Act
        await _permitManager.GetByNearestAsync(latitude, longitude, status, take, _cancellationToken);
        
        // Assert
        await _permitProvider.Received(1).FetchPermitAsync(_cancellationToken);
        await _permitRepository.Received(1).CommitAsync(_cancellationToken);
    }
    
    [TestMethod, TestCategory(nameof(PermitManager.GetByNearestAsync))]
    public async Task GetByNearestAsync_GetPermitWithCacheUpdatedShouldNotFetchFromProvider()
    {
        // Arrange
        var cacheUpdated = new DateTimeOffset(2023, 09, 23, 15, 00, 00, TimeSpan.Zero);
        var now = cacheUpdated.AddMinutes(_configuration.CacheDuration.Minutes - 1);
        _dateTimeOffsetProvider.Now.Returns(now);
        _permitProvider.LastCacheUpdate.Returns(cacheUpdated);
        const double latitude = 0.0;
        const double longitude = 0.0;
        const string status = "Approved";
        const int take = 5;
        
        // Act
        await _permitManager.GetByNearestAsync(latitude, longitude, status, take, _cancellationToken);
        
        // Assert
        await _permitProvider.Received(0).FetchPermitAsync(_cancellationToken);
        await _permitRepository.Received(0).CommitAsync(_cancellationToken);
    }
    [TestMethod, TestCategory(nameof(PermitManager.GetByNearestAsync))]
    public async Task GetByNearestAsync_GetPermitShouldSucceed()
    {
        // Arrange
        const double latitude = 0.0;
        const double longitude = 0.0;
        const string status = "Approved";
        const int take = 5;
        
        // Act
        await _permitManager.GetByNearestAsync(latitude, longitude, status, take, _cancellationToken);
        
        // Assert
        _permitRepository.Received(1).GetOrderedAsync(Arg.Any<Expression<Func<Permit, bool>>>(), Arg.Any<Expression<Func<Permit, double>>>(), take, _cancellationToken);
    }
}