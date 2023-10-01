using FoodFacilities.Data.Repositories;
using FoodFacilities.Models;
using FoodFacilitiesAPI.Controllers;
using FoodFacilitiesAPI.Managers;
using FoodFacilitiesAPI.Providers;
using NSubstitute;

namespace FoodFacilities.API.Tests.Controllers;

[TestClass]
public class PermitControllerTests
{
    private IPermitManager _permitManager;
    private CancellationToken _cancellationToken;
    private PermitController _permitController;

    [TestInitialize]
    public void InitializeTest()
    {
        _cancellationToken = CancellationToken.None;
        _permitManager = Substitute.For<IPermitManager>();
        _permitController = new PermitController(_permitManager);
    }

    [TestMethod, TestCategory(nameof(PermitController.GetPermitByApplicantName))]
    public async Task GetPermitByApplicantName_GetPermitWithValidNameShouldSucceed()
    {
        // Arrange
        var applicantName = "Name1";
        string? status = null;

        // Act
        await _permitController.GetPermitByApplicantName(applicantName, status, _cancellationToken);

        // Assert
        await _permitManager.Received(1).GetByApplicantAsync(applicantName, status, _cancellationToken);
    }
    
    [TestMethod, TestCategory(nameof(PermitController.GetPermitByApplicantName))]
    public async Task GetPermitByApplicantName_GetPermitWithValidNameAndStatusShouldSucceed()
    {
        // Arrange
        var applicantName = "Name1";
        string? status = "Approved";

        // Act
        await _permitController.GetPermitByApplicantName(applicantName, status, _cancellationToken);

        // Assert
        await _permitManager.Received(1).GetByApplicantAsync(applicantName, status, _cancellationToken);
    }

    [TestMethod, TestCategory(nameof(PermitController.GetPermitByApplicantName))]
    public async Task GetPermitByApplicantName_GetPermitWithInvalidNameShouldThrowException()
    {
        // Arrange
        var applicantName = "";
        string? status = null;

        // Act & Assert
        await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            await _permitController.GetPermitByApplicantName(applicantName, status, _cancellationToken));
    }
    
    [TestMethod, TestCategory(nameof(PermitController.GetPermitByAddress))]
    public async Task GetPermitByAddress_GetPermitWithValidAddressShouldSucceed()
    {
        // Arrange
        var address = "Address1";

        // Act
        await _permitController.GetPermitByAddress(address, _cancellationToken);

        // Assert
        await _permitManager.Received(1).GetByAddressAsync(address, _cancellationToken);
    }

    [TestMethod, TestCategory(nameof(PermitController.GetPermitByAddress))]
    public async Task GetPermitByAddress_GetPermitWithInvalidAddressShouldThrowException()
    {
        // Arrange
        string? address = "";

        // Act & Assert
        await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            await _permitController.GetPermitByAddress(address, _cancellationToken));
    }
    
    [TestMethod, TestCategory(nameof(PermitController.GetPermitByNearestName))]
    public async Task GetPermitByNearestName_GetPermitWithValidCoordinatesAndStatusAndTakeShouldSucceed()
    {
        // Arrange
        const double latitude = 0.0;
        const double longitude = 0.0;
        const string status = "Approved1";
        const int take = 2;

        // Act
        await _permitController.GetPermitByNearestName(latitude, longitude, _cancellationToken, status, take);

        // Assert
        await _permitManager.Received(1).GetByNearestAsync(latitude, longitude, status, take, _cancellationToken);
    }

    [TestMethod, TestCategory(nameof(PermitController.GetPermitByNearestName))]
    public async Task GetPermitByNearestName_GetPermitWithInvalidLatitudeShouldThrowException()
    {
        // Arrange
        const double latitude = 100.0;
        const double longitude = 0.0;
        const string status = "Approved1";
        const int take = 2;

        // Act & Assert
        await Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(async () =>
            await _permitController.GetPermitByNearestName(latitude, longitude, _cancellationToken, status, take));
    }
    
    [TestMethod, TestCategory(nameof(PermitController.GetPermitByNearestName))]
    public async Task GetPermitByNearestName_GetPermitWithInvalidLongitudeShouldThrowException()
    {
        // Arrange
        const double latitude = 0.0;
        const double longitude = 190.0;
        const string status = "Approved1";
        const int take = 2;

        // Act & Assert
        await Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(async () =>
            await _permitController.GetPermitByNearestName(latitude, longitude, _cancellationToken, status, take));
    }
    [TestMethod, TestCategory(nameof(PermitController.GetPermitByNearestName))]
    public async Task GetPermitByNearestName_GetPermitWithInvalidStatusShouldThrowException()
    {
        // Arrange
        const double latitude = 0.0;
        const double longitude = 0.0;
        const string status = "";
        const int take = 2;

        // Act & Assert
        await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            await _permitController.GetPermitByNearestName(latitude, longitude, _cancellationToken, status, take));
    }
    [TestMethod, TestCategory(nameof(PermitController.GetPermitByNearestName))]
    public async Task GetPermitByNearestName_GetPermitWithInvalidTakeShouldThrowException()
    {
        // Arrange
        const double latitude = 0.0;
        const double longitude = 0.0;
        const string status = "Approved1";
        const int take = -2;

        // Act & Assert
        await Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(async () =>
            await _permitController.GetPermitByNearestName(latitude, longitude, _cancellationToken, status, take));
    }
}