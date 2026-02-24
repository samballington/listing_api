using VehicleListing.Api.DTOs;

namespace VehicleListing.Api.Services;

public interface IDealerService
{
    Task<IEnumerable<DealerDto>> GetAllAsync();
    Task<DealerDto?> GetByIdAsync(int id);
    Task<DealerDto> CreateAsync(CreateDealerRequest request);
}
