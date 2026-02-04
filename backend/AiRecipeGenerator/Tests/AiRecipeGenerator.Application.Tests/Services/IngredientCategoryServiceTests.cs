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

public class IngredientCategoryServiceTests
{
    [Theory, AutoData]
    public async Task GetAllAsync_WithValidQueryModel_CallsRepositoryGetAllAsync_ReturnsResult(GetIngredientCategoriesQueryModel queryModel)
    {
        // Arrange
        var mockRepository = Substitute.For<IIngredientCategoryRepository>();
        var repositoryResult = new PaginatedResultModel<IngredientCategoryRepositoryModel>
        {
            Items = new List<IngredientCategoryRepositoryModel>
            {
                new() { Id = 1, Name = "Vegetables" }
            },
            Total = 1
        };
        mockRepository.GetAllAsync(queryModel).Returns(repositoryResult);

        var service = new IngredientCategoryService(mockRepository);

        // Act
        var result = await service.GetAllAsync(queryModel);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result.Items);
        Assert.Equal(1, result.Total);
        await mockRepository.Received(1).GetAllAsync(queryModel);
    }

    [Theory, AutoData]
    public async Task UpdateAsync_WithValidCommandModel_CallsRepositoryUpdateAsync(UpdateIngredientCategoryCommandModel commandModel)
    {
        // Arrange
        var mockRepository = Substitute.For<IIngredientCategoryRepository>();
        var service = new IngredientCategoryService(mockRepository);

        // Act
        await service.UpdateAsync(commandModel);

        // Assert
        await mockRepository.Received(1).UpdateAsync(commandModel);
    }

    [Theory, AutoData]
    public async Task AddAsync_WithValidCommandModel_CallsRepositoryAddAsync(AddIngredientCategoryCommandModel commandModel)
    {
        // Arrange
        var mockRepository = Substitute.For<IIngredientCategoryRepository>();
        var service = new IngredientCategoryService(mockRepository);

        // Act
        await service.AddAsync(commandModel);

        // Assert
        await mockRepository.Received(1).AddAsync(commandModel);
    }
}
