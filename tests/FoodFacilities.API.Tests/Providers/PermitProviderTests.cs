using System.Net;
using FoodFacilitiesAPI.Providers;
using NSubstitute;

namespace FoodFacilities.API.Tests.Providers;

[TestClass]
public class PermitProviderTests
{
    private IPermitProvider _permitProvider;
    private IDateTimeOffsetProvider _dateTimeOffsetProvider;
    private HttpClient _httpClient;
    private CancellationToken _cancellationToken;

    [TestInitialize]
    public void InitializeTest()
    {
        
        var httpClientFactory = Substitute.For<IHttpClientFactory>();
        _httpClient = Substitute.For<HttpClient>();
        httpClientFactory.CreateClient().Returns(_httpClient);
        
        _dateTimeOffsetProvider = Substitute.For<IDateTimeOffsetProvider>();
        
        _cancellationToken = CancellationToken.None;
        _permitProvider = new PermitProvider(httpClientFactory, _dateTimeOffsetProvider);
    }
    
    [TestMethod, TestCategory(nameof(PermitProvider.FetchPermitAsync))]
    public async Task FetchPermitAsync_FetchPermitShouldSendHttpRequest()
    {
        // Arrange
        var httpRequestMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent("[]"),
        };
        _httpClient.SendAsync(Arg.Any<HttpRequestMessage>(), _cancellationToken).Returns(httpRequestMessage);
        var now = new DateTimeOffset(2023, 09, 23, 15, 00, 00, TimeSpan.Zero);
        _dateTimeOffsetProvider.Now.Returns(now);
        
        // Act
        await _permitProvider.FetchPermitAsync(_cancellationToken);
        
        // Assert
        await _httpClient.Received(1).SendAsync(Arg.Is<HttpRequestMessage>(h => h.Method == HttpMethod.Get), _cancellationToken);
    }
    
    [TestMethod, TestCategory(nameof(PermitProvider.FetchPermitAsync))]
    public async Task FetchPermitAsync_FetchPermitShouldDeserializeResponseToPermitList()
    {
        // Arrange
        var response =
            "[{\"objectid\":\"1\",\"applicant\":\"Leo's Hot Dogs\",\"facilitytype\":\"Push Cart\",\"cnn\":\"9\",\"locationdescription\":\"Street\",\"address\":\"Street\",\"blocklot\":\"3\",\"block\":\"3\",\"lot\":\"0\",\"permit\":\"2\",\"status\":\"APPROVED\",\"fooditems\":\"Hot dogs\",\"x\":\"6007018.02\",\"y\":\"2104913.057\",\"latitude\":\"37.76008693198698\",\"longitude\":\"-122.41880648110114\",\"schedule\":\"\",\"approved\":\"2023-09-20T00:00:00.000\",\"received\":\"2\",\"priorpermit\":\"1\",\"expirationdate\":\"2024-11-15T00:00:00.000\"}]";
        var httpRequestMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(response),
        };
        _httpClient.SendAsync(Arg.Any<HttpRequestMessage>(), _cancellationToken).Returns(httpRequestMessage);
        var now = new DateTimeOffset(2023, 09, 23, 15, 00, 00, TimeSpan.Zero);
        _dateTimeOffsetProvider.Now.Returns(now);
        
        // Act
        var permits = await _permitProvider.FetchPermitAsync(_cancellationToken);
        
        // Assert
        Assert.IsNotNull(permits);
        var permitsArray = permits.ToArray();
        Assert.AreEqual(permitsArray.Count(), 1);
        Assert.AreEqual(permitsArray.First().Id, "1");
    }
    
    [TestMethod, TestCategory(nameof(PermitProvider.FetchPermitAsync))]
    public async Task FetchPermitAsync_FetchPermitShouldSetLastCacheUpdate()
    {
        // Arrange
        var httpRequestMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent("[]"),
        };
        _httpClient.SendAsync(Arg.Any<HttpRequestMessage>(), _cancellationToken).Returns(httpRequestMessage);
        var now = new DateTimeOffset(2023, 09, 23, 15, 00, 00, TimeSpan.Zero);
        _dateTimeOffsetProvider.Now.Returns(now);
        
        // Act
        await _permitProvider.FetchPermitAsync(_cancellationToken);
        
        // Assert
        Assert.AreEqual(_permitProvider.LastCacheUpdate, now); 
    }
}