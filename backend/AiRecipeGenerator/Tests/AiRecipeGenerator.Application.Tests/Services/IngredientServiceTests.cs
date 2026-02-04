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

public class IngredientServiceTests
{
    [Theory, AutoData]
    public async Task GetAsync_WithValidQueryModel_CallsRepositoryGetAsync_ReturnsResult(GetIngredientsQueryModel queryModel)
    {
        // Arrange
        var mockRepository = Substitute.For<IIngredientRepository>();
        var repositoryResult = new PaginatedResultModel<IngredientRepositoryModel>
        {
            Items = new List<IngredientRepositoryModel>
            {
                new() { Id = 1, Name = "Tomato", CategoryId = 1, IsVisibleOnCard = true }
            },
            Total = 1
        };
        mockRepository.GetAsync(queryModel).Returns(repositoryResult);

        var service = new IngredientService(mockRepository);

        // Act
        var result = await service.GetAsync(queryModel);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result.Items);
        Assert.Equal(1, result.Total);
        await mockRepository.Received(1).GetAsync(queryModel);
    }

    [Theory, AutoData]
    public async Task UpdateAsync_WithValidCommandModel_CallsRepositoryUpdateAsync(UpdateIngredientCommandModel commandModel)
    {
        // Arrange
        var mockRepository = Substitute.For<IIngredientRepository>();
        var service = new IngredientService(mockRepository);

        // Act
        await service.UpdateAsync(commandModel);

        // Assert
        await mockRepository.Received(1).UpdateAsync(commandModel);
    }

    [Theory, AutoData]
    public async Task AddAsync_WithValidCommandModel_CallsRepositoryAddAsync(AddIngredientCommandModel commandModel)
    {
        // Arrange
        var mockRepository = Substitute.For<IIngredientRepository>();
        var service = new IngredientService(mockRepository);

        // Act
        await service.AddAsync(commandModel);

        // Assert
        await mockRepository.Received(1).AddAsync(commandModel);
    }

    [Theory, AutoData]
    public async Task DeleteAsync_WithValidId_CallsRepositoryDeleteAsync(int id)
    {
        // Arrange
        var mockRepository = Substitute.For<IIngredientRepository>();
        var service = new IngredientService(mockRepository);

        // Act
        await service.DeleteAsync(id);

        // Assert
        await mockRepository.Received(1).DeleteAsync(id);
    }

    [Fact]
    public async Task GetAllAsync_CallsRepositoryAndReturnsGroupedDtos()
    {
        // Arrange
        var mockRepository = Substitute.For<IIngredientRepository>();
        var repositoryResult = new List<(IngredientRepositoryModel Ingredient, string CategoryName)>
        {
            (
                new IngredientRepositoryModel
                {
                    Id = 1,
                    Name = "Tomato",
                    CategoryId = 1,
                    IsVisibleOnCard = true,
                    ImagePath = "/images/tomato.jpg",
                    CreatedOn = DateTime.UtcNow,
                    UpdatedOn = DateTime.UtcNow
                },
                "Vegetables"
            ),
            (
                new IngredientRepositoryModel
                {
                    Id = 2,
                    Name = "Carrot",
                    CategoryId = 1,
                    IsVisibleOnCard = true,
                    ImagePath = "/images/carrot.jpg",
                    CreatedOn = DateTime.UtcNow,
                    UpdatedOn = DateTime.UtcNow
                },
                "Vegetables"
            ),
            (
                new IngredientRepositoryModel
                {
                    Id = 3,
                    Name = "Apple",
                    CategoryId = 2,
                    IsVisibleOnCard = true,
                    ImagePath = "/images/apple.jpg",
                    CreatedOn = DateTime.UtcNow,
                    UpdatedOn = DateTime.UtcNow
                },
                "Fruits"
            )
        };

        mockRepository.GetAllAsync().Returns(repositoryResult);

        var service = new IngredientService(mockRepository);

        // Act
        var result = await service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        var resultList = result.ToList();
        Assert.Equal(2, resultList.Count);
        
        var vegetablesGroup = resultList.FirstOrDefault(x => x.Category == "Vegetables");
        Assert.NotNull(vegetablesGroup);
        Assert.Equal(2, vegetablesGroup.Ingredients.Count());
        
        var fruitsGroup = resultList.FirstOrDefault(x => x.Category == "Fruits");
        Assert.NotNull(fruitsGroup);
        Assert.Single(fruitsGroup.Ingredients);

        await mockRepository.Received(1).GetAllAsync();
    }
}
