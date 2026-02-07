import { useEffect, useMemo, useState } from "react";
import { addRecipe, generateRecipe, getAllIngredientsGrouped } from "../lib/apiClient";
import type {
  GeneratedRecipeResponseModel,
  GetAllIngredientsResponseModel,
  RecipeContentResponseModel,
} from "../lib/apiResponseModels";

type SelectedIngredient = {
  name: string;
};

type EditableRecipeFields = {
  name: string;
  description: string;
  dishType: string;
};

function normalizeIngredientName(value: string): string {
  return value.trim().toLowerCase();
}

function mapRecipeToEditableFields(recipe: RecipeContentResponseModel): EditableRecipeFields {
  return {
    name: recipe.name,
    description: recipe.description,
    dishType: recipe.dishType,
  };
}

function hasValue(value: string | null | undefined): boolean {
  return Boolean(value && value.trim());
}

export default function GenerateRecipePage() {
  const [ingredientGroups, setIngredientGroups] = useState<GetAllIngredientsResponseModel[]>([]);
  const [isLoadingIngredients, setIsLoadingIngredients] = useState(true);
  const [loadError, setLoadError] = useState<string | null>(null);
  const [inputValue, setInputValue] = useState("");
  const [selectedIngredients, setSelectedIngredients] = useState<SelectedIngredient[]>([]);
  const [isGenerating, setIsGenerating] = useState(false);
  const [recipeResponse, setRecipeResponse] = useState<GeneratedRecipeResponseModel | null>(null);
  const [recipeFields, setRecipeFields] = useState<EditableRecipeFields | null>(null);
  const [saveStatus, setSaveStatus] = useState<string | null>(null);
  const [saveError, setSaveError] = useState<string | null>(null);

  useEffect(() => {
    let isMounted = true;
    setIsLoadingIngredients(true);
    setLoadError(null);

    getAllIngredientsGrouped()
      .then((data) => {
        if (isMounted) {
          setIngredientGroups(data);
        }
      })
      .catch((error) => {
        if (isMounted) {
          setLoadError(error instanceof Error ? error.message : "Failed to load ingredients.");
        }
      })
      .finally(() => {
        if (isMounted) {
          setIsLoadingIngredients(false);
        }
      });

    return () => {
      isMounted = false;
    };
  }, []);

  const allIngredients = useMemo(
    () =>
      ingredientGroups.flatMap((group) =>
        group.ingredients.map((ingredient) => ingredient.name),
      ),
    [ingredientGroups],
  );

  const suggestionItems = useMemo(() => {
    const searchValue = normalizeIngredientName(inputValue);
    if (!searchValue) {
      return [];
    }

    const existingNames = new Set(
      selectedIngredients.map((ingredient) => normalizeIngredientName(ingredient.name)),
    );

    return allIngredients
      .filter((name) => normalizeIngredientName(name).includes(searchValue))
      .filter((name) => !existingNames.has(normalizeIngredientName(name)))
      .slice(0, 6);
  }, [allIngredients, inputValue, selectedIngredients]);

  const canGenerate = selectedIngredients.length > 0 && !isGenerating;

  const handleAddIngredient = (value: string) => {
    const cleaned = value.trim();
    if (!cleaned) {
      return;
    }

    const normalized = normalizeIngredientName(cleaned);
    const exists = selectedIngredients.some(
      (ingredient) => normalizeIngredientName(ingredient.name) === normalized,
    );

    if (exists) {
      setInputValue("");
      return;
    }

    setSelectedIngredients((prev) => [...prev, { name: cleaned }]);
    setInputValue("");
  };

  const handleRemoveIngredient = (name: string) => {
    setSelectedIngredients((prev) =>
      prev.filter((ingredient) => normalizeIngredientName(ingredient.name) !== normalizeIngredientName(name)),
    );
  };

  const handleGenerate = async () => {
    if (!canGenerate) {
      return;
    }

    setIsGenerating(true);
    setRecipeResponse(null);
    setRecipeFields(null);
    setSaveStatus(null);
    setSaveError(null);

    try {
      const response = await generateRecipe({
        ingredients: selectedIngredients.map((ingredient) => ingredient.name),
      });

      setRecipeResponse(response);
      if (response.recipeFound && response.recipe) {
        setRecipeFields(mapRecipeToEditableFields(response.recipe));
      }
    } catch (error) {
      setRecipeResponse({
        recipeFound: false,
        message: error instanceof Error ? error.message : "Recipe generation failed.",
        recipe: null,
      });
    } finally {
      setIsGenerating(false);
    }
  };

  const handleSave = async () => {
    if (!recipeResponse?.recipeFound || !recipeResponse.recipe || !recipeFields) {
      return;
    }

    setSaveStatus(null);
    setSaveError(null);

    try {
      await addRecipe({
        name: recipeFields.name,
        description: recipeFields.description,
        dishType: recipeFields.dishType,
        cookingTimeFrom: recipeResponse.recipe.cookingTime.from,
        cookingTimeTo: recipeResponse.recipe.cookingTime.to,
        steps: recipeResponse.recipe.steps,
      });
      setSaveStatus("Recipe saved.");
    } catch (error) {
      setSaveError(error instanceof Error ? error.message : "Failed to save recipe.");
    }
  };

  return (
    <section className="card">
      <h2>Generate Recipe</h2>
      <p className="muted">Start a new recipe from selected ingredients.</p>

      <div className="generation-layout">
        <div>
          <h3>Pick ingredients</h3>

          <div className="ingredient-input">
            <div className="input-group">
              <input
                className="text-input"
                placeholder="Add ingredient"
                value={inputValue}
                onChange={(event) => setInputValue(event.target.value)}
                onKeyDown={(event) => {
                  if (event.key === "Enter") {
                    event.preventDefault();
                    handleAddIngredient(inputValue);
                  }
                }}
              />
              <button className="button" onClick={() => handleAddIngredient(inputValue)}>
                Add
              </button>
            </div>
            {suggestionItems.length > 0 && (
              <div className="suggestions">
                {suggestionItems.map((name) => (
                  <button
                    key={name}
                    className="suggestion-item"
                    onClick={() => handleAddIngredient(name)}
                  >
                    {name}
                  </button>
                ))}
              </div>
            )}
          </div>

          {isLoadingIngredients && <p className="muted">Loading ingredients...</p>}
          {loadError && <p className="warning">{loadError}</p>}

          {!isLoadingIngredients && !loadError && (
            <div className="ingredient-groups">
              {ingredientGroups.map((group) => {
                const visibleIngredients = group.ingredients.filter(
                  (ingredient) => ingredient.isVisibleOnCard,
                );

                if (visibleIngredients.length === 0) {
                  return null;
                }

                return (
                  <div key={group.category} className="ingredient-group">
                    <h4>{group.category}</h4>
                    <div className="ingredient-grid">
                      {visibleIngredients.map((ingredient) => (
                        <button
                          key={`${group.category}-${ingredient.name}`}
                          className="ingredient-card"
                          onClick={() => handleAddIngredient(ingredient.name)}
                        >
                          {ingredient.imagePath && (
                            <img
                              className="ingredient-image"
                              src={ingredient.imagePath}
                              alt={ingredient.name}
                            />
                          )}
                          <span>{ingredient.name}</span>
                        </button>
                      ))}
                    </div>
                  </div>
                );
              })}
            </div>
          )}
        </div>

        <div>
          <h3>Selected ingredients</h3>
          <div className="selected-list">
            {selectedIngredients.length === 0 && (
              <p className="muted">No ingredients selected yet.</p>
            )}
            {selectedIngredients.map((ingredient) => (
              <div key={ingredient.name} className="selected-card">
                <span>{ingredient.name}</span>
                <button
                  className="remove-button"
                  onClick={() => handleRemoveIngredient(ingredient.name)}
                  aria-label={`Remove ${ingredient.name}`}
                >
                  ×
                </button>
              </div>
            ))}
          </div>

          <button className="button" disabled={!canGenerate} onClick={handleGenerate}>
            {isGenerating ? "Generating..." : "Generate"}
          </button>

          {recipeResponse && (
            <div className="recipe-response">
              {!recipeResponse.recipeFound && (
                <p className="warning">{recipeResponse.message}</p>
              )}

              {recipeResponse.recipeFound && recipeResponse.recipe && recipeFields && (
                <div className="recipe-details">
                  {hasValue(recipeFields.name) && (
                    <div className="field-row">
                      <label className="field-label" htmlFor="recipe-name">
                        Recipe Name
                      </label>
                      <input
                        id="recipe-name"
                        className="text-input"
                        value={recipeFields.name}
                        onChange={(event) =>
                          setRecipeFields((prev) =>
                            prev ? { ...prev, name: event.target.value } : prev,
                          )
                        }
                      />
                    </div>
                  )}

                  {hasValue(recipeFields.description) && (
                    <div className="field-row">
                      <label className="field-label" htmlFor="recipe-description">
                        Description
                      </label>
                      <textarea
                        id="recipe-description"
                        className="text-area"
                        value={recipeFields.description}
                        onChange={(event) =>
                          setRecipeFields((prev) =>
                            prev ? { ...prev, description: event.target.value } : prev,
                          )
                        }
                      />
                    </div>
                  )}

                  {hasValue(recipeFields.dishType) && (
                    <div className="field-row">
                      <label className="field-label" htmlFor="recipe-type">
                        Dish Type
                      </label>
                      <input
                        id="recipe-type"
                        className="text-input"
                        value={recipeFields.dishType}
                        onChange={(event) =>
                          setRecipeFields((prev) =>
                            prev ? { ...prev, dishType: event.target.value } : prev,
                          )
                        }
                      />
                    </div>
                  )}

                  <div className="field-row">
                    <span className="field-label">Cooking Time</span>
                    <span>
                      {recipeResponse.recipe.cookingTime.from} - {recipeResponse.recipe.cookingTime.to} minutes
                    </span>
                  </div>

                  <div className="field-row">
                    <span className="field-label">Cooking Instructions</span>
                    <ul className="steps-list">
                      {recipeResponse.recipe.steps.map((step, index) => (
                        <li key={`${step}-${index}`}>{step}</li>
                      ))}
                    </ul>
                  </div>

                  <div className="actions-row">
                    <button className="button" onClick={handleSave}>
                      Save
                    </button>
                    {saveStatus && <span className="success">{saveStatus}</span>}
                    {saveError && <span className="warning">{saveError}</span>}
                  </div>
                </div>
              )}
            </div>
          )}
        </div>
      </div>
    </section>
  );
}
