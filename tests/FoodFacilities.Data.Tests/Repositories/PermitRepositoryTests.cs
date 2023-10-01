using System.Linq.Expressions;
using FoodFacilities.Data.Repositories;
using FoodFacilities.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodFacilities.Data.Tests.Repositories;

[TestClass]
public class PermitRepositoryTests
{
    private IPermitRepository _permitRepository;
    private FoodFacilitiesDbContext _dbContext;
    private CancellationToken _cancellationToken;

    [TestInitialize]
    public void InitializeTest()
    {
        var options = new DbContextOptionsBuilder<FoodFacilitiesDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _dbContext = new FoodFacilitiesDbContext(options);
        _permitRepository = new PermitRepository(_dbContext);
        _cancellationToken = CancellationToken.None;
    }

    [TestMethod, TestCategory(nameof(PermitRepository.GetAsync))]
    public void GetAsync_GetPermitWithExistingItemShouldSucceed()
    {
        // Arrange
        Expression<Func<Permit, bool>> filter = p => p.Applicant == "Person1";
        PopulateDb(5);
        
        // Act
        var permits = _permitRepository.GetAsync(filter);
        
        // Assert
        Assert.AreEqual(1, permits.ToBlockingEnumerable().Count());
    }
    
    [TestMethod, TestCategory(nameof(PermitRepository.GetAsync))]
    public void GetAsync_GetPermitWithNonExistingItemShouldFail()
    {
        // Arrange
        Expression<Func<Permit, bool>> filter = p => p.Applicant == "Person6";
        PopulateDb(5);
        
        // Act
        var permits = _permitRepository.GetAsync(filter);
        
        // Assert
        Assert.AreEqual(0, permits.ToBlockingEnumerable().Count());
    }
    
    [TestMethod, TestCategory(nameof(PermitRepository.GetAllAsync))]
    public void GetAllAsync_GetAllPermitsWithExistingItemsShouldSucceed()
    {
        // Arrange
        PopulateDb(5);
        
        // Act
        var permits = _permitRepository.GetAllAsync();
        
        // Assert
        Assert.AreEqual(5, permits.ToBlockingEnumerable().Count());
    }
    
    [TestMethod, TestCategory(nameof(PermitRepository.GetAllAsync))]
    public void GetAllAsync_GetAllPermitWithNonExistingItemShouldSucceed()
    {
        // Arrange
        
        // Act
        var permits = _permitRepository.GetAllAsync();
        
        // Assert
        Assert.AreEqual(0, permits.ToBlockingEnumerable().Count());
    }
    
    [TestMethod, TestCategory(nameof(PermitRepository.AddRangeAsync))]
    public async Task AddRangeAsync_AddPermitsShouldSucceed()
    {
        // Arrange
        var permits = CreatePermits(5);
        
        // Act
        await _permitRepository.AddRangeAsync(permits, _cancellationToken);
        await _dbContext.SaveChangesAsync(_cancellationToken);
        
        // Assert
        Assert.AreEqual(5, _dbContext.Permits.Count());
    }
    
    [TestMethod, TestCategory(nameof(PermitRepository.RemoveAllAsync))]
    public async Task RemoveAllAsync_RemoveAllPermitsShouldSucceed()
    {
        // Arrange
        PopulateDb(5);
        
        // Act
        await _permitRepository.RemoveAllAsync(_cancellationToken);
        await _dbContext.SaveChangesAsync(_cancellationToken);
        
        // Assert
        Assert.AreEqual(0, _dbContext.Permits.Count());
    }
    
    [TestMethod, TestCategory(nameof(PermitRepository.CommitAsync))]
    public async Task CommitAsync_CommitChangeShouldSaveToDb()
    {
        // Arrange
        var permits = CreatePermits(5).ToArray();
        _dbContext.Permits.AddRange(permits);
        await _dbContext.SaveChangesAsync(_cancellationToken);
        _dbContext.Remove(permits.First());
        
        // Act
        await _permitRepository.CommitAsync(_cancellationToken);
        
        // Assert
        Assert.AreEqual(4, _dbContext.Permits.Count());
    }
    
    [TestMethod, TestCategory(nameof(PermitRepository.GetOrderedAsync))]
    public void GetOrderedAsync_GetPermitOrderedWithExistingItemShouldSucceed()
    {
        // Arrange
        Expression<Func<Permit, bool>> filter = p => p.Applicant.Contains("Person");
        Expression<Func<Permit, double>> order = p => p.Latitude;
        PopulateDb(5);
        
        // Act
        var permits = _permitRepository.GetOrderedAsync(filter, order, 3, _cancellationToken);
        
        // Assert
        var permitsArray = permits.ToBlockingEnumerable().ToArray();
        Assert.AreEqual(3, permitsArray.Count());
        Assert.IsTrue(permitsArray[0].Latitude < permitsArray[1].Latitude);
    }

    private IEnumerable<Permit> CreatePermits(int amount)
    {
        for (var i = amount; i > 0; i--)
        {
            yield return new Permit()
            {
                Id = $"{i}",
                Applicant = $"Person{i}",
                Address = $"Address{i}",
                Latitude = i,
                Longitude = i,
                Status = "Approved",
            };
        }
    }

    private void PopulateDb(int amount)
    {
        _dbContext.Permits.AddRange(CreatePermits(amount));
        _dbContext.SaveChanges();
    }
}