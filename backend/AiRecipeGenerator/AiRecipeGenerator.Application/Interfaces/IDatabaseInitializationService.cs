namespace AiRecipeGenerator.Application.Interfaces;

public interface IDatabaseInitializationService
{
    Task InitializeDatabaseAsync();
}
