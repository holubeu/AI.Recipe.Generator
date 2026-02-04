using AiRecipeGenerator.API.Controllers;
using AiRecipeGenerator.API.Models.Requests;
using AiRecipeGenerator.Application.Dtos;
using AiRecipeGenerator.Application.Interfaces;
using AiRecipeGenerator.Database.Models.Commands;
using AiRecipeGenerator.Database.Models.Queries;

using AutoFixture.Xunit2;

using NSubstitute;

using Xunit;

namespace AiRecipeGenerator.API.Tests.Controllers;

public class RecipesControllerTests
{
    [Theory, AutoData]
    public async Task GetAsync_WithValidRequestModel_CallsServiceAndReturnsOkWithRecipes(GetRecipesRequestModel requestModel)
    {
        // Arrange
        var mockService = Substitute.For<IRecipeService>();
        var recipeDtos = new List<RecipeDto>
        {
            new()
            {
                Id = 1,
                Name = "Test Recipe",
                Description = "Test",
                DishType = "Main",
                CookingTimeFrom = 20,
                CookingTimeTo = 30,
                Steps = new[] { "Step 1", "Step 2" },
                CreatedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow
            }
        };

        var paginatedResult = new PaginatedResultDto<RecipeDto>
        {
            Items = recipeDtos,
            Total = 1
        };

        mockService.GetAsync(Arg.Any<GetRecipesQueryModel>()).Returns(paginatedResult);

        var controller = new RecipesController(mockService);

        // Act
        var result = await controller.GetAsync(requestModel);

        // Assert
        Assert.NotNull(result);
        var okResult = Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(result.Result);
        Assert.NotNull(okResult.Value);
        await mockService.Received(1).GetAsync(Arg.Any<GetRecipesQueryModel>());
    }

    [Theory, AutoData]
    public async Task AddAsync_WithValidRequestModel_CallsServiceAndReturnsNoContent(AddRecipeRequestModel requestModel)
    {
        // Arrange
        var mockService = Substitute.For<IRecipeService>();
        var controller = new RecipesController(mockService);

        // Act
        var result = await controller.AddAsync(requestModel);

        // Assert
        Assert.IsType<Microsoft.AspNetCore.Mvc.NoContentResult>(result);
        await mockService.Received(1).AddAsync(Arg.Any<AiRecipeGenerator.Database.Models.Commands.AddRecipeCommandModel>());
    }

    [Theory, AutoData]
    public async Task UpdateAsync_WithValidRequestModel_CallsServiceAndReturnsNoContent(UpdateRecipeRequestModel requestModel)
    {
        // Arrange
        var mockService = Substitute.For<IRecipeService>();
        var controller = new RecipesController(mockService);

        // Act
        var result = await controller.UpdateAsync(requestModel);

        // Assert
        Assert.IsType<Microsoft.AspNetCore.Mvc.NoContentResult>(result);
        await mockService.Received(1).UpdateAsync(Arg.Any<UpdateRecipeCommandModel>());
    }

    [Theory, AutoData]
    public async Task DeleteAsync_WithValidId_CallsServiceAndReturnsNoContent(int id)
    {
        // Arrange
        var mockService = Substitute.For<IRecipeService>();
        var controller = new RecipesController(mockService);

        // Act
        var result = await controller.DeleteAsync(id);

        // Assert
        Assert.IsType<Microsoft.AspNetCore.Mvc.NoContentResult>(result);
        await mockService.Received(1).DeleteAsync(id);
    }
}
