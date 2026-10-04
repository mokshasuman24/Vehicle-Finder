using Microsoft.AspNetCore.Mvc;
using VehicleFinder.Api.Repositories;

namespace VehicleFinder.Api.Controllers;

[ApiController]
[Route("api/records")]
public class RecordsController : ControllerBase
{
    private readonly IVehicleRepository _repository;

    public RecordsController(IVehicleRepository repository)
    {
        _repository = repository;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetRecord(int id)
    {
        var record =
            await _repository.GetRecordByIdAsync(id);

        if (record == null)
        {
            return NotFound(new
            {
                success = false,
                message = "Vehicle record not found."
            });
        }

        return Ok(record);
    }
}