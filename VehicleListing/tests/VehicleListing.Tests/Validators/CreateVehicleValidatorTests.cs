using VehicleListing.Api.DTOs;
using VehicleListing.Api.Validators;

namespace VehicleListing.Tests.Validators;

public class CreateVehicleValidatorTests
{
    private readonly CreateVehicleValidator _validator = new();

    [Fact]
    public void Validate_WithValidRequest_Passes()
    {
        // Arrange
        var request = new CreateVehicleRequest
        {
            DealerId = 1,
            Make = "Toyota",
            Model = "Camry",
            Year = 2022,
            Price = 25000m,
            Mileage = 0,
            Status = "Available",
            Description = "A great car"
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_WithNegativePrice_Fails()
    {
        // Arrange
        var request = new CreateVehicleRequest
        {
            DealerId = 1,
            Make = "Toyota",
            Model = "Camry",
            Year = 2022,
            Price = -1m,
            Mileage = 0,
            Status = "Available"
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Price");
    }
}
