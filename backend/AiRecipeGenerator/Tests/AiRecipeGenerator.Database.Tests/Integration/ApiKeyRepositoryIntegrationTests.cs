using System.Data.Common;

using AiRecipeGenerator.Database.Factories;
using AiRecipeGenerator.Database.Repositories;

using Xunit;

namespace AiRecipeGenerator.Database.Tests.Integration;

public class ApiKeyRepositoryIntegrationTests : IAsyncLifetime
{
    // Use URI format for shared in-memory database that persists across connections
    private const string SharedInMemoryConnectionString = "Data Source=file:memdb?mode=memory&cache=shared";
    private DbConnection _initialConnection;

    public async Task InitializeAsync()
    {
        // Create initial connection to keep the shared database alive during test
        _initialConnection = new System.Data.SQLite.SQLiteConnection(SharedInMemoryConnectionString);
        await _initialConnection.OpenAsync();
    }

    public async Task DisposeAsync()
    {
        if (_initialConnection != null)
        {
            await _initialConnection.CloseAsync();
            _initialConnection.Dispose();
        }
    }

    private static async Task InitializeDatabaseAsync(DbConnection connection)
    {
        await connection.OpenAsync();
        using var command = connection.CreateCommand();
        command.CommandText = @"
            CREATE TABLE IF NOT EXISTS ApiKeys (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Key TEXT NOT NULL,
                CreatedOn TEXT NOT NULL
            );";
        await command.ExecuteNonQueryAsync();
    }

    private static async Task SeedTestDataAsync(DbConnection connection, string apiKey)
    {
        using var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO ApiKeys (Key, CreatedOn) 
            VALUES (@Key, @CreatedOn);";
        
        var keyParam = command.CreateParameter();
        keyParam.ParameterName = "@Key";
        keyParam.Value = apiKey;
        command.Parameters.Add(keyParam);

        var createdOnParam = command.CreateParameter();
        createdOnParam.ParameterName = "@CreatedOn";
        createdOnParam.Value = DateTime.UtcNow.ToString("O");
        command.Parameters.Add(createdOnParam);

        await command.ExecuteNonQueryAsync();
    }

    private async Task ClearTableAsync(DbConnection connection)
    {
        using var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM ApiKeys;";
        await command.ExecuteNonQueryAsync();
    }

    [Fact]
    public async Task GetLatestAsync_WithExistingApiKey_ReturnsLatestKey()
    {
        // Arrange
        var connectionFactory = new SqliteConnectionFactory(SharedInMemoryConnectionString);
        
        using (var connection = (DbConnection)connectionFactory.CreateConnection())
        {
            await InitializeDatabaseAsync(connection);
            await ClearTableAsync(connection);
            await SeedTestDataAsync(connection, "test-key-1");
            
            // Add a second key to ensure we get the latest one
            await System.Threading.Tasks.Task.Delay(100);
            await SeedTestDataAsync(connection, "test-key-2");
        }

        var repository = new ApiKeyRepository(connectionFactory);

        // Act
        var result = await repository.GetLatestAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal("test-key-2", result);
    }

    [Fact]
    public async Task GetLatestAsync_WithNoApiKeys_ReturnsNull()
    {
        // Arrange
        var connectionFactory = new SqliteConnectionFactory(SharedInMemoryConnectionString);
        
        using (var connection = (DbConnection)connectionFactory.CreateConnection())
        {
            await InitializeDatabaseAsync(connection);
            await ClearTableAsync(connection);
        }

        var repository = new ApiKeyRepository(connectionFactory);

        // Act
        var result = await repository.GetLatestAsync();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task AddAsync_WithValidKey_InsertsSuccessfully()
    {
        // Arrange
        var connectionFactory = new SqliteConnectionFactory(SharedInMemoryConnectionString);
        
        using (var connection = (DbConnection)connectionFactory.CreateConnection())
        {
            await InitializeDatabaseAsync(connection);
            await ClearTableAsync(connection);
        }

        var repository = new ApiKeyRepository(connectionFactory);
        var testKey = "new-test-key-12345";

        // Act
        await repository.AddAsync(new AiRecipeGenerator.Database.Models.Commands.AddApiKeyCommandModel 
        { 
            Key = testKey 
        });

        // Assert - Verify the key was inserted
        var retrievedKey = await repository.GetLatestAsync();
        Assert.Equal(testKey, retrievedKey);
    }
}
