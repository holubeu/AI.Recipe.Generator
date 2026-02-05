using AiRecipeGenerator.API.Authentication;
using AiRecipeGenerator.API.Mappings.Responses;
using AiRecipeGenerator.API.Models.Requests;
using AiRecipeGenerator.API.Models.Responses;
using AiRecipeGenerator.Application.Interfaces;
using AiRecipeGenerator.Database.Models.Commands;

using Microsoft.AspNetCore.Mvc;

namespace AiRecipeGenerator.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApiKeysController(IApiKeyService service) : ControllerBase
{
    [HttpGet("latest")]
    [RoleAuthorize(UserRole.User)]
    public async Task<ActionResult<GetLatestApiKeyResponseModel>> GetLatestAsync()
    {
        var result = await service.GetLatestAsync();
        return Ok(result.ToGetLatestApiKeyResponseModel());
    }

    [HttpPost]
    [RoleAuthorize(UserRole.Admin)]
    public async Task<IActionResult> AddAsync([FromBody] AddApiKeyRequestModel requestModel)
    {
        var commandModel = new AddApiKeyCommandModel { Key = requestModel.Key };
        await service.AddAsync(commandModel);
        return NoContent();
    }
}
