using MySqlConnector;
using VehicleFinder.Api.Models;

namespace VehicleFinder.Api.Repositories;

public class VehicleRepository : IVehicleRepository
{
    private readonly string _connectionString;

    public VehicleRepository(IConfiguration configuration)
    {
        _connectionString =
            configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Database connection string is missing.");
    }

    public async Task<int> GetRecordCountAsync()
    {
        await using var connection =
            new MySqlConnection(_connectionString);

        await connection.OpenAsync();

        const string sql = """
            SELECT COUNT(*)
            FROM vehicle_records;
            """;

        await using var command =
            new MySqlCommand(sql, connection);

        var result = await command.ExecuteScalarAsync();

        return Convert.ToInt32(result);
    }


    public async Task<List<VehicleSearchResult>>
        SearchByVehicleNumberAsync(string last4)
    {
        const string sql = """
            SELECT
                vr.id,
                vr.vehicle_no,
                vr.model,
                f.name AS finance,
                b.name AS branch
            FROM vehicle_records vr
            INNER JOIN branches b
                ON vr.branch_id = b.id
            INNER JOIN finances f
                ON b.finance_id = f.id
            WHERE vr.vehicle_last4 = @q
            ORDER BY vr.id;
            """;

        return await SearchAsync(sql, last4);
    }


    public async Task<List<VehicleSearchResult>>
        SearchByChassisAsync(string last5)
    {
        const string sql = """
            SELECT
                vr.id,
                vr.vehicle_no,
                vr.model,
                f.name AS finance,
                b.name AS branch
            FROM vehicle_records vr
            INNER JOIN branches b
                ON vr.branch_id = b.id
            INNER JOIN finances f
                ON b.finance_id = f.id
            WHERE vr.chassis_last5 = @q
            ORDER BY vr.id;
            """;

        return await SearchAsync(sql, last5);
    }


    private async Task<List<VehicleSearchResult>>
        SearchAsync(string sql, string query)
    {
        var results = new List<VehicleSearchResult>();

        await using var connection =
            new MySqlConnection(_connectionString);

        await connection.OpenAsync();

        await using var command =
            new MySqlCommand(sql, connection);

        command.Parameters.AddWithValue("@q", query);

        await using var reader =
            await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            results.Add(new VehicleSearchResult
            {
                Id = reader.GetInt32("id"),
                VehicleNo = reader.GetString("vehicle_no"),
                Model = reader.GetString("model"),
                Finance = reader.GetString("finance"),
                Branch = reader.GetString("branch")
            });
        }

        return results;
    }


    public async Task<VehicleRecord?> GetRecordByIdAsync(int id)
    {
        const string sql = """
            SELECT
                vr.id,
                vr.vehicle_no,
                vr.chassis_no,
                vr.engine_no,
                vr.model,
                vr.customer_name,
                vr.customer_mobile,
                vr.overdue_amount,
                vr.created_at,
                f.name AS finance,
                b.name AS branch,
                b.city
            FROM vehicle_records vr
            INNER JOIN branches b
                ON vr.branch_id = b.id
            INNER JOIN finances f
                ON b.finance_id = f.id
            WHERE vr.id = @id;
            """;

        await using var connection =
            new MySqlConnection(_connectionString);

        await connection.OpenAsync();

        await using var command =
            new MySqlCommand(sql, connection);

        command.Parameters.AddWithValue("@id", id);

        await using var reader =
            await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
        {
            return null;
        }

        return new VehicleRecord
        {
            Id = reader.GetInt32("id"),
            VehicleNo = reader.GetString("vehicle_no"),
            ChassisNo = reader.GetString("chassis_no"),
            EngineNo = reader.GetString("engine_no"),
            Model = reader.GetString("model"),
            CustomerName = reader.GetString("customer_name"),
            CustomerMobile = reader.GetString("customer_mobile"),
            OverdueAmount = reader.GetDecimal("overdue_amount"),
            Finance = reader.GetString("finance"),
            Branch = reader.GetString("branch"),
            City = reader.GetString("city"),
            CreatedAt = reader.GetDateTime("created_at")
        };
    }


    public async Task LogSearchAsync(
        string query,
        string searchType,
        int resultCount)
    {
        const string sql = """
            INSERT INTO search_logs
            (
                query,
                search_type,
                result_count
            )
            VALUES
            (
                @query,
                @searchType,
                @resultCount
            );
            """;

        await using var connection =
            new MySqlConnection(_connectionString);

        await connection.OpenAsync();

        await using var command =
            new MySqlCommand(sql, connection);

        command.Parameters.AddWithValue("@query", query);
        command.Parameters.AddWithValue("@searchType", searchType);
        command.Parameters.AddWithValue("@resultCount", resultCount);

        await command.ExecuteNonQueryAsync();
    }

    public async Task<List<object>> GetFinanceStatsAsync()
{
    const string sql = """
        SELECT
            f.name AS finance,
            COUNT(vr.id) AS record_count
        FROM finances f
        LEFT JOIN branches b
            ON f.id = b.finance_id
        LEFT JOIN vehicle_records vr
            ON b.id = vr.branch_id
        GROUP BY f.id, f.name
        ORDER BY record_count DESC;
        """;

    var results = new List<object>();

    await using var connection =
        new MySqlConnection(_connectionString);

    await connection.OpenAsync();

    await using var command =
        new MySqlCommand(sql, connection);

    await using var reader =
        await command.ExecuteReaderAsync();

    while (await reader.ReadAsync())
    {
        results.Add(new
        {
            finance = reader.GetString("finance"),
            recordCount = reader.GetInt64("record_count")
        });
    }

    return results;
}


public async Task<List<object>> GetTopSearchesAsync()
{
    const string sql = """
        SELECT
            query,
            search_type,
            COUNT(*) AS search_count
        FROM search_logs
        GROUP BY query, search_type
        ORDER BY search_count DESC
        LIMIT 10;
        """;

    var results = new List<object>();

    await using var connection =
        new MySqlConnection(_connectionString);

    await connection.OpenAsync();

    await using var command =
        new MySqlCommand(sql, connection);

    await using var reader =
        await command.ExecuteReaderAsync();

    while (await reader.ReadAsync())
    {
        results.Add(new
        {
            query = reader.GetString("query"),
            searchType = reader.GetString("search_type"),
            searchCount = reader.GetInt64("search_count")
        });
    }

    return results;
}
}