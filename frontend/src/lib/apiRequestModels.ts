export type AddApiKeyRequestModel = {
  key: string;
};

export type GetIngredientCategoriesRequestModel = {
  skip?: number;
  take?: number;
};

export type AddIngredientCategoryRequestModel = {
  name: string;
};

export type UpdateIngredientCategoryRequestModel = {
  id: number;
  name: string;
};

export type GetIngredientsRequestModel = {
  name?: string;
  categoryId?: number;
  skip?: number;
  take?: number;
};

export type AddIngredientRequestModel = {
  name: string;
  categoryId: number;
  isVisibleOnCard: boolean;
  imagePath: string;
};

export type UpdateIngredientRequestModel = {
  id: number;
  name: string;
  categoryId: number;
  isVisibleOnCard: boolean;
  imagePath: string;
};

export type GetRecipesRequestModel = {
  name?: string;
  dishType?: string;
  maxCookingTime?: number;
  skip?: number;
  take?: number;
};

export type AddRecipeRequestModel = {
  name: string;
  description: string;
  dishType: string;
  cookingTimeFrom: number;
  cookingTimeTo: number;
  steps: string[];
};

export type UpdateRecipeRequestModel = {
  id: number;
  name: string;
  description: string;
  dishType: string;
  cookingTimeFrom: number;
  cookingTimeTo: number;
  steps: string[];
};

export type GenerateRecipeRequestModel = {
  ingredients: string[];
};
