using System.Data.Common;

using AiRecipeGenerator.Database.Interfaces;
using AiRecipeGenerator.Database.Models;
using AiRecipeGenerator.Database.Models.Commands;
using AiRecipeGenerator.Database.Models.Queries;
using AiRecipeGenerator.Database.Models.Repository;

using Dapper;

namespace AiRecipeGenerator.Database.Repositories;

public class RecipeRepository(IDbConnectionFactory connectionFactory) : IRecipeRepository
{
    public async Task<PaginatedResultModel<RecipeRepositoryModel>> GetAsync(GetRecipesQueryModel queryModel)
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

        if (!string.IsNullOrWhiteSpace(queryModel.DishType))
        {
            whereConditions.Add("DishType = @DishType");
            parameters.Add("@DishType", queryModel.DishType);
        }

        if (queryModel.CookingTimeFrom.HasValue)
        {
            whereConditions.Add("CookingTimeFrom >= @CookingTimeFrom");
            parameters.Add("@CookingTimeFrom", queryModel.CookingTimeFrom.Value);
        }

        if (queryModel.CookingTimeTo.HasValue)
        {
            whereConditions.Add("CookingTimeTo <= @CookingTimeTo");
            parameters.Add("@CookingTimeTo", queryModel.CookingTimeTo.Value);
        }

        var whereClause = whereConditions.Count > 0 ? "WHERE " + string.Join(" AND ", whereConditions) : "";

        parameters.Add("@Skip", queryModel.Skip);
        parameters.Add("@Take", queryModel.Take);

        var countQuery = $"SELECT COUNT(*) FROM Recipes {whereClause}";
        var total = await connection.QueryFirstOrDefaultAsync<int>(countQuery, parameters);

        var dataQuery = $@"SELECT * FROM Recipes {whereClause} 
                         ORDER BY Id DESC 
                         LIMIT @Take OFFSET @Skip";
        var items = await connection.QueryAsync<RecipeRepositoryModel>(dataQuery, parameters);

        return new PaginatedResultModel<RecipeRepositoryModel>
        {
            Items = items,
            Total = total
        };
    }

    public async Task UpdateAsync(UpdateRecipeCommandModel commandModel)
    {
        using var connection = (DbConnection)connectionFactory.CreateConnection();
        await connection.OpenAsync();
        var query = @"UPDATE Recipes 
                    SET Name = @Name, Description = @Description, DishType = @DishType,
                        CookingTimeFrom = @CookingTimeFrom, CookingTimeTo = @CookingTimeTo,
                        Steps = @Steps, UpdatedOn = @UpdatedOn
                    WHERE Id = @Id";
        var parameters = new
        {
            Id = commandModel.Id,
            Name = commandModel.Name,
            Description = commandModel.Description,
            DishType = commandModel.DishType,
            CookingTimeFrom = commandModel.CookingTimeFrom,
            CookingTimeTo = commandModel.CookingTimeTo,
            Steps = commandModel.Steps,
            UpdatedOn = DateTime.UtcNow.ToString("O")
        };
        await connection.ExecuteAsync(query, parameters);
    }

    public async Task AddAsync(AddRecipeCommandModel commandModel)
    {
        using var connection = (DbConnection)connectionFactory.CreateConnection();
        await connection.OpenAsync();
        var query = @"INSERT INTO Recipes (Name, Description, DishType, CookingTimeFrom, CookingTimeTo, Steps, CreatedOn, UpdatedOn)
                    VALUES (@Name, @Description, @DishType, @CookingTimeFrom, @CookingTimeTo, @Steps, @CreatedOn, @UpdatedOn)";
        var parameters = new
        {
            Name = commandModel.Name,
            Description = commandModel.Description,
            DishType = commandModel.DishType,
            CookingTimeFrom = commandModel.CookingTimeFrom,
            CookingTimeTo = commandModel.CookingTimeTo,
            Steps = commandModel.Steps,
            CreatedOn = DateTime.UtcNow.ToString("O"),
            UpdatedOn = DateTime.UtcNow.ToString("O")
        };
        await connection.ExecuteAsync(query, parameters);
    }

    public async Task DeleteAsync(int id)
    {
        using var connection = (DbConnection)connectionFactory.CreateConnection();
        await connection.OpenAsync();
        var query = "DELETE FROM Recipes WHERE Id = @Id";
        var parameters = new { Id = id };
        await connection.ExecuteAsync(query, parameters);
    }
}
