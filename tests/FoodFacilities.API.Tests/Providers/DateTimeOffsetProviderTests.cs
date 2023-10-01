using FoodFacilitiesAPI.Providers;

namespace FoodFacilities.API.Tests.Providers;

[TestClass]
public class DateTimeOffsetProviderTests
{
    private DateTimeOffsetProvider _dateTimeOffsetProvider;

    [TestInitialize]
    public void InitializeTest()
    {
        _dateTimeOffsetProvider = new DateTimeOffsetProvider();
    }

    [TestMethod, TestCategory(nameof(DateTimeOffsetProvider.Now))]
    public void Now_NowShouldReturnValidDateTimeOffset()
    {
        // Arrange
        // Act
        var now = _dateTimeOffsetProvider.Now;
        // Assert
        Assert.AreNotEqual(default, now);
    }
}