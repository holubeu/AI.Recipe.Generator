using System.Data.Common;

using AiRecipeGenerator.Database.Interfaces;
using AiRecipeGenerator.Database.Models;
using AiRecipeGenerator.Database.Models.Commands;
using AiRecipeGenerator.Database.Models.Queries;
using AiRecipeGenerator.Database.Models.Repository;

using Dapper;

namespace AiRecipeGenerator.Database.Repositories;

public class IngredientRepository(IDbConnectionFactory connectionFactory) : IIngredientRepository
{
    public async Task<PaginatedResultModel<IngredientRepositoryModel>> GetAsync(GetIngredientsQueryModel queryModel)
    {
        using var connection = (DbConnection)connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var whereConditions = new List<string>();
        var parameters = new DynamicParameters();

        if (!string.IsNullOrWhiteSpace(queryModel.Name))
        {
            whereConditions.Add("Name LIKE @Name");
            parameters.Add("@Name", $"%{queryModel.Name}%");
        }

        if (queryModel.CategoryId.HasValue)
        {
            whereConditions.Add("CategoryId = @CategoryId");
            parameters.Add("@CategoryId", queryModel.CategoryId.Value);
        }

        var whereClause = whereConditions.Count > 0 ? "WHERE " + string.Join(" AND ", whereConditions) : "";

        parameters.Add("@Skip", queryModel.Skip);
        parameters.Add("@Take", queryModel.Take);

        var countQuery = $"SELECT COUNT(*) FROM Ingredients {whereClause}";
        var total = await connection.QueryFirstOrDefaultAsync<int>(countQuery, parameters);

        var dataQuery = $@"SELECT * FROM Ingredients {whereClause} 
                         ORDER BY Id 
                         LIMIT @Take OFFSET @Skip";
        var items = await connection.QueryAsync<IngredientRepositoryModel>(dataQuery, parameters);

        return new PaginatedResultModel<IngredientRepositoryModel>
        {
            Items = items,
            Total = total
        };
    }

    public async Task UpdateAsync(UpdateIngredientCommandModel commandModel)
    {
        using var connection = (DbConnection)connectionFactory.CreateConnection();
        await connection.OpenAsync();
        var query = @"UPDATE Ingredients 
                    SET Name = @Name, CategoryId = @CategoryId, IsVisibleOnCard = @IsVisibleOnCard,
                        ImagePath = @ImagePath, UpdatedOn = @UpdatedOn
                    WHERE Id = @Id";
        var parameters = new
        {
            Id = commandModel.Id,
            Name = commandModel.Name,
            CategoryId = commandModel.CategoryId,
            IsVisibleOnCard = commandModel.IsVisibleOnCard ? 1 : 0,
            ImagePath = commandModel.ImagePath,
            UpdatedOn = DateTime.UtcNow.ToString("O")
        };
        await connection.ExecuteAsync(query, parameters);
    }

    public async Task AddAsync(AddIngredientCommandModel commandModel)
    {
        using var connection = (DbConnection)connectionFactory.CreateConnection();
        await connection.OpenAsync();
        var query = @"INSERT INTO Ingredients (Name, CategoryId, IsVisibleOnCard, ImagePath, CreatedOn, UpdatedOn)
                    VALUES (@Name, @CategoryId, @IsVisibleOnCard, @ImagePath, @CreatedOn, @UpdatedOn)";
        var parameters = new
        {
            Name = commandModel.Name,
            CategoryId = commandModel.CategoryId,
            IsVisibleOnCard = commandModel.IsVisibleOnCard ? 1 : 0,
            ImagePath = commandModel.ImagePath,
            CreatedOn = DateTime.UtcNow.ToString("O"),
            UpdatedOn = DateTime.UtcNow.ToString("O")
        };
        await connection.ExecuteAsync(query, parameters);
    }

    public async Task DeleteAsync(int id)
    {
        using var connection = (DbConnection)connectionFactory.CreateConnection();
        await connection.OpenAsync();
        var query = "DELETE FROM Ingredients WHERE Id = @Id";
        var parameters = new { Id = id };
        await connection.ExecuteAsync(query, parameters);
    }

    public async Task<IEnumerable<(IngredientRepositoryModel Ingredient, string CategoryName)>> GetAllAsync()
    {
        using var connection = (DbConnection)connectionFactory.CreateConnection();
        await connection.OpenAsync();
        var query = @"SELECT i.*, ic.Name as CategoryName FROM Ingredients i
                    INNER JOIN IngredientCategories ic ON i.CategoryId = ic.Id
                    ORDER BY ic.Name, i.Name";
        
        var result = await connection.QueryAsync<dynamic>(query);
        
        return result.Select(row => (
            new IngredientRepositoryModel
            {
                Id = (int)row.Id,
                Name = row.Name,
                CategoryId = (int)row.CategoryId,
                IsVisibleOnCard = Convert.ToBoolean(row.IsVisibleOnCard),
                CreatedOn = DateTime.ParseExact((string)row.CreatedOn, "O", System.Globalization.CultureInfo.InvariantCulture),
                UpdatedOn = DateTime.ParseExact((string)row.UpdatedOn, "O", System.Globalization.CultureInfo.InvariantCulture),
                ImagePath = row.ImagePath
            },
            (string)row.CategoryName
        ));
    }
}
