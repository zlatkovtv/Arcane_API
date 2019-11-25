using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

public interface IDatabaseRepository {
    IDbConnection GetConnection();
}