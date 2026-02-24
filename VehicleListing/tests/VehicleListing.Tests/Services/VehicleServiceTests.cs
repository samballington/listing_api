using Microsoft.Extensions.Logging;
using Moq;
using VehicleListing.Api.DTOs;
using VehicleListing.Api.Models;
using VehicleListing.Api.Repositories;
using VehicleListing.Api.Services;

namespace VehicleListing.Tests.Services;

public class VehicleServiceTests
{
    private readonly Mock<IVehicleRepository> _vehicleRepoMock;
    private readonly Mock<IDealerRepository> _dealerRepoMock;
    private readonly Mock<ILogger<VehicleService>> _loggerMock;
    private readonly VehicleService _sut;

    public VehicleServiceTests()
    {
        _vehicleRepoMock = new Mock<IVehicleRepository>();
        _dealerRepoMock = new Mock<IDealerRepository>();
        _loggerMock = new Mock<ILogger<VehicleService>>();
        _sut = new VehicleService(_vehicleRepoMock.Object, _dealerRepoMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsVehicleDtos()
    {
        // Arrange
        var dealer = new Dealer { Id = 1, Name = "Auto World", Location = "TX" };
        var vehicles = new List<Vehicle>
        {
            new() { Id = 1, Make = "Toyota", Model = "Camry", Year = 2022, Price = 25000, Mileage = 0, Status = "Available", DealerId = 1, Dealer = dealer, CreatedAt = DateTime.UtcNow }
        };
        _vehicleRepoMock.Setup(r => r.GetAllAsync(It.IsAny<VehicleFilterParams>()))
                        .ReturnsAsync(vehicles);

        // Act
        var result = await _sut.GetAllAsync(new VehicleFilterParams());

        // Assert
        var dtos = result.ToList();
        Assert.Single(dtos);
        Assert.Equal("Toyota", dtos[0].Make);
        Assert.Equal("Auto World", dtos[0].DealerName);
    }

    [Fact]
    public async Task GetByIdAsync_WhenVehicleExists_ReturnsDto()
    {
        // Arrange
        var dealer = new Dealer { Id = 1, Name = "City Cars", Location = "CA" };
        var vehicle = new Vehicle { Id = 5, Make = "Honda", Model = "Civic", Year = 2021, Price = 22000, Mileage = 5000, Status = "Available", DealerId = 1, Dealer = dealer, CreatedAt = DateTime.UtcNow };
        _vehicleRepoMock.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(vehicle);

        // Act
        var result = await _sut.GetByIdAsync(5);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(5, result.Id);
        Assert.Equal("Honda", result.Make);
        Assert.Equal("City Cars", result.DealerName);
    }

    [Fact]
    public async Task GetByIdAsync_WhenVehicleDoesNotExist_ReturnsNull()
    {
        // Arrange
        _vehicleRepoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Vehicle?)null);

        // Act
        var result = await _sut.GetByIdAsync(99);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_WhenDealerExists_CreatesAndReturnsDto()
    {
        // Arrange
        var request = new CreateVehicleRequest
        {
            DealerId = 1,
            Make = "Ford",
            Model = "F-150",
            Year = 2023,
            Price = 45000,
            Mileage = 0,
            Status = "Available"
        };
        var dealer = new Dealer { Id = 1, Name = "Truck Depot", Location = "TX" };
        var savedVehicle = new Vehicle
        {
            Id = 10,
            DealerId = 1,
            Make = "Ford",
            Model = "F-150",
            Year = 2023,
            Price = 45000,
            Mileage = 0,
            Status = "Available",
            Dealer = dealer,
            CreatedAt = DateTime.UtcNow
        };

        _dealerRepoMock.Setup(r => r.ExistsAsync(1)).ReturnsAsync(true);
        _vehicleRepoMock.Setup(r => r.AddAsync(It.IsAny<Vehicle>())).ReturnsAsync(savedVehicle);

        // Act
        var result = await _sut.CreateAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(10, result.Id);
        Assert.Equal("Ford", result.Make);
        _vehicleRepoMock.Verify(r => r.AddAsync(It.IsAny<Vehicle>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WhenDealerDoesNotExist_ThrowsArgumentException()
    {
        // Arrange
        var request = new CreateVehicleRequest { DealerId = 99, Make = "BMW", Model = "X5", Year = 2023, Price = 60000, Mileage = 0, Status = "Available" };
        _dealerRepoMock.Setup(r => r.ExistsAsync(99)).ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _sut.CreateAsync(request));
    }

    [Fact]
    public async Task DeleteAsync_WhenVehicleExists_ReturnsTrue()
    {
        // Arrange
        var dealer = new Dealer { Id = 1, Name = "Dealer", Location = "NY" };
        var vehicle = new Vehicle { Id = 7, Make = "Chevy", Model = "Silverado", Year = 2020, Price = 35000, Mileage = 10000, Status = "Available", DealerId = 1, Dealer = dealer, CreatedAt = DateTime.UtcNow };
        _vehicleRepoMock.Setup(r => r.GetByIdAsync(7)).ReturnsAsync(vehicle);
        _vehicleRepoMock.Setup(r => r.DeleteAsync(7)).Returns(Task.CompletedTask);

        // Act
        var result = await _sut.DeleteAsync(7);

        // Assert
        Assert.True(result);
        _vehicleRepoMock.Verify(r => r.DeleteAsync(7), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WhenVehicleDoesNotExist_ReturnsFalse()
    {
        // Arrange
        _vehicleRepoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Vehicle?)null);

        // Act
        var result = await _sut.DeleteAsync(99);

        // Assert
        Assert.False(result);
        _vehicleRepoMock.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
    }
}
