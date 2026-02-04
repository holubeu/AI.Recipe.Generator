using System.Data.Common;

using AiRecipeGenerator.Database.Interfaces;
using AiRecipeGenerator.Database.Models.Commands;

using Dapper;

namespace AiRecipeGenerator.Database.Repositories;

public class ApiKeyRepository(IDbConnectionFactory connectionFactory) : IApiKeyRepository
{
    public async Task<string> GetLatestAsync()
    {
        using var connection = (DbConnection)connectionFactory.CreateConnection();
        await connection.OpenAsync();
        var query = "SELECT Key FROM ApiKeys ORDER BY CreatedOn DESC LIMIT 1";
        var key = await connection.QueryFirstOrDefaultAsync<string>(query);
        return key;
    }

    public async Task AddAsync(AddApiKeyCommandModel commandModel)
    {
        using var connection = (DbConnection)connectionFactory.CreateConnection();
        await connection.OpenAsync();
        var query = @"INSERT INTO ApiKeys (Key, CreatedOn) 
                    VALUES (@Key, @CreatedOn)";
        var parameters = new
        {
            Key = commandModel.Key,
            CreatedOn = DateTime.UtcNow.ToString("O")
        };
        await connection.ExecuteAsync(query, parameters);
    }
}
