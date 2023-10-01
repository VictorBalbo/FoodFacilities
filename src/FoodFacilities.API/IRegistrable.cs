namespace FoodFacilitiesAPI;

/// <summary>
/// Responsible for registering dependencies for Dependency Injection pattern
/// </summary>
public interface IRegistrable
{
    /// <summary>
    /// Register dependencies from a context to a Service Collection
    /// </summary>
    /// <param name="services"></param>
    public void RegisterTo(IServiceCollection services);
}