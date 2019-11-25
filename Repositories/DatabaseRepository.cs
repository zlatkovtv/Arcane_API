using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

public class DatabaseRepository: IDatabaseRepository {
    private readonly string connectionString;

    public DatabaseRepository(IConfiguration configuration) {
        connectionString = Microsoft
        .Extensions
        .Configuration
        .ConfigurationExtensions
        .GetConnectionString(configuration, "DefaultConnection");
    }

    public IDbConnection GetConnection() {
        return new SqlConnection(connectionString);
    }
}