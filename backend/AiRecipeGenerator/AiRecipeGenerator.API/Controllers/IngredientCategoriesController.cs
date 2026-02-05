using AiRecipeGenerator.API.Authentication;
using AiRecipeGenerator.API.Mappings.Requests;
using AiRecipeGenerator.API.Mappings.Responses;
using AiRecipeGenerator.API.Models.Requests;
using AiRecipeGenerator.API.Models.Responses;
using AiRecipeGenerator.Application.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace AiRecipeGenerator.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IngredientCategoriesController(IIngredientCategoryService service) : ControllerBase
{
    [HttpGet]
    [RoleAuthorize(UserRole.Admin)]
    public async Task<ActionResult<PaginatedResponseModel<GetIngredientCategoryResponseModel>>> GetAllAsync([FromQuery] GetIngredientCategoriesRequestModel requestModel)
    {
        var queryModel = requestModel.ToGetIngredientCategoriesQueryModel();
        var result = await service.GetAllAsync(queryModel);
        return Ok(result.ToPaginatedGetIngredientCategoryResponseModel());
    }

    [HttpPost]
    [RoleAuthorize(UserRole.Admin)]
    public async Task<IActionResult> AddAsync([FromBody] AddIngredientCategoryRequestModel requestModel)
    {
        var commandModel = requestModel.ToAddIngredientCategoryCommandModel();
        await service.AddAsync(commandModel);
        return NoContent();
    }

    [HttpPut]
    [RoleAuthorize(UserRole.Admin)]
    public async Task<IActionResult> UpdateAsync([FromBody] UpdateIngredientCategoryRequestModel requestModel)
    {
        var commandModel = requestModel.ToUpdateIngredientCategoryCommandModel();
        await service.UpdateAsync(commandModel);
        return NoContent();
    }
}
