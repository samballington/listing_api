using VehicleListing.Api.DTOs;
using VehicleListing.Api.Models;
using VehicleListing.Api.Repositories;

namespace VehicleListing.Api.Services;

public class VehicleService : IVehicleService
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IDealerRepository _dealerRepository;
    private readonly ILogger<VehicleService> _logger;

    public VehicleService(
        IVehicleRepository vehicleRepository,
        IDealerRepository dealerRepository,
        ILogger<VehicleService> logger)
    {
        _vehicleRepository = vehicleRepository;
        _dealerRepository = dealerRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<VehicleDto>> GetAllAsync(VehicleFilterParams filters)
    {
        var vehicles = await _vehicleRepository.GetAllAsync(filters);
        return vehicles.Select(MapToDto);
    }

    public async Task<VehicleDto?> GetByIdAsync(int id)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(id);
        if (vehicle is null)
        {
            _logger.LogWarning("Vehicle {VehicleId} not found", id);
            return null;
        }
        return MapToDto(vehicle);
    }

    public async Task<IEnumerable<VehicleDto>> SearchAsync(string query)
    {
        _logger.LogInformation("Searching vehicles with query {Query}", query);
        var vehicles = await _vehicleRepository.SearchAsync(query);
        return vehicles.Select(MapToDto);
    }

    public async Task<VehicleDto> CreateAsync(CreateVehicleRequest request)
    {
        var dealerExists = await _dealerRepository.ExistsAsync(request.DealerId);
        if (!dealerExists)
        {
            _logger.LogWarning("Create vehicle failed — Dealer {DealerId} does not exist", request.DealerId);
            throw new ArgumentException($"Dealer with ID {request.DealerId} does not exist.");
        }

        var vehicle = new Vehicle
        {
            DealerId = request.DealerId,
            Make = request.Make,
            Model = request.Model,
            Year = request.Year,
            Price = request.Price,
            Mileage = request.Mileage,
            Status = request.Status,
            Description = request.Description,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _vehicleRepository.AddAsync(vehicle);
        _logger.LogInformation("Vehicle {VehicleId} created", created.Id);
        return MapToDto(created);
    }

    public async Task<VehicleDto?> UpdateAsync(int id, UpdateVehicleRequest request)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(id);
        if (vehicle is null)
        {
            _logger.LogWarning("Update failed — Vehicle {VehicleId} not found", id);
            return null;
        }

        if (request.Price.HasValue)
            vehicle.Price = request.Price.Value;

        if (request.Mileage.HasValue)
            vehicle.Mileage = request.Mileage.Value;

        if (request.Status is not null)
            vehicle.Status = request.Status;

        if (request.Description is not null)
            vehicle.Description = request.Description;

        vehicle.UpdatedAt = DateTime.UtcNow;

        await _vehicleRepository.UpdateAsync(vehicle);
        _logger.LogInformation("Vehicle {VehicleId} updated", id);
        return MapToDto(vehicle);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(id);
        if (vehicle is null)
        {
            _logger.LogWarning("Delete failed — Vehicle {VehicleId} not found", id);
            return false;
        }

        await _vehicleRepository.DeleteAsync(id);
        _logger.LogInformation("Vehicle {VehicleId} deleted", id);
        return true;
    }

    private static VehicleDto MapToDto(Vehicle vehicle) => new()
    {
        Id = vehicle.Id,
        DealerId = vehicle.DealerId,
        DealerName = vehicle.Dealer?.Name ?? string.Empty,
        Make = vehicle.Make,
        Model = vehicle.Model,
        Year = vehicle.Year,
        Price = vehicle.Price,
        Mileage = vehicle.Mileage,
        Status = vehicle.Status,
        Description = vehicle.Description,
        CreatedAt = vehicle.CreatedAt,
        UpdatedAt = vehicle.UpdatedAt
    };
}
