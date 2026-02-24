namespace VehicleListing.Api.DTOs;

public class DealerDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public List<VehicleDto>? Vehicles { get; set; }
}
