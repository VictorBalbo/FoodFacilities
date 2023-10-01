using System.Reflection;
using IConfiguration = FoodFacilitiesAPI.Configurations.IConfiguration;

namespace FoodFacilitiesAPI;

/// <summary>
/// Responsible for registering dependencies for Dependency Injection pattern
/// </summary>
public static class Registrable
{
    public static void RegisterTo(IServiceCollection services)
    {
        var registrables = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t =>
                !t.IsAbstract &&
                typeof(IRegistrable).IsAssignableFrom(t))
            .Select(t => (IRegistrable)Activator.CreateInstance(t)!);

        foreach (var registrable in registrables)
        {
            registrable.RegisterTo(services);
        }

    }

    public static void RegisterApiDependencies(this IServiceCollection services, IConfiguration configurations)
    {
        services.AddSingleton(configurations);
        services.AddHttpClient();
        
        RegisterTo(services);
    }
}