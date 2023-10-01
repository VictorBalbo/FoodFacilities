namespace FoodFacilitiesAPI.Providers;

public class DateTimeOffsetProvider : IDateTimeOffsetProvider
{
    public DateTimeOffset Now { get; } = DateTimeOffset.Now;
}