using AiRecipeGenerator.Application.Services;
using AiRecipeGenerator.Database.Interfaces;
using AiRecipeGenerator.Database.Models;
using AiRecipeGenerator.Database.Models.Commands;
using AiRecipeGenerator.Database.Models.Queries;
using AiRecipeGenerator.Database.Models.Repository;

using AutoFixture.Xunit2;

using NSubstitute;

using Xunit;

namespace AiRecipeGenerator.Application.Tests.Services;

public class RecipeServiceTests
{
    [Theory, AutoData]
    public async Task GetAsync_WithValidQueryModel_CallsRepositoryGetAsync_ReturnsResult(GetRecipesQueryModel queryModel)
    {
        // Arrange
        var mockRepository = Substitute.For<IRecipeRepository>();
        var repositoryResult = new PaginatedResultModel<RecipeRepositoryModel>
        {
            Items = new List<RecipeRepositoryModel>
            {
                new() { Id = 1, Name = "Test Recipe", Steps = "[]" }
            },
            Total = 1
        };
        mockRepository.GetAsync(queryModel).Returns(repositoryResult);

        var service = new RecipeService(mockRepository);

        // Act
        var result = await service.GetAsync(queryModel);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result.Items);
        Assert.Equal(1, result.Total);
        await mockRepository.Received(1).GetAsync(queryModel);
    }

    [Theory, AutoData]
    public async Task UpdateAsync_WithValidCommandModel_CallsRepositoryUpdateAsync(UpdateRecipeCommandModel commandModel)
    {
        // Arrange
        var mockRepository = Substitute.For<IRecipeRepository>();
        var service = new RecipeService(mockRepository);

        // Act
        await service.UpdateAsync(commandModel);

        // Assert
        await mockRepository.Received(1).UpdateAsync(commandModel);
    }

    [Theory, AutoData]
    public async Task AddAsync_WithValidCommandModel_CallsRepositoryAddAsync(AddRecipeCommandModel commandModel)
    {
        // Arrange
        var mockRepository = Substitute.For<IRecipeRepository>();
        var service = new RecipeService(mockRepository);

        // Act
        await service.AddAsync(commandModel);

        // Assert
        await mockRepository.Received(1).AddAsync(commandModel);
    }

    [Theory, AutoData]
    public async Task DeleteAsync_WithValidId_CallsRepositoryDeleteAsync(int id)
    {
        // Arrange
        var mockRepository = Substitute.For<IRecipeRepository>();
        var service = new RecipeService(mockRepository);

        // Act
        await service.DeleteAsync(id);

        // Assert
        await mockRepository.Received(1).DeleteAsync(id);
    }
}
