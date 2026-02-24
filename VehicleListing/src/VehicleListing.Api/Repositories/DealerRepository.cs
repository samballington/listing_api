using Microsoft.EntityFrameworkCore;
using VehicleListing.Api.Data;
using VehicleListing.Api.Models;

namespace VehicleListing.Api.Repositories;

public class DealerRepository : IDealerRepository
{
    private readonly AppDbContext _context;

    public DealerRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Dealer>> GetAllAsync()
    {
        return await _context.Dealers.ToListAsync();
    }

    public async Task<Dealer?> GetByIdAsync(int id)
    {
        return await _context.Dealers
            .Include(d => d.Vehicles)
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task<Dealer> AddAsync(Dealer dealer)
    {
        _context.Dealers.Add(dealer);
        await _context.SaveChangesAsync();
        return dealer;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Dealers.AnyAsync(d => d.Id == id);
    }
}
