using System.Data.Common;

using AiRecipeGenerator.Database.Interfaces;
using AiRecipeGenerator.Database.Models;
using AiRecipeGenerator.Database.Models.Commands;
using AiRecipeGenerator.Database.Models.Queries;
using AiRecipeGenerator.Database.Models.Repository;

using Dapper;

namespace AiRecipeGenerator.Database.Repositories;

public class IngredientCategoryRepository(IDbConnectionFactory connectionFactory) : IIngredientCategoryRepository
{
    public async Task<PaginatedResultModel<IngredientCategoryRepositoryModel>> GetAllAsync(GetIngredientCategoriesQueryModel queryModel)
    {
        using var connection = (DbConnection)connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var parameters = new
        {
            Skip = queryModel.Skip,
            Take = queryModel.Take
        };

        var countQuery = "SELECT COUNT(*) FROM IngredientCategories";
        var total = await connection.QueryFirstOrDefaultAsync<int>(countQuery);

        var dataQuery = @"SELECT * FROM IngredientCategories 
                        ORDER BY Id 
                        LIMIT @Take OFFSET @Skip";
        var items = await connection.QueryAsync<IngredientCategoryRepositoryModel>(dataQuery, parameters);

        return new PaginatedResultModel<IngredientCategoryRepositoryModel>
        {
            Items = items,
            Total = total
        };
    }

    public async Task UpdateAsync(UpdateIngredientCategoryCommandModel commandModel)
    {
        using var connection = (DbConnection)connectionFactory.CreateConnection();
        await connection.OpenAsync();
        var query = @"UPDATE IngredientCategories 
                    SET Name = @Name, UpdatedOn = @UpdatedOn
                    WHERE Id = @Id";
        var parameters = new
        {
            Id = commandModel.Id,
            Name = commandModel.Name,
            UpdatedOn = DateTime.UtcNow.ToString("O")
        };
        await connection.ExecuteAsync(query, parameters);
    }

    public async Task AddAsync(AddIngredientCategoryCommandModel commandModel)
    {
        using var connection = (DbConnection)connectionFactory.CreateConnection();
        await connection.OpenAsync();
        var query = @"INSERT INTO IngredientCategories (Name, CreatedOn, UpdatedOn)
                    VALUES (@Name, @CreatedOn, @UpdatedOn)";
        var parameters = new
        {
            Name = commandModel.Name,
            CreatedOn = DateTime.UtcNow.ToString("O"),
            UpdatedOn = DateTime.UtcNow.ToString("O")
        };
        await connection.ExecuteAsync(query, parameters);
    }
}
