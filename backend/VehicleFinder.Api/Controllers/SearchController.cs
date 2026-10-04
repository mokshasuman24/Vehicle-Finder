using Microsoft.AspNetCore.Mvc;
using VehicleFinder.Api.Models;
using VehicleFinder.Api.Repositories;

namespace VehicleFinder.Api.Controllers;

[ApiController]
[Route("api/search")]
public class SearchController : ControllerBase
{
    private readonly IVehicleRepository _repository;

    public SearchController(IVehicleRepository repository)
    {
        _repository = repository;
    }

    [HttpGet("rc/{last4}")]
    public async Task<IActionResult> SearchVehicle(string last4)
    {
        if (string.IsNullOrWhiteSpace(last4) || last4.Length != 4)
        {
            return BadRequest(new
            {
                success = false,
                message = "Vehicle number search requires exactly 4 characters."
            });
        }

        var results =
            await _repository.SearchByVehicleNumberAsync(last4);

        await _repository.LogSearchAsync(
            last4,
            "rc",
            results.Count);

        var response = new SearchResponse
        {
            Success = true,
            Type = "rc",
            Query = last4,
            Count = results.Count,
            Results = results
        };

        return Ok(response);
    }


    [HttpGet("chassis/{last5}")]
public async Task<IActionResult> SearchChassis(string last5)
{
    last5 = last5.Trim();

    if (last5.Length != 5)
    {
        return BadRequest(new
        {
            success = false,
            message = "Chassis search requires exactly 5 characters."
        });
    }

    var results =
        await _repository.SearchByChassisAsync(last5);

    await _repository.LogSearchAsync(
        last5,
        "chassis",
        results.Count);

    var response = new SearchResponse
    {
        Success = true,
        Type = "chassis",
        Query = last5,
        Count = results.Count,
        Results = results
    };

    return Ok(response);
}
}