namespace VehicleFinder.Api.Models;

public class SearchResponse
{
    public bool Success { get; set; }

    public string Type { get; set; } = string.Empty;

    public string Query { get; set; } = string.Empty;

    public int Count { get; set; }

    public List<VehicleSearchResult> Results { get; set; } = new();
}