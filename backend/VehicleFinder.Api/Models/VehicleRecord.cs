namespace VehicleFinder.Api.Models;

public class VehicleRecord
{
    public int Id { get; set; }

    public string VehicleNo { get; set; } = string.Empty;

    public string ChassisNo { get; set; } = string.Empty;

    public string EngineNo { get; set; } = string.Empty;

    public string Model { get; set; } = string.Empty;

    public string CustomerName { get; set; } = string.Empty;

    public string CustomerMobile { get; set; } = string.Empty;

    public decimal OverdueAmount { get; set; }

    public string Finance { get; set; } = string.Empty;

    public string Branch { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}