using AiRecipeGenerator.API.Mappings.Requests;
using AiRecipeGenerator.API.Mappings.Responses;
using AiRecipeGenerator.API.Models.Requests;
using AiRecipeGenerator.API.Models.Responses;
using AiRecipeGenerator.Application.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace AiRecipeGenerator.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecipesController(IRecipeService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PaginatedResponseModel<GetRecipeResponseModel>>> GetAsync([FromQuery] GetRecipesRequestModel requestModel)
    {
        var queryModel = requestModel.ToGetRecipesQueryModel();
        var result = await service.GetAsync(queryModel);
        return Ok(result.ToPaginatedGetRecipeResponseModel());
    }

    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody] AddRecipeRequestModel requestModel)
    {
        var commandModel = requestModel.ToAddRecipeCommandModel();
        await service.AddAsync(commandModel);
        return NoContent();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] UpdateRecipeRequestModel requestModel)
    {
        var commandModel = requestModel.ToUpdateRecipeCommandModel();
        await service.UpdateAsync(commandModel);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        await service.DeleteAsync(id);
        return NoContent();
    }
}
