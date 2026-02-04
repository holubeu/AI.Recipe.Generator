using System.Data.Common;

using AiRecipeGenerator.Database.Factories;
using AiRecipeGenerator.Database.Models.Commands;
using AiRecipeGenerator.Database.Models.Queries;
using AiRecipeGenerator.Database.Repositories;

using Xunit;

namespace AiRecipeGenerator.Database.Tests.Integration;

public class IngredientCategoryRepositoryIntegrationTests : IAsyncLifetime
{
    private const string SharedInMemoryConnectionString = "Data Source=file:memdb2?mode=memory&cache=shared";
    private DbConnection _initialConnection;

    public async Task InitializeAsync()
    {
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
            CREATE TABLE IF NOT EXISTS IngredientCategories (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                CreatedOn TEXT NOT NULL,
                UpdatedOn TEXT NOT NULL
            );";
        await command.ExecuteNonQueryAsync();
    }

    private static async Task SeedTestDataAsync(DbConnection connection, string name)
    {
        using var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO IngredientCategories (Name, CreatedOn, UpdatedOn)
            VALUES (@Name, @CreatedOn, @UpdatedOn);";
        
        command.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@Name", name));
        command.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@CreatedOn", DateTime.UtcNow.ToString("O")));
        command.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@UpdatedOn", DateTime.UtcNow.ToString("O")));

        await command.ExecuteNonQueryAsync();
    }

    private async Task ClearTableAsync(DbConnection connection)
    {
        using var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM IngredientCategories;";
        await command.ExecuteNonQueryAsync();
    }

    [Fact]
    public async Task GetAllAsync_ReturnsPaginatedCategories()
    {
        // Arrange
        var connectionFactory = new SqliteConnectionFactory(SharedInMemoryConnectionString);
        
        using (var connection = (DbConnection)connectionFactory.CreateConnection())
        {
            await InitializeDatabaseAsync(connection);
            await ClearTableAsync(connection);
            await SeedTestDataAsync(connection, "Vegetables");
            await SeedTestDataAsync(connection, "Fruits");
            await SeedTestDataAsync(connection, "Dairy");
        }

        var repository = new IngredientCategoryRepository(connectionFactory);
        var queryModel = new GetIngredientCategoriesQueryModel { Skip = 0, Take = 10 };

        // Act
        var result = await repository.GetAllAsync(queryModel);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Items);
        Assert.Equal(3, result.Total);
    }

    [Fact]
    public async Task GetAllAsync_WithPagination_ReturnsPaginatedResults()
    {
        // Arrange
        var connectionFactory = new SqliteConnectionFactory(SharedInMemoryConnectionString);
        
        using (var connection = (DbConnection)connectionFactory.CreateConnection())
        {
            await InitializeDatabaseAsync(connection);
            await ClearTableAsync(connection);
            for (int i = 0; i < 5; i++)
            {
                await SeedTestDataAsync(connection, $"Category{i}");
            }
        }

        var repository = new IngredientCategoryRepository(connectionFactory);
        var queryModel = new GetIngredientCategoriesQueryModel { Skip = 0, Take = 2 };

        // Act
        var result = await repository.GetAllAsync(queryModel);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Items.Count());
        Assert.Equal(5, result.Total);
    }

    [Fact]
    public async Task AddAsync_WithValidCategory_InsertsSuccessfully()
    {
        // Arrange
        var connectionFactory = new SqliteConnectionFactory(SharedInMemoryConnectionString);
        
        using (var connection = (DbConnection)connectionFactory.CreateConnection())
        {
            await InitializeDatabaseAsync(connection);
            await ClearTableAsync(connection);
        }

        var repository = new IngredientCategoryRepository(connectionFactory);
        var commandModel = new AddIngredientCategoryCommandModel { Name = "New Category" };

        // Act
        await repository.AddAsync(commandModel);

        // Assert - Verify insertion
        var queryModel = new GetIngredientCategoriesQueryModel { Skip = 0, Take = 10 };
        var result = await repository.GetAllAsync(queryModel);
        Assert.Single(result.Items);
        Assert.Equal("New Category", result.Items.First().Name);
    }

    [Fact]
    public async Task UpdateAsync_WithValidCategory_UpdatesSuccessfully()
    {
        // Arrange
        var connectionFactory = new SqliteConnectionFactory(SharedInMemoryConnectionString);
        
        using (var connection = (DbConnection)connectionFactory.CreateConnection())
        {
            await InitializeDatabaseAsync(connection);
            await ClearTableAsync(connection);
            await SeedTestDataAsync(connection, "Original Category");
        }

        var repository = new IngredientCategoryRepository(connectionFactory);
        
        // Get the inserted category ID
        var getQueryModel = new GetIngredientCategoriesQueryModel { Skip = 0, Take = 10 };
        var categories = await repository.GetAllAsync(getQueryModel);
        var categoryId = categories.Items.First().Id;

        var updateCommandModel = new UpdateIngredientCategoryCommandModel
        {
            Id = categoryId,
            Name = "Updated Category"
        };

        // Act
        await repository.UpdateAsync(updateCommandModel);

        // Assert - Verify update
        var updatedCategories = await repository.GetAllAsync(getQueryModel);
        Assert.Single(updatedCategories.Items);
        Assert.Equal("Updated Category", updatedCategories.Items.First().Name);
    }
}
