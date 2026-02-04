using AiRecipeGenerator.API.Controllers;
using AiRecipeGenerator.API.Models.Requests;
using AiRecipeGenerator.Application.Dtos;
using AiRecipeGenerator.Application.Interfaces;
using AiRecipeGenerator.Database.Models.Commands;
using AiRecipeGenerator.Database.Models.Queries;

using AutoFixture.Xunit2;

using Microsoft.AspNetCore.Mvc;

using NSubstitute;

using Xunit;

namespace AiRecipeGenerator.API.Tests.Controllers;

public class IngredientsControllerTests
{
    [Theory, AutoData]
    public async Task GetAsync_WithValidRequestModel_CallsServiceAndReturnsOkWithIngredients(GetIngredientsRequestModel requestModel)
    {
        // Arrange
        var mockService = Substitute.For<IIngredientService>();
        var ingredientDtos = new List<IngredientDto>
        {
            new()
            {
                Id = 1,
                Name = "Tomato",
                CategoryId = 1,
                IsVisibleOnCard = true,
                CreatedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow,
                ImagePath = "/images/tomato.jpg"
            }
        };

        var paginatedResult = new PaginatedResultDto<IngredientDto>
        {
            Items = ingredientDtos,
            Total = 1
        };

        mockService.GetAsync(Arg.Any<GetIngredientsQueryModel>()).Returns(paginatedResult);

        var controller = new IngredientsController(mockService);

        // Act
        var result = await controller.GetAsync(requestModel);

        // Assert
        Assert.NotNull(result);
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(okResult.Value);
        await mockService.Received(1).GetAsync(Arg.Any<GetIngredientsQueryModel>());
    }

    [Theory, AutoData]
    public async Task AddAsync_WithValidRequestModel_CallsServiceAndReturnsNoContent(AddIngredientRequestModel requestModel)
    {
        // Arrange
        var mockService = Substitute.For<IIngredientService>();
        var controller = new IngredientsController(mockService);

        // Act
        var result = await controller.AddAsync(requestModel);

        // Assert
        Assert.IsType<NoContentResult>(result);
        await mockService.Received(1).AddAsync(Arg.Any<AddIngredientCommandModel>());
    }

    [Theory, AutoData]
    public async Task UpdateAsync_WithValidRequestModel_CallsServiceAndReturnsNoContent(UpdateIngredientRequestModel requestModel)
    {
        // Arrange
        var mockService = Substitute.For<IIngredientService>();
        var controller = new IngredientsController(mockService);

        // Act
        var result = await controller.UpdateAsync(requestModel);

        // Assert
        Assert.IsType<NoContentResult>(result);
        await mockService.Received(1).UpdateAsync(Arg.Any<UpdateIngredientCommandModel>());
    }

    [Theory, AutoData]
    public async Task DeleteAsync_WithValidId_CallsServiceAndReturnsNoContent(int id)
    {
        // Arrange
        var mockService = Substitute.For<IIngredientService>();
        var controller = new IngredientsController(mockService);

        // Act
        var result = await controller.DeleteAsync(id);

        // Assert
        Assert.IsType<NoContentResult>(result);
        await mockService.Received(1).DeleteAsync(id);
    }

    [Theory, AutoData]
    public async Task GetAllAsync_CallsServiceAndReturnsOkWithGroupedIngredients(List<GetAllIngredientsDto> ingredientsByCategory)
    {
        // Arrange
        var mockService = Substitute.For<IIngredientService>();

        mockService.GetAllAsync().Returns(ingredientsByCategory);

        var controller = new IngredientsController(mockService);

        // Act
        var result = await controller.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(okResult.Value);
        await mockService.Received(1).GetAllAsync();
    }
}
