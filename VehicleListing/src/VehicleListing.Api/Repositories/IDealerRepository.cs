using VehicleListing.Api.Models;

namespace VehicleListing.Api.Repositories;

public interface IDealerRepository
{
    Task<IEnumerable<Dealer>> GetAllAsync();
    Task<Dealer?> GetByIdAsync(int id);
    Task<Dealer> AddAsync(Dealer dealer);
    Task<bool> ExistsAsync(int id);
}
