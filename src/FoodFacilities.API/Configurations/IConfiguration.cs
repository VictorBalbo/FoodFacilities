namespace FoodFacilitiesAPI.Configurations;

/// <summary>
/// Configuration interface that maps appsettings
/// </summary>
public interface IConfiguration
{
    /// <summary>
    /// Name of the database to be used
    /// </summary>
    public string DatabaseName { get; }
    
    /// <summary>
    /// Duration the registers on cache can be used
    /// </summary>
    public TimeSpan CacheDuration { get; }
}