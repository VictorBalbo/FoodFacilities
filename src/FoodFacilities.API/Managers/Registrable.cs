namespace FoodFacilitiesAPI.Managers;

public class Registrable: IRegistrable
{
    public void RegisterTo(IServiceCollection services)
    {
        services.AddScoped<IPermitManager, PermitManager>();
    }
}