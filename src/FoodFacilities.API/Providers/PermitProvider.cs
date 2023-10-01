using System.Text.Json;
using FoodFacilities.Models;

namespace FoodFacilitiesAPI.Providers;

public class PermitProvider : IPermitProvider
{
    private readonly HttpClient _httpClient;
    private readonly IDateTimeOffsetProvider _dateTimeOffsetProvider; 

    public PermitProvider(IHttpClientFactory httpClientFactory, IDateTimeOffsetProvider dateTimeOffsetProvider)
    {
        _dateTimeOffsetProvider = dateTimeOffsetProvider;
        _httpClient =  httpClientFactory.CreateClient();
    }

    public DateTimeOffset LastCacheUpdate { get; private set; }
    
    public async Task<IEnumerable<Permit>> FetchPermitAsync(CancellationToken cancellationToken)
    {
        using var requestMessage = new HttpRequestMessage(HttpMethod.Get, "https://data.sfgov.org/resource/rqzj-sfat.json");
        var responseMessage = await _httpClient.SendAsync(requestMessage, cancellationToken);
        
        responseMessage.EnsureSuccessStatusCode();
        
        LastCacheUpdate = _dateTimeOffsetProvider.Now;
        
        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        
        var permits = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<Permit>>(jsonOptions, cancellationToken) ?? Enumerable.Empty<Permit>();
        return permits;
    }
}