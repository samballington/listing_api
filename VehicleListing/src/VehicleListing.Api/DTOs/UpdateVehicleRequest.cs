namespace VehicleListing.Api.DTOs;

public class UpdateVehicleRequest
{
    public decimal? Price { get; set; }
    public int? Mileage { get; set; }
    public string? Status { get; set; }
    public string? Description { get; set; }
}
