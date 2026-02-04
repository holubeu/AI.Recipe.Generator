using System.Data.Common;

using AiRecipeGenerator.Database.Factories;
using AiRecipeGenerator.Database.Models.Commands;
using AiRecipeGenerator.Database.Models.Queries;
using AiRecipeGenerator.Database.Repositories;

using Xunit;

namespace AiRecipeGenerator.Database.Tests.Integration;

public class RecipeRepositoryIntegrationTests : IAsyncLifetime
{
    private const string SharedInMemoryConnectionString = "Data Source=file:memdb1?mode=memory&cache=shared";
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
            CREATE TABLE IF NOT EXISTS Recipes (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                Description TEXT,
                DishType TEXT,
                CookingTimeFrom INTEGER NOT NULL,
                CookingTimeTo INTEGER NOT NULL,
                Steps TEXT NOT NULL,
                CreatedOn TEXT NOT NULL,
                UpdatedOn TEXT NOT NULL
            );";
        await command.ExecuteNonQueryAsync();
    }

    private static async Task SeedTestDataAsync(DbConnection connection, string name, string dishType, int cookingTimeFrom, int cookingTimeTo)
    {
        using var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO Recipes (Name, Description, DishType, CookingTimeFrom, CookingTimeTo, Steps, CreatedOn, UpdatedOn)
            VALUES (@Name, @Description, @DishType, @CookingTimeFrom, @CookingTimeTo, @Steps, @CreatedOn, @UpdatedOn);";
        
        command.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@Name", name));
        command.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@Description", "Test description"));
        command.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@DishType", dishType));
        command.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@CookingTimeFrom", cookingTimeFrom));
        command.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@CookingTimeTo", cookingTimeTo));
        command.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@Steps", "[\"step 1\", \"step 2\"]"));
        command.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@CreatedOn", DateTime.UtcNow.ToString("O")));
        command.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@UpdatedOn", DateTime.UtcNow.ToString("O")));

        await command.ExecuteNonQueryAsync();
    }

    private async Task ClearTableAsync(DbConnection connection)
    {
        using var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM Recipes;";
        await command.ExecuteNonQueryAsync();
    }

    [Fact]
    public async Task GetAsync_WithoutFilters_ReturnsPaginatedRecipes()
    {
        // Arrange
        var connectionFactory = new SqliteConnectionFactory(SharedInMemoryConnectionString);
        
        using (var connection = (DbConnection)connectionFactory.CreateConnection())
        {
            await InitializeDatabaseAsync(connection);
            await ClearTableAsync(connection);
            await SeedTestDataAsync(connection, "Pasta", "Main", 20, 30);
            await SeedTestDataAsync(connection, "Salad", "Starter", 5, 10);
        }

        var repository = new RecipeRepository(connectionFactory);
        var queryModel = new GetRecipesQueryModel { Skip = 0, Take = 10 };

        // Act
        var result = await repository.GetAsync(queryModel);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Items);
        Assert.Equal(2, result.Total);
    }

    [Fact]
    public async Task GetAsync_WithNameFilter_ReturnsFilteredRecipes()
    {
        // Arrange
        var connectionFactory = new SqliteConnectionFactory(SharedInMemoryConnectionString);
        
        using (var connection = (DbConnection)connectionFactory.CreateConnection())
        {
            await InitializeDatabaseAsync(connection);
            await ClearTableAsync(connection);
            await SeedTestDataAsync(connection, "Pasta Carbonara", "Main", 20, 30);
            await SeedTestDataAsync(connection, "Caesar Salad", "Starter", 5, 10);
        }

        var repository = new RecipeRepository(connectionFactory);
        var queryModel = new GetRecipesQueryModel { Name = "Pasta", Skip = 0, Take = 10 };

        // Act
        var result = await repository.GetAsync(queryModel);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result.Items);
        Assert.Equal("Pasta Carbonara", result.Items.First().Name);
    }

    [Fact]
    public async Task AddAsync_WithValidRecipe_InsertsSuccessfully()
    {
        // Arrange
        var connectionFactory = new SqliteConnectionFactory(SharedInMemoryConnectionString);
        
        using (var connection = (DbConnection)connectionFactory.CreateConnection())
        {
            await InitializeDatabaseAsync(connection);
            await ClearTableAsync(connection);
        }

        var repository = new RecipeRepository(connectionFactory);
        var commandModel = new AddRecipeCommandModel
        {
            Name = "New Recipe",
            Description = "A new test recipe",
            DishType = "Main",
            CookingTimeFrom = 30,
            CookingTimeTo = 45,
            Steps = "[\"step 1\", \"step 2\", \"step 3\"]"
        };

        // Act
        await repository.AddAsync(commandModel);

        // Assert - Verify insertion
        var queryModel = new GetRecipesQueryModel { Skip = 0, Take = 10 };
        var result = await repository.GetAsync(queryModel);
        Assert.Single(result.Items);
        Assert.Equal("New Recipe", result.Items.First().Name);
    }

    [Fact]
    public async Task UpdateAsync_WithValidRecipe_UpdatesSuccessfully()
    {
        // Arrange
        var connectionFactory = new SqliteConnectionFactory(SharedInMemoryConnectionString);
        
        using (var connection = (DbConnection)connectionFactory.CreateConnection())
        {
            await InitializeDatabaseAsync(connection);
            await ClearTableAsync(connection);
            await SeedTestDataAsync(connection, "Original Recipe", "Main", 20, 30);
        }

        var repository = new RecipeRepository(connectionFactory);
        
        // Get the inserted recipe ID
        var getQueryModel = new GetRecipesQueryModel { Skip = 0, Take = 10 };
        var recipes = await repository.GetAsync(getQueryModel);
        var recipeId = recipes.Items.First().Id;

        var updateCommandModel = new UpdateRecipeCommandModel
        {
            Id = recipeId,
            Name = "Updated Recipe",
            Description = "Updated description",
            DishType = "Dessert",
            CookingTimeFrom = 15,
            CookingTimeTo = 25,
            Steps = "[\"new step 1\", \"new step 2\"]"
        };

        // Act
        await repository.UpdateAsync(updateCommandModel);

        // Assert - Verify update
        var updatedRecipes = await repository.GetAsync(getQueryModel);
        Assert.Single(updatedRecipes.Items);
        Assert.Equal("Updated Recipe", updatedRecipes.Items.First().Name);
        Assert.Equal("Dessert", updatedRecipes.Items.First().DishType);
    }

    [Fact]
    public async Task DeleteAsync_WithValidId_DeletesSuccessfully()
    {
        // Arrange
        var connectionFactory = new SqliteConnectionFactory(SharedInMemoryConnectionString);
        
        using (var connection = (DbConnection)connectionFactory.CreateConnection())
        {
            await InitializeDatabaseAsync(connection);
            await ClearTableAsync(connection);
            await SeedTestDataAsync(connection, "Recipe to Delete", "Main", 20, 30);
        }

        var repository = new RecipeRepository(connectionFactory);
        
        // Get the inserted recipe ID
        var getQueryModel = new GetRecipesQueryModel { Skip = 0, Take = 10 };
        var recipes = await repository.GetAsync(getQueryModel);
        var recipeId = recipes.Items.First().Id;

        // Act
        await repository.DeleteAsync(recipeId);

        // Assert - Verify deletion
        var remainingRecipes = await repository.GetAsync(getQueryModel);
        Assert.Empty(remainingRecipes.Items);
    }
}
