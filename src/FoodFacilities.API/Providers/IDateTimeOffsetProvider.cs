namespace FoodFacilitiesAPI.Providers;

public interface IDateTimeOffsetProvider
{
    public DateTimeOffset Now { get; }
}