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
public class IngredientsController(IIngredientService service) : ControllerBase
{
    [HttpGet]
    [RoleAuthorize(UserRole.Admin)]
    public async Task<ActionResult<PaginatedResponseModel<GetIngredientResponseModel>>> GetAsync([FromQuery] GetIngredientsRequestModel requestModel)
    {
        var queryModel = requestModel.ToGetIngredientsQueryModel();
        var result = await service.GetAsync(queryModel);
        return Ok(result.ToPaginatedGetIngredientResponseModel());
    }

    [HttpGet("grouped")]
    [RoleAuthorize(UserRole.User)]
    public async Task<ActionResult<IEnumerable<GetAllIngredientsResponseModel>>> GetAllAsync()
    {
        var result = await service.GetAllAsync();
        return Ok(result.ToGetAllIngredientsResponseModels());
    }

    [HttpPost]
    [RoleAuthorize(UserRole.Admin)]
    public async Task<IActionResult> AddAsync([FromBody] AddIngredientRequestModel requestModel)
    {
        var commandModel = requestModel.ToAddIngredientCommandModel();
        await service.AddAsync(commandModel);
        return NoContent();
    }

    [HttpPut]
    [RoleAuthorize(UserRole.Admin)]
    public async Task<IActionResult> UpdateAsync([FromBody] UpdateIngredientRequestModel requestModel)
    {
        var commandModel = requestModel.ToUpdateIngredientCommandModel();
        await service.UpdateAsync(commandModel);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [RoleAuthorize(UserRole.Admin)]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        await service.DeleteAsync(id);
        return NoContent();
    }
}
