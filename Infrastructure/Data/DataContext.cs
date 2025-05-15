using Npgsql;

namespace Infrastructure.Data;

public class DataContext
{
    private const string connectionString = "Host=localhost;Port=5432;Database=cw-14-05;Username=postgres;Password=1234";
    
    public Task<NpgsqlConnection> GetNpgsqlConnection()
    {
        return Task.FromResult(new NpgsqlConnection(connectionString));
    }
}
