using System.Data.Common;

using AiRecipeGenerator.Database.Factories;
using AiRecipeGenerator.Database.Models.Commands;
using AiRecipeGenerator.Database.Models.Queries;
using AiRecipeGenerator.Database.Repositories;

using Xunit;

namespace AiRecipeGenerator.Database.Tests.Integration;

public class IngredientRepositoryIntegrationTests : IAsyncLifetime
{
    private const string SharedInMemoryConnectionString = "Data Source=file:memdb3?mode=memory&cache=shared";
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
            );
            
            CREATE TABLE IF NOT EXISTS Ingredients (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                CategoryId INTEGER NOT NULL,
                IsVisibleOnCard INTEGER NOT NULL,
                CreatedOn TEXT NOT NULL,
                UpdatedOn TEXT NOT NULL,
                ImagePath TEXT,
                FOREIGN KEY (CategoryId) REFERENCES IngredientCategories(Id)
            );";
        await command.ExecuteNonQueryAsync();
    }

    private static async Task SeedCategoryAsync(DbConnection connection, int categoryId, string categoryName)
    {
        using var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO IngredientCategories (Id, Name, CreatedOn, UpdatedOn)
            VALUES (@Id, @Name, @CreatedOn, @UpdatedOn);";
        
        command.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@Id", categoryId));
        command.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@Name", categoryName));
        command.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@CreatedOn", DateTime.UtcNow.ToString("O")));
        command.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@UpdatedOn", DateTime.UtcNow.ToString("O")));

        await command.ExecuteNonQueryAsync();
    }

    private static async Task SeedTestDataAsync(DbConnection connection, string name, int categoryId, bool isVisibleOnCard)
    {
        using var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO Ingredients (Name, CategoryId, IsVisibleOnCard, CreatedOn, UpdatedOn, ImagePath)
            VALUES (@Name, @CategoryId, @IsVisibleOnCard, @CreatedOn, @UpdatedOn, @ImagePath);";
        
        command.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@Name", name));
        command.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@CategoryId", categoryId));
        command.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@IsVisibleOnCard", isVisibleOnCard ? 1 : 0));
        command.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@CreatedOn", DateTime.UtcNow.ToString("O")));
        command.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@UpdatedOn", DateTime.UtcNow.ToString("O")));
        command.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@ImagePath", DBNull.Value));

        await command.ExecuteNonQueryAsync();
    }

    private async Task ClearTablesAsync(DbConnection connection)
    {
        using var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM Ingredients; DELETE FROM IngredientCategories;";
        await command.ExecuteNonQueryAsync();
    }

    [Fact]
    public async Task GetAsync_WithoutFilters_ReturnsPaginatedIngredients()
    {
        // Arrange
        var connectionFactory = new SqliteConnectionFactory(SharedInMemoryConnectionString);
        
        using (var connection = (DbConnection)connectionFactory.CreateConnection())
        {
            await InitializeDatabaseAsync(connection);
            await ClearTablesAsync(connection);
            await SeedCategoryAsync(connection, 1, "Vegetables");
            await SeedTestDataAsync(connection, "Tomato", 1, true);
            await SeedTestDataAsync(connection, "Carrot", 1, false);
        }

        var repository = new IngredientRepository(connectionFactory);
        var queryModel = new GetIngredientsQueryModel { Skip = 0, Take = 10 };

        // Act
        var result = await repository.GetAsync(queryModel);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Items);
        Assert.Equal(2, result.Total);
    }

    [Fact]
    public async Task GetAsync_WithNameFilter_ReturnsFilteredIngredients()
    {
        // Arrange
        var connectionFactory = new SqliteConnectionFactory(SharedInMemoryConnectionString);
        
        using (var connection = (DbConnection)connectionFactory.CreateConnection())
        {
            await InitializeDatabaseAsync(connection);
            await ClearTablesAsync(connection);
            await SeedCategoryAsync(connection, 1, "Vegetables");
            await SeedTestDataAsync(connection, "Tomato", 1, true);
            await SeedTestDataAsync(connection, "Carrot", 1, false);
        }

        var repository = new IngredientRepository(connectionFactory);
        var queryModel = new GetIngredientsQueryModel { Name = "Tom", Skip = 0, Take = 10 };

        // Act
        var result = await repository.GetAsync(queryModel);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result.Items);
        Assert.Equal("Tomato", result.Items.First().Name);
    }

    [Fact]
    public async Task GetAsync_WithCategoryFilter_ReturnsFilteredIngredients()
    {
        // Arrange
        var connectionFactory = new SqliteConnectionFactory(SharedInMemoryConnectionString);
        
        using (var connection = (DbConnection)connectionFactory.CreateConnection())
        {
            await InitializeDatabaseAsync(connection);
            await ClearTablesAsync(connection);
            await SeedCategoryAsync(connection, 1, "Vegetables");
            await SeedCategoryAsync(connection, 2, "Fruits");
            await SeedTestDataAsync(connection, "Tomato", 1, true);
            await SeedTestDataAsync(connection, "Apple", 2, true);
        }

        var repository = new IngredientRepository(connectionFactory);
        var queryModel = new GetIngredientsQueryModel { CategoryId = 2, Skip = 0, Take = 10 };

        // Act
        var result = await repository.GetAsync(queryModel);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result.Items);
        Assert.Equal("Apple", result.Items.First().Name);
        Assert.Equal(2, result.Items.First().CategoryId);
    }

    [Fact]
    public async Task AddAsync_WithValidIngredient_InsertsSuccessfully()
    {
        // Arrange
        var connectionFactory = new SqliteConnectionFactory(SharedInMemoryConnectionString);
        
        using (var connection = (DbConnection)connectionFactory.CreateConnection())
        {
            await InitializeDatabaseAsync(connection);
            await ClearTablesAsync(connection);
            await SeedCategoryAsync(connection, 1, "Vegetables");
        }

        var repository = new IngredientRepository(connectionFactory);
        var commandModel = new AddIngredientCommandModel
        {
            Name = "New Ingredient",
            CategoryId = 1,
            IsVisibleOnCard = true,
            ImagePath = null
        };

        // Act
        await repository.AddAsync(commandModel);

        // Assert - Verify insertion
        var queryModel = new GetIngredientsQueryModel { Skip = 0, Take = 10 };
        var result = await repository.GetAsync(queryModel);
        Assert.Single(result.Items);
        Assert.Equal("New Ingredient", result.Items.First().Name);
        Assert.True(result.Items.First().IsVisibleOnCard);
    }

    [Fact]
    public async Task UpdateAsync_WithValidIngredient_UpdatesSuccessfully()
    {
        // Arrange
        var connectionFactory = new SqliteConnectionFactory(SharedInMemoryConnectionString);
        
        using (var connection = (DbConnection)connectionFactory.CreateConnection())
        {
            await InitializeDatabaseAsync(connection);
            await ClearTablesAsync(connection);
            await SeedCategoryAsync(connection, 1, "Vegetables");
            await SeedCategoryAsync(connection, 2, "Fruits");
            await SeedTestDataAsync(connection, "Original Ingredient", 1, false);
        }

        var repository = new IngredientRepository(connectionFactory);
        
        // Get the inserted ingredient ID
        var getQueryModel = new GetIngredientsQueryModel { Skip = 0, Take = 10 };
        var ingredients = await repository.GetAsync(getQueryModel);
        var ingredientId = ingredients.Items.First().Id;

        var updateCommandModel = new UpdateIngredientCommandModel
        {
            Id = ingredientId,
            Name = "Updated Ingredient",
            CategoryId = 2,
            IsVisibleOnCard = true,
            ImagePath = "/images/updated.jpg"
        };

        // Act
        await repository.UpdateAsync(updateCommandModel);

        // Assert - Verify update
        var updatedIngredients = await repository.GetAsync(getQueryModel);
        Assert.Single(updatedIngredients.Items);
        Assert.Equal("Updated Ingredient", updatedIngredients.Items.First().Name);
        Assert.Equal(2, updatedIngredients.Items.First().CategoryId);
        Assert.True(updatedIngredients.Items.First().IsVisibleOnCard);
    }

    [Fact]
    public async Task DeleteAsync_WithValidId_DeletesSuccessfully()
    {
        // Arrange
        var connectionFactory = new SqliteConnectionFactory(SharedInMemoryConnectionString);
        
        using (var connection = (DbConnection)connectionFactory.CreateConnection())
        {
            await InitializeDatabaseAsync(connection);
            await ClearTablesAsync(connection);
            await SeedCategoryAsync(connection, 1, "Vegetables");
            await SeedTestDataAsync(connection, "Ingredient to Delete", 1, true);
        }

        var repository = new IngredientRepository(connectionFactory);
        
        // Get the inserted ingredient ID
        var getQueryModel = new GetIngredientsQueryModel { Skip = 0, Take = 10 };
        var ingredients = await repository.GetAsync(getQueryModel);
        var ingredientId = ingredients.Items.First().Id;

        // Act
        await repository.DeleteAsync(ingredientId);

        // Assert - Verify deletion
        var remainingIngredients = await repository.GetAsync(getQueryModel);
        Assert.Empty(remainingIngredients.Items);
    }

    [Fact]
    public async Task GetAllAsync_WithMultipleCategoriesAndIngredients_ReturnsGroupedResults()
    {
        // Arrange
        var connectionFactory = new SqliteConnectionFactory(SharedInMemoryConnectionString);
        
        using (var connection = (DbConnection)connectionFactory.CreateConnection())
        {
            await InitializeDatabaseAsync(connection);
            await ClearTablesAsync(connection);
            
            await SeedCategoryAsync(connection, 1, "Vegetables");
            await SeedCategoryAsync(connection, 2, "Fruits");
            
            await SeedTestDataAsync(connection, "Tomato", 1, true);
            await SeedTestDataAsync(connection, "Carrot", 1, false);
            await SeedTestDataAsync(connection, "Apple", 2, true);
        }

        var repository = new IngredientRepository(connectionFactory);

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        var resultList = result.ToList();
        Assert.NotEmpty(resultList);

        var vegetables = resultList.Where(x => x.CategoryName == "Vegetables").ToList();
        Assert.Equal(2, vegetables.Count);
        Assert.Contains(vegetables, x => x.Ingredient.Name == "Tomato" && x.Ingredient.IsVisibleOnCard);
        Assert.Contains(vegetables, x => x.Ingredient.Name == "Carrot" && !x.Ingredient.IsVisibleOnCard);

        var fruits = resultList.Where(x => x.CategoryName == "Fruits").ToList();
        Assert.Single(fruits);
        Assert.Equal("Apple", fruits[0].Ingredient.Name);
    }

    [Fact]
    public async Task GetAllAsync_WithEmptyDatabase_ReturnsEmptyCollection()
    {
        // Arrange
        var connectionFactory = new SqliteConnectionFactory(SharedInMemoryConnectionString);
        
        using (var connection = (DbConnection)connectionFactory.CreateConnection())
        {
            await InitializeDatabaseAsync(connection);
            await ClearTablesAsync(connection);
        }

        var repository = new IngredientRepository(connectionFactory);

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
}
