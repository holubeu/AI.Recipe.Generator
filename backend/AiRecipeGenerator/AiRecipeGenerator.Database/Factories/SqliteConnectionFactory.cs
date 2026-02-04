using System.Data;
using System.Data.SQLite;

using AiRecipeGenerator.Database.Interfaces;

namespace AiRecipeGenerator.Database.Factories;

public class SqliteConnectionFactory(string connectionString) : IDbConnectionFactory
{
    private readonly string _connectionString = connectionString;

    public IDbConnection CreateConnection() => new SQLiteConnection(_connectionString);
}
