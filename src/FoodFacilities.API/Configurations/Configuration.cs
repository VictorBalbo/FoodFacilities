namespace FoodFacilitiesAPI.Configurations;

public class Configuration : IConfiguration
{
    public string DatabaseName { get; set; }
    public TimeSpan CacheDuration { get; set; }
}