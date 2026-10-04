using Microsoft.AspNetCore.Mvc;
using VehicleFinder.Api.Repositories;

namespace VehicleFinder.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly IVehicleRepository _repository;

    public HealthController(IVehicleRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetHealth()
    {
        var recordCount = await _repository.GetRecordCountAsync();

        return Ok(new
        {
            status = "ok",
            records = recordCount
        });
    }
}