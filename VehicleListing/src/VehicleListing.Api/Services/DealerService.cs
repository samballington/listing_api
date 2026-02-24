using VehicleListing.Api.DTOs;
using VehicleListing.Api.Models;
using VehicleListing.Api.Repositories;

namespace VehicleListing.Api.Services;

public class DealerService : IDealerService
{
    private readonly IDealerRepository _dealerRepository;
    private readonly ILogger<DealerService> _logger;

    public DealerService(IDealerRepository dealerRepository, ILogger<DealerService> logger)
    {
        _dealerRepository = dealerRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<DealerDto>> GetAllAsync()
    {
        var dealers = await _dealerRepository.GetAllAsync();
        return dealers.Select(MapToDto);
    }

    public async Task<DealerDto?> GetByIdAsync(int id)
    {
        var dealer = await _dealerRepository.GetByIdAsync(id);
        if (dealer is null)
        {
            _logger.LogWarning("Dealer {DealerId} not found", id);
            return null;
        }
        return MapToDtoWithVehicles(dealer);
    }

    public async Task<DealerDto> CreateAsync(CreateDealerRequest request)
    {
        var dealer = new Dealer
        {
            Name = request.Name,
            Location = request.Location,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _dealerRepository.AddAsync(dealer);
        _logger.LogInformation("Dealer {DealerId} created", created.Id);
        return MapToDto(created);
    }

    private static DealerDto MapToDto(Dealer dealer) => new()
    {
        Id = dealer.Id,
        Name = dealer.Name,
        Location = dealer.Location,
        CreatedAt = dealer.CreatedAt
    };

    private static DealerDto MapToDtoWithVehicles(Dealer dealer) => new()
    {
        Id = dealer.Id,
        Name = dealer.Name,
        Location = dealer.Location,
        CreatedAt = dealer.CreatedAt,
        Vehicles = dealer.Vehicles.Select(v => new VehicleDto
        {
            Id = v.Id,
            DealerId = v.DealerId,
            DealerName = dealer.Name,
            Make = v.Make,
            Model = v.Model,
            Year = v.Year,
            Price = v.Price,
            Mileage = v.Mileage,
            Status = v.Status,
            Description = v.Description,
            CreatedAt = v.CreatedAt,
            UpdatedAt = v.UpdatedAt
        }).ToList()
    };
}
