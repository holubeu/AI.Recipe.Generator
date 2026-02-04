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

public class IngredientCategoriesControllerTests
{
    [Theory, AutoData]
    public async Task GetAllAsync_WithValidRequestModel_CallsServiceAndReturnsOkWithCategories(GetIngredientCategoriesRequestModel requestModel)
    {
        // Arrange
        var mockService = Substitute.For<IIngredientCategoryService>();
        var categoryDtos = new List<IngredientCategoryDto>
        {
            new()
            {
                Id = 1,
                Name = "Vegetables",
                CreatedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow
            }
        };

        var paginatedResult = new PaginatedResultDto<IngredientCategoryDto>
        {
            Items = categoryDtos,
            Total = 1
        };

        mockService.GetAllAsync(Arg.Any<GetIngredientCategoriesQueryModel>()).Returns(paginatedResult);

        var controller = new IngredientCategoriesController(mockService);

        // Act
        var result = await controller.GetAllAsync(requestModel);

        // Assert
        Assert.NotNull(result);
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(okResult.Value);
        await mockService.Received(1).GetAllAsync(Arg.Any<GetIngredientCategoriesQueryModel>());
    }

    [Theory, AutoData]
    public async Task AddAsync_WithValidRequestModel_CallsServiceAndReturnsNoContent(AddIngredientCategoryRequestModel requestModel)
    {
        // Arrange
        var mockService = Substitute.For<IIngredientCategoryService>();
        var controller = new IngredientCategoriesController(mockService);

        // Act
        var result = await controller.AddAsync(requestModel);

        // Assert
        Assert.IsType<NoContentResult>(result);
        await mockService.Received(1).AddAsync(Arg.Any<AddIngredientCategoryCommandModel>());
    }

    [Theory, AutoData]
    public async Task UpdateAsync_WithValidRequestModel_CallsServiceAndReturnsNoContent(UpdateIngredientCategoryRequestModel requestModel)
    {
        // Arrange
        var mockService = Substitute.For<IIngredientCategoryService>();
        var controller = new IngredientCategoriesController(mockService);

        // Act
        var result = await controller.UpdateAsync(requestModel);

        // Assert
        Assert.IsType<Microsoft.AspNetCore.Mvc.NoContentResult>(result);
        await mockService.Received(1).UpdateAsync(Arg.Any<UpdateIngredientCategoryCommandModel>());
    }
}
