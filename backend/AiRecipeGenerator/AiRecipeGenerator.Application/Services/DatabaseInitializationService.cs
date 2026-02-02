using System.Data.SQLite;

using AiRecipeGenerator.Application.Interfaces;

namespace AiRecipeGenerator.Application.Services;

public class DatabaseInitializationService : IDatabaseInitializationService
{
    private readonly string _connectionString;
    private readonly string _databasePath;

    public DatabaseInitializationService(string connectionString)
    {
        _connectionString = connectionString;
        var uriBuilder = new SQLiteConnectionStringBuilder(connectionString);
        _databasePath = uriBuilder.DataSource;
    }

    public async Task InitializeDatabaseAsync()
    {
        var databaseExists = File.Exists(_databasePath);

        if (databaseExists && await IsDatabaseNotEmptyAsync())
        {
            return;
        }

        await CreateDatabaseAsync();
        await PopulateDatabaseAsync();
    }

    private async Task CreateDatabaseAsync()
    {
        using (var connection = new SQLiteConnection(_connectionString))
        {
            await connection.OpenAsync();
            var schemaScript = await ReadScriptAsync("Scripts/Schema.sql");
            using (var command = connection.CreateCommand())
            {
                command.CommandText = schemaScript;
                await command.ExecuteNonQueryAsync();
            }
        }
    }

    private async Task PopulateDatabaseAsync()
    {
        using (var connection = new SQLiteConnection(_connectionString))
        {
            await connection.OpenAsync();
            var seedScript = await ReadScriptAsync("Scripts/Seed.sql");
            using (var command = connection.CreateCommand())
            {
                command.CommandText = seedScript;
                await command.ExecuteNonQueryAsync();
            }
        }
    }

    private async Task<bool> IsDatabaseNotEmptyAsync()
    {
        using (var connection = new SQLiteConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT COUNT(*) FROM sqlite_master WHERE type='table'";
                var result = (long?)await command.ExecuteScalarAsync();
                return result.HasValue && result.Value > 0;
            }
        }
    }

    private async Task<string> ReadScriptAsync(string relativePath)
    {
        var basePath = AppDomain.CurrentDomain.BaseDirectory;
        var fullPath = Path.Combine(basePath, relativePath);
        return await File.ReadAllTextAsync(fullPath);
    }
}
