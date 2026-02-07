export type PaginatedResponseModel<T> = {
  items: T[];
  total: number;
};

export type GetLatestApiKeyResponseModel = {
  key: string;
};

export type GetIngredientCategoryResponseModel = {
  id: number;
  name: string;
  createdOn: string;
  updatedOn: string;
};

export type GetIngredientResponseModel = {
  id: number;
  name: string;
  categoryId: number;
  isVisibleOnCard: boolean;
  imagePath: string;
  createdOn: string;
  updatedOn: string;
};

export type IngredientByGroupResponseModel = {
  name: string;
  isVisibleOnCard: boolean;
  imagePath: string;
};

export type GetAllIngredientsResponseModel = {
  category: string;
  ingredients: IngredientByGroupResponseModel[];
};

export type GetRecipeResponseModel = {
  id: number;
  name: string;
  description: string;
  dishType: string;
  cookingTimeFrom: number;
  cookingTimeTo: number;
  steps: string[];
  createdOn: string;
  updatedOn: string;
};

export type CookingTimeResponseModel = {
  from: number;
  to: number;
};

export type RecipeContentResponseModel = {
  name: string;
  description: string;
  dishType: string;
  steps: string[];
  cookingTime: CookingTimeResponseModel;
};

export type GeneratedRecipeResponseModel = {
  recipeFound: boolean;
  message: string;
  recipe: RecipeContentResponseModel | null;
};
