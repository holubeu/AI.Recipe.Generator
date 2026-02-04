using System.Data;

namespace AiRecipeGenerator.Database.Interfaces;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}
