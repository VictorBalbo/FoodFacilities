namespace FoodFacilitiesAPI.Providers;

public class Registrable : IRegistrable
{
    public void RegisterTo(IServiceCollection services)
    {
        services
            .AddSingleton<IPermitProvider, PermitProvider>()
            .AddSingleton<IDateTimeOffsetProvider, DateTimeOffsetProvider>();
    }
}