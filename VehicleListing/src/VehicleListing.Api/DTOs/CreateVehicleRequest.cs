namespace VehicleListing.Api.DTOs;

public class CreateVehicleRequest
{
    public int DealerId { get; set; }
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public decimal Price { get; set; }
    public int Mileage { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Description { get; set; }
}
