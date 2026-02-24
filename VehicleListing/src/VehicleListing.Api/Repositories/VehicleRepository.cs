using Microsoft.EntityFrameworkCore;
using VehicleListing.Api.Data;
using VehicleListing.Api.DTOs;
using VehicleListing.Api.Models;

namespace VehicleListing.Api.Repositories;

public class VehicleRepository : IVehicleRepository
{
    private readonly AppDbContext _context;

    public VehicleRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Vehicle>> GetAllAsync(VehicleFilterParams filters)
    {
        IQueryable<Vehicle> query = _context.Vehicles.Include(v => v.Dealer);

        if (!string.IsNullOrWhiteSpace(filters.Make))
            query = query.Where(v => v.Make == filters.Make);

        if (!string.IsNullOrWhiteSpace(filters.Model))
            query = query.Where(v => v.Model == filters.Model);

        if (filters.Year.HasValue)
            query = query.Where(v => v.Year == filters.Year.Value);

        if (filters.MinPrice.HasValue)
            query = query.Where(v => v.Price >= filters.MinPrice.Value);

        if (filters.MaxPrice.HasValue)
            query = query.Where(v => v.Price <= filters.MaxPrice.Value);

        if (!string.IsNullOrWhiteSpace(filters.Status))
            query = query.Where(v => v.Status == filters.Status);

        return await query.ToListAsync();
    }

    public async Task<Vehicle?> GetByIdAsync(int id)
    {
        return await _context.Vehicles
            .Include(v => v.Dealer)
            .FirstOrDefaultAsync(v => v.Id == id);
    }

    public async Task<IEnumerable<Vehicle>> SearchAsync(string query)
    {
        return await _context.Vehicles
            .Include(v => v.Dealer)
            .Where(v => v.Make.Contains(query) ||
                        v.Model.Contains(query) ||
                        (v.Description != null && v.Description.Contains(query)))
            .ToListAsync();
    }

    public async Task<Vehicle> AddAsync(Vehicle vehicle)
    {
        _context.Vehicles.Add(vehicle);
        await _context.SaveChangesAsync();
        return vehicle;
    }

    public async Task UpdateAsync(Vehicle vehicle)
    {
        _context.Vehicles.Update(vehicle);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var vehicle = await _context.Vehicles.FindAsync(id);
        if (vehicle is not null)
        {
            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();
        }
    }
}
