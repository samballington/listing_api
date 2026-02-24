using VehicleListing.Api.DTOs;

namespace VehicleListing.Api.Services;

public interface IVehicleService
{
    Task<IEnumerable<VehicleDto>> GetAllAsync(VehicleFilterParams filters);
    Task<VehicleDto?> GetByIdAsync(int id);
    Task<IEnumerable<VehicleDto>> SearchAsync(string query);
    Task<VehicleDto> CreateAsync(CreateVehicleRequest request);
    Task<VehicleDto?> UpdateAsync(int id, UpdateVehicleRequest request);
    Task<bool> DeleteAsync(int id);
}
