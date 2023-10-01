using System.Text.Json.Serialization;

namespace FoodFacilities.Models;

public class Permit
{
    [JsonPropertyName("objectid")]
    public string Id { get; set; }

    public string Applicant { get; set; }

    public string? FacilityType { get; set; }

    public string? Cnn { get; set; }

    public string? LocationDescription { get; set; }

    public string Address { get; set; }

    public string? BlockLot { get; set; }

    public string? Block { get; set; }

    public string? Lot { get; set; }

    [JsonPropertyName("permit")] 
    public string? PermitId { get; set; }

    public string Status { get; set; }

    public string? X { get; set; }

    public string? Y { get; set; }

    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public double Latitude { get; set; }

    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public double Longitude { get; set; }

    public string? Schedule { get; set; }

    public string? Received { get; set; }

    public string? PriorPermit { get; set; }

    public string? ExpirationDate { get; set; }
}