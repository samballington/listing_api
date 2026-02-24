using Microsoft.AspNetCore.Mvc;
using VehicleListing.Api.DTOs;
using VehicleListing.Api.Services;

namespace VehicleListing.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DealersController : ControllerBase
{
    private readonly IDealerService _dealerService;

    public DealersController(IDealerService dealerService)
    {
        _dealerService = dealerService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var dealers = await _dealerService.GetAllAsync();
        return Ok(dealers);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var dealer = await _dealerService.GetByIdAsync(id);
        return dealer is null ? NotFound() : Ok(dealer);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateDealerRequest request)
    {
        var created = await _dealerService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }
}
