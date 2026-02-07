using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

using AiRecipeGenerator.Application.Dtos;
using AiRecipeGenerator.Application.Exceptions;
using AiRecipeGenerator.Application.Interfaces;
using AiRecipeGenerator.Application.Mappings;
using AiRecipeGenerator.Application.Models.OpenRouter;
using AiRecipeGenerator.Database.Models.Queries;

namespace AiRecipeGenerator.Application.Services;

public class OpenRouterService(IApiKeyService apiKeyService, HttpClient httpClient) : IOpenRouterService
{
    private const string OpenRouterApiUrl = "https://openrouter.ai/api/v1/chat/completions";
    private const string ModelName = "arcee-ai/trinity-large-preview:free";

    private const string PromptTemplate = @"You are a recipe generator. Your task is to provide cooking recipes based on a given list of ingredients.
Instructions:
- Generate one real existing recipe using the following ingredients: {ingredients}.
- A recipe may omit some of the listed ingredients.
- A recipe may include ingredients not from the list only if they are spices, water, salt, pepper, or decorative elements.
- Do not invent fictional recipes. Only provide real recipes.
- The recipe must describe the cooking process step by step.
- Do not format the text inside JSON fields (e.g., do not highlight fields with *).
- Specify temperature units in degrees Celsius, using the °C symbol.
- Put true to the recipeFound field of JSON if a recipe is found, otherwise put false.
- If no recipe can be found for the given ingredients, write a message stating that and put it to message field of the JSON.
- Put the name of the dish to recipe.name field of the JSON.
- Put a dish type if it exist to recipe.dishType field of the JSON. For example Salads, Hot dishes, Cold starters.
- Put a short description of the dish if the description was found to recipe.description field of the JSON.
- Put each step into a separate element of the steps array of the JSON.
- Put the minimum cooking time in minutes to cookingTime.from field of the JSON.
- Put the maximum cooking time in minutes to cookingTime.to field of the JSON.
- The response must contain only JSON. There must be no characters before or after the JSON.
- Output must be in JSON format with the following structure:
{
  ""recipeFound"": ""boolean"",
  ""message"": ""string"",
  ""recipe"": {
    ""name"": ""string"",
    ""description"": ""string"",
    ""steps"": [
      ""string""
    ],
    ""cookingTime"": {
      ""from"": ""number"",
      ""to"": "" number ""
    }
  }
}
";

    public async Task<GeneratedRecipeDto> GenerateRecipeAsync(GenerateRecipeQueryModel queryModel)
    {
        ArgumentNullException.ThrowIfNull(queryModel);
        ArgumentNullException.ThrowIfNull(queryModel.Ingredients);

        var apiKey = await apiKeyService.GetLatestAsync();
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException("API key not found. Please configure an OpenRouter API key.");
        }

        var ingredientsString = string.Join(", ", queryModel.Ingredients);
        var prompt = PromptTemplate.Replace("{ingredients}", ingredientsString);

        var request = new OpenRouterRequest
        {
            Model = ModelName,
            Messages =
            [
                new OpenRouterRequestMessage
                {
                    Role = "user",
                    Content = prompt
                }
            ]
        };

        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, OpenRouterApiUrl)
        {
            Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")
        };

        httpRequest.Headers.Add("Authorization", $"Bearer {apiKey}");

        var response = await httpClient.SendAsync(httpRequest);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var openRouterResponse = DeserializeOpenRouterResponse(responseContent);

        if (openRouterResponse?.Choices == null || openRouterResponse.Choices.Length == 0)
        {
            throw new InvalidRecipeContentException("No choices found in OpenRouter response");
        }

        var contentJson = openRouterResponse.Choices[0].Message.Content;
        contentJson = ExtractAndCleanJson(contentJson);

        var recipeContent = DeserializeRecipeContent(contentJson);

        return recipeContent.ToGeneratedRecipeDto();
    }

    private static OpenRouterResponse DeserializeOpenRouterResponse(string responseContent)
    {
        ArgumentNullException.ThrowIfNull(responseContent);

        try
        {
            var openRouterResponse = JsonSerializer.Deserialize<OpenRouterResponse>(responseContent);
            return openRouterResponse;
        }
        catch (JsonException ex)
        {
            throw new InvalidRecipeContentException($"Failed to deserialize OpenRouter response: {responseContent}", ex);
        }
    }

    private static RecipeContent DeserializeRecipeContent(string contentJson)
    {
        ArgumentNullException.ThrowIfNull(contentJson);

        RecipeContent recipeContent;
        try
        {
            recipeContent = JsonSerializer.Deserialize<RecipeContent>(contentJson);
        }
        catch (JsonException ex)
        {
            throw new InvalidRecipeContentException($"Failed to deserialize recipe content: {contentJson}", ex);
        }

        if (recipeContent == null)
        {
            throw new InvalidRecipeContentException("Recipe content deserialized to null");
        }

        return recipeContent;
    }

    private static string ExtractAndCleanJson(string content)
    {
        ArgumentNullException.ThrowIfNull(content);

        // Extract JSON from markdown code block if present
        if (content.StartsWith("```json"))
        {
            content = content.Substring(7); // Remove ```json
        }
        else if (content.StartsWith("```"))
        {
            content = content.Substring(3); // Remove ```
        }

        if (content.EndsWith("```"))
        {
            content = content.Substring(0, content.Length - 3); // Remove trailing ```
        }

        // Clean the JSON string by removing excess whitespace and newlines
        content = Regex.Replace(content, @"\s+", " ").Trim();

        return content;
    }
}
