using VehicleFinder.Api.Models;

namespace VehicleFinder.Api.Repositories;

public interface IVehicleRepository
{
    Task<int> GetRecordCountAsync();

    Task<List<VehicleSearchResult>> SearchByVehicleNumberAsync(string last4);

    Task<List<VehicleSearchResult>> SearchByChassisAsync(string last5);

    Task<VehicleRecord?> GetRecordByIdAsync(int id);

    Task LogSearchAsync(
        string query,
        string searchType,
        int resultCount);

    Task<List<object>> GetFinanceStatsAsync();

    Task<List<object>> GetTopSearchesAsync();
}