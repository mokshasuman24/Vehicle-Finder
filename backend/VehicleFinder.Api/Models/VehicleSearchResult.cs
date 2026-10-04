namespace VehicleFinder.Api.Models;

public class VehicleSearchResult
{
    public int Id { get; set; }

    public string VehicleNo { get; set; } = string.Empty;

    public string Model { get; set; } = string.Empty;

    public string Finance { get; set; } = string.Empty;

    public string Branch { get; set; } = string.Empty;
}