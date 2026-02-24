using VehicleListing.Api.DTOs;
using VehicleListing.Api.Models;

namespace VehicleListing.Api.Repositories;

public interface IVehicleRepository
{
    Task<IEnumerable<Vehicle>> GetAllAsync(VehicleFilterParams filters);
    Task<Vehicle?> GetByIdAsync(int id);
    Task<IEnumerable<Vehicle>> SearchAsync(string query);
    Task<Vehicle> AddAsync(Vehicle vehicle);
    Task UpdateAsync(Vehicle vehicle);
    Task DeleteAsync(int id);
}
