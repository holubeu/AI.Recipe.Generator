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
    public async Task<ActionResult<PaginatedResponseModel<GetIngredientResponseModel>>> GetAsync([FromQuery] GetIngredientsRequestModel requestModel)
    {
        var queryModel = requestModel.ToGetIngredientsQueryModel();
        var result = await service.GetAsync(queryModel);
        return Ok(result.ToPaginatedGetIngredientResponseModel());
    }

    [HttpGet("grouped")]
    public async Task<ActionResult<IEnumerable<GetAllIngredientsResponseModel>>> GetAllAsync()
    {
        var result = await service.GetAllAsync();
        return Ok(result.ToGetAllIngredientsResponseModels());
    }

    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody] AddIngredientRequestModel requestModel)
    {
        var commandModel = requestModel.ToAddIngredientCommandModel();
        await service.AddAsync(commandModel);
        return NoContent();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] UpdateIngredientRequestModel requestModel)
    {
        var commandModel = requestModel.ToUpdateIngredientCommandModel();
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
