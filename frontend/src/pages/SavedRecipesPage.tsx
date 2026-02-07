import { useEffect, useMemo, useState } from "react";
import { deleteRecipe, getRecipes, updateRecipe } from "../lib/apiClient";
import RecipeEditForm from "../components/RecipeEditForm.tsx";
import type { GetRecipeResponseModel } from "../lib/apiResponseModels";

const PAGE_SIZE = 25;

const cookingTimeOptions = [
  { label: "doesn't matter", value: "" },
  { label: "15", value: "15" },
  { label: "30", value: "30" },
  { label: "45", value: "45" },
  { label: "60", value: "60" },
  { label: "90", value: "90" },
  { label: "120", value: "120" },
];

type EditableRecipeFields = {
  name: string;
  description: string;
  dishType: string;
};

export default function SavedRecipesPage() {
  const [recipes, setRecipes] = useState<GetRecipeResponseModel[]>([]);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [searchTerm, setSearchTerm] = useState("");
  const [maxCookingTime, setMaxCookingTime] = useState<number | null>(null);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [editingId, setEditingId] = useState<number | null>(null);
  const [editingFields, setEditingFields] = useState<EditableRecipeFields | null>(null);
  const [isSaving, setIsSaving] = useState(false);
  const [isDeletingId, setIsDeletingId] = useState<number | null>(null);

  const totalPages = useMemo(() => Math.max(1, Math.ceil(total / PAGE_SIZE)), [total]);

  useEffect(() => {
    setIsLoading(true);
    setError(null);

    getRecipes({
      name: searchTerm.trim() || undefined,
      maxCookingTime: maxCookingTime ?? undefined,
      skip: (page - 1) * PAGE_SIZE,
      take: PAGE_SIZE,
    })
      .then((result) => {
        setRecipes(result.items);
        setTotal(result.total);
      })
      .catch((fetchError) => {
        setError(fetchError instanceof Error ? fetchError.message : "Failed to load recipes.");
      })
      .finally(() => {
        setIsLoading(false);
      });
  }, [page, searchTerm, maxCookingTime]);

  const handleEdit = (recipe: GetRecipeResponseModel) => {
    setEditingId(recipe.id);
    setEditingFields({
      name: recipe.name,
      description: recipe.description,
      dishType: recipe.dishType,
    });
  };

  const handleCancel = () => {
    setEditingId(null);
    setEditingFields(null);
  };

  const handleSave = async (recipe: GetRecipeResponseModel) => {
    if (!editingFields) {
      return;
    }

    setIsSaving(true);

    try {
      await updateRecipe({
        id: recipe.id,
        name: editingFields.name,
        description: editingFields.description,
        dishType: editingFields.dishType,
        cookingTimeFrom: recipe.cookingTimeFrom,
        cookingTimeTo: recipe.cookingTimeTo,
        steps: recipe.steps,
      });

      setRecipes((prev) =>
        prev.map((item) =>
          item.id === recipe.id
            ? {
                ...item,
                name: editingFields.name,
                description: editingFields.description,
                dishType: editingFields.dishType,
              }
            : item,
        ),
      );
      handleCancel();
    } catch (saveError) {
      setError(saveError instanceof Error ? saveError.message : "Failed to update recipe.");
    } finally {
      setIsSaving(false);
    }
  };

  const handleDelete = async (recipeId: number) => {
    setIsDeletingId(recipeId);
    setError(null);

    try {
      await deleteRecipe(recipeId);
      setRecipes((prev) => {
        const next = prev.filter((item) => item.id !== recipeId);
        if (next.length === 0 && page > 1) {
          setPage(page - 1);
        }
        return next;
      });
      setTotal((prev) => Math.max(0, prev - 1));
    } catch (deleteError) {
      setError(deleteError instanceof Error ? deleteError.message : "Failed to delete recipe.");
    } finally {
      setIsDeletingId(null);
    }
  };

  return (
    <section className="card">
      <h2>Saved Recipes</h2>
      <p className="muted">Browse recipes you have saved.</p>

      <div className="filters-row">
        <div className="field-row">
          <label className="field-label" htmlFor="recipe-search">
            Search by name
          </label>
          <input
            id="recipe-search"
            className="text-input"
            placeholder="Search recipes"
            value={searchTerm}
            onChange={(event) => {
              setSearchTerm(event.target.value);
              setPage(1);
            }}
          />
        </div>

        <div className="field-row">
          <label className="field-label" htmlFor="max-cooking-time">
            Max cooking time, in minutes
          </label>
          <select
            id="max-cooking-time"
            className="select-input"
            value={maxCookingTime === null ? "" : String(maxCookingTime)}
            onChange={(event) => {
              const value = event.target.value;
              setMaxCookingTime(value ? Number(value) : null);
              setPage(1);
            }}
          >
            {cookingTimeOptions.map((option) => (
              <option key={`${option.label}-${option.value}`} value={option.value}>
                {option.label}
              </option>
            ))}
          </select>
        </div>
      </div>

      {isLoading && <p className="muted">Loading recipes...</p>}
      {error && <p className="warning">{error}</p>}

      {!isLoading && !error && recipes.length === 0 && (
        <p className="muted">No recipes found.</p>
      )}

      <div className="recipe-list">
        {recipes.map((recipe) => {
          const isEditing = editingId === recipe.id;

          return (
            <article key={recipe.id} className="recipe-item">
              {isEditing && editingFields ? (
                  <RecipeEditForm
                    recipe={recipe}
                    fields={editingFields}
                    onChange={(fields: EditableRecipeFields) => setEditingFields(fields)}
                    onSave={() => handleSave(recipe)}
                    onCancel={handleCancel}
                    isSaving={isSaving}
                  />
              ) : (
                <>
                  <div className="recipe-header">
                    <div>
                      <h3>{recipe.name}</h3>
                      <p className="muted">{recipe.dishType}</p>
                    </div>
                    <div className="recipe-actions">
                      <button className="button" onClick={() => handleEdit(recipe)}>
                        Edit
                      </button>
                      <button
                        className="button secondary"
                        onClick={() => handleDelete(recipe.id)}
                        disabled={isDeletingId === recipe.id}
                      >
                        {isDeletingId === recipe.id ? "Deleting..." : "Delete"}
                      </button>
                    </div>
                  </div>
                  <p>{recipe.description}</p>
                  <p className="muted">
                    Cooking time: {recipe.cookingTimeFrom} - {recipe.cookingTimeTo} minutes
                  </p>
                  <div className="field-row">
                    <span className="field-label">Cooking Instructions</span>
                    <ul className="steps-list">
                      {recipe.steps.map((step, index) => (
                        <li key={`${step}-${index}`}>{step}</li>
                      ))}
                    </ul>
                  </div>
                </>
              )}
            </article>
          );
        })}
      </div>

      <div className="pagination">
        <button
          className="button secondary"
          onClick={() => setPage((prev) => Math.max(1, prev - 1))}
          disabled={page === 1}
        >
          Previous
        </button>
        <span className="muted">
          Page {page} of {totalPages}
        </span>
        <button
          className="button secondary"
          onClick={() => setPage((prev) => Math.min(totalPages, prev + 1))}
          disabled={page >= totalPages}
        >
          Next
        </button>
      </div>
    </section>
  );
}
