import { getRole } from "./roleStorage";
import type {
  AddApiKeyRequestModel,
  AddIngredientCategoryRequestModel,
  AddIngredientRequestModel,
  AddRecipeRequestModel,
  GenerateRecipeRequestModel,
  GetIngredientCategoriesRequestModel,
  GetIngredientsRequestModel,
  GetRecipesRequestModel,
  UpdateIngredientCategoryRequestModel,
  UpdateIngredientRequestModel,
  UpdateRecipeRequestModel,
} from "./apiRequestModels";
import type {
  GeneratedRecipeResponseModel,
  GetAllIngredientsResponseModel,
  GetIngredientCategoryResponseModel,
  GetIngredientResponseModel,
  GetLatestApiKeyResponseModel,
  GetRecipeResponseModel,
  PaginatedResponseModel,
} from "./apiResponseModels";

const API_BASE_URL = "https://localhost:7292";

export const apiBaseUrl = API_BASE_URL;

type RequestOptions = {
  method?: "GET" | "POST" | "PUT" | "DELETE";
  body?: unknown;
};

function buildHeaders(hasBody: boolean): HeadersInit {
  const role = getRole();
  const headers: HeadersInit = role ? { "X-User-Role": role } : {};
  if (hasBody) {
    headers["Content-Type"] = "application/json";
  }

  return headers;
}

function toQueryString(params: Record<string, string | number | boolean | null | undefined>): string {
  const search = new URLSearchParams();
  Object.entries(params).forEach(([key, value]) => {
    if (value === null || value === undefined) {
      return;
    }

    if (typeof value === "string" && value.trim() === "") {
      return;
    }

    search.set(key, String(value));
  });

  const query = search.toString();
  return query ? `?${query}` : "";
}

async function requestJson<T>(path: string, options: RequestOptions = {}): Promise<T> {
  const hasBody = options.body !== undefined;
  const response = await fetch(`${API_BASE_URL}${path}`, {
    method: options.method ?? "GET",
    headers: buildHeaders(hasBody),
    body: hasBody ? JSON.stringify(options.body) : undefined,
  });

  if (!response.ok) {
    throw new Error(`Request failed: ${response.status}`);
  }

  return response.json() as Promise<T>;
}

async function requestNoContent(path: string, options: RequestOptions = {}): Promise<void> {
  const hasBody = options.body !== undefined;
  const response = await fetch(`${API_BASE_URL}${path}`, {
    method: options.method ?? "POST",
    headers: buildHeaders(hasBody),
    body: hasBody ? JSON.stringify(options.body) : undefined,
  });

  if (!response.ok) {
    throw new Error(`Request failed: ${response.status}`);
  }
}

export async function getJson<T>(path: string): Promise<T> {
  return requestJson<T>(path);
}

export async function getLatestApiKey(): Promise<GetLatestApiKeyResponseModel> {
  return requestJson<GetLatestApiKeyResponseModel>("/api/ApiKeys/latest");
}

export async function addApiKey(requestModel: AddApiKeyRequestModel): Promise<void> {
  return requestNoContent("/api/ApiKeys", { method: "POST", body: requestModel });
}

export async function getIngredientCategories(
  requestModel: GetIngredientCategoriesRequestModel = {},
): Promise<PaginatedResponseModel<GetIngredientCategoryResponseModel>> {
  const query = toQueryString(requestModel);
  return requestJson<PaginatedResponseModel<GetIngredientCategoryResponseModel>>(
    `/api/IngredientCategories${query}`,
  );
}

export async function addIngredientCategory(
  requestModel: AddIngredientCategoryRequestModel,
): Promise<void> {
  return requestNoContent("/api/IngredientCategories", { method: "POST", body: requestModel });
}

export async function updateIngredientCategory(
  requestModel: UpdateIngredientCategoryRequestModel,
): Promise<void> {
  return requestNoContent("/api/IngredientCategories", { method: "PUT", body: requestModel });
}

export async function getIngredients(
  requestModel: GetIngredientsRequestModel = {},
): Promise<PaginatedResponseModel<GetIngredientResponseModel>> {
  const query = toQueryString(requestModel);
  return requestJson<PaginatedResponseModel<GetIngredientResponseModel>>(
    `/api/Ingredients${query}`,
  );
}

export async function getAllIngredientsGrouped(): Promise<GetAllIngredientsResponseModel[]> {
  return requestJson<GetAllIngredientsResponseModel[]>("/api/Ingredients/grouped");
}

export async function addIngredient(requestModel: AddIngredientRequestModel): Promise<void> {
  return requestNoContent("/api/Ingredients", { method: "POST", body: requestModel });
}

export async function updateIngredient(requestModel: UpdateIngredientRequestModel): Promise<void> {
  return requestNoContent("/api/Ingredients", { method: "PUT", body: requestModel });
}

export async function deleteIngredient(id: number): Promise<void> {
  return requestNoContent(`/api/Ingredients/${id}`, { method: "DELETE" });
}

export async function getRecipes(
  requestModel: GetRecipesRequestModel = {},
): Promise<PaginatedResponseModel<GetRecipeResponseModel>> {
  const query = toQueryString(requestModel);
  return requestJson<PaginatedResponseModel<GetRecipeResponseModel>>(`/api/Recipes${query}`);
}

export async function addRecipe(requestModel: AddRecipeRequestModel): Promise<void> {
  return requestNoContent("/api/Recipes", { method: "POST", body: requestModel });
}

export async function updateRecipe(requestModel: UpdateRecipeRequestModel): Promise<void> {
  return requestNoContent("/api/Recipes", { method: "PUT", body: requestModel });
}

export async function deleteRecipe(id: number): Promise<void> {
  return requestNoContent(`/api/Recipes/${id}`, { method: "DELETE" });
}

export async function generateRecipe(
  requestModel: GenerateRecipeRequestModel,
): Promise<GeneratedRecipeResponseModel> {
  return requestJson<GeneratedRecipeResponseModel>("/api/Recipes/generate", {
    method: "POST",
    body: requestModel,
  });
}
