using Microsoft.AspNetCore.Mvc;
using VehicleFinder.Api.Repositories;

namespace VehicleFinder.Api.Controllers;

[ApiController]
[Route("api/stats")]
public class StatsController : ControllerBase
{
    private readonly IVehicleRepository _repository;

    public StatsController(IVehicleRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetStats()
    {
        var totalRecords =
            await _repository.GetRecordCountAsync();

        var financeStats =
            await _repository.GetFinanceStatsAsync();

        var topSearches =
            await _repository.GetTopSearchesAsync();

        return Ok(new
        {
            totalRecords,
            financeStats,
            topSearches
        });
    }
}