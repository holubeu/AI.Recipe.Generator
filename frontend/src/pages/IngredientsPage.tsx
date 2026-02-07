import { useEffect, useMemo, useState } from "react";
import {
  addIngredient,
  deleteIngredient,
  getIngredientCategories,
  getIngredients,
  updateIngredient,
} from "../lib/apiClient";
import IngredientForm, { type IngredientFormValues } from "../components/IngredientForm";
import type {
  GetIngredientCategoryResponseModel,
  GetIngredientResponseModel,
} from "../lib/apiResponseModels";

const pageSizeOptions = [25, 50, 100];

export default function IngredientsPage() {
  const [ingredients, setIngredients] = useState<GetIngredientResponseModel[]>([]);
  const [categories, setCategories] = useState<GetIngredientCategoryResponseModel[]>([]);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [pageSize, setPageSize] = useState(25);
  const [nameFilter, setNameFilter] = useState("");
  const [categoryFilter, setCategoryFilter] = useState<string>("");
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [isFormOpen, setIsFormOpen] = useState(false);
  const [editingIngredient, setEditingIngredient] = useState<GetIngredientResponseModel | null>(
    null,
  );
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [isDeletingId, setIsDeletingId] = useState<number | null>(null);

  const totalPages = useMemo(() => Math.max(1, Math.ceil(total / pageSize)), [total, pageSize]);

  const categoryMap = useMemo(() => {
    const map = new Map<number, string>();
    categories.forEach((category) => map.set(category.id, category.name));
    return map;
  }, [categories]);

  const loadIngredients = () => {
    setIsLoading(true);
    setError(null);

    return getIngredients({
      name: nameFilter.trim() || undefined,
      categoryId: categoryFilter ? Number(categoryFilter) : undefined,
      skip: (page - 1) * pageSize,
      take: pageSize,
    })
      .then((result) => {
        setIngredients(result.items);
        setTotal(result.total);
      })
      .catch((fetchError) => {
        setError(fetchError instanceof Error ? fetchError.message : "Failed to load ingredients.");
      })
      .finally(() => {
        setIsLoading(false);
      });
  };

  useEffect(() => {
    getIngredientCategories({ skip: 0, take: 200 })
      .then((result) => setCategories(result.items))
      .catch((fetchError) => {
        setError(fetchError instanceof Error ? fetchError.message : "Failed to load categories.");
      });
  }, []);

  useEffect(() => {
    loadIngredients();
  }, [page, pageSize, nameFilter, categoryFilter]);

  const openAddForm = () => {
    setEditingIngredient(null);
    setIsFormOpen(true);
  };

  const openEditForm = (ingredient: GetIngredientResponseModel) => {
    setEditingIngredient(ingredient);
    setIsFormOpen(true);
  };

  const closeForm = () => {
    setEditingIngredient(null);
    setIsFormOpen(false);
  };

  const handleSubmit = async (values: IngredientFormValues) => {
    setIsSubmitting(true);
    setError(null);

    try {
      if (editingIngredient) {
        await updateIngredient({
          id: editingIngredient.id,
          name: values.name,
          categoryId: Number(values.categoryId),
          isVisibleOnCard: values.isVisibleOnCard,
          imagePath: values.imagePath,
        });
      } else {
        await addIngredient({
          name: values.name,
          categoryId: Number(values.categoryId),
          isVisibleOnCard: values.isVisibleOnCard,
          imagePath: values.imagePath,
        });
      }

      closeForm();
      await loadIngredients();
    } catch (submitError) {
      setError(submitError instanceof Error ? submitError.message : "Failed to save ingredient.");
    } finally {
      setIsSubmitting(false);
    }
  };

  const handleDelete = async (ingredientId: number) => {
    setIsDeletingId(ingredientId);
    setError(null);

    try {
      await deleteIngredient(ingredientId);
      setIngredients((prev) => {
        const next = prev.filter((item) => item.id !== ingredientId);
        if (next.length === 0 && page > 1) {
          setPage(page - 1);
        }
        return next;
      });
      setTotal((prev) => Math.max(0, prev - 1));
    } catch (deleteError) {
      setError(deleteError instanceof Error ? deleteError.message : "Failed to delete ingredient.");
    } finally {
      setIsDeletingId(null);
    }
  };

  const initialFormValues: IngredientFormValues | undefined = editingIngredient
    ? {
        name: editingIngredient.name,
        categoryId: String(editingIngredient.categoryId),
        isVisibleOnCard: editingIngredient.isVisibleOnCard,
        imagePath: editingIngredient.imagePath ?? "",
      }
    : undefined;

  return (
    <section className="card">
      <h2>Ingredients</h2>
      <p className="muted">Manage ingredients in the catalog.</p>

      <div className="filters-row">
        <div className="field-row">
          <label className="field-label" htmlFor="ingredient-name">
            Ingredient name
          </label>
          <input
            id="ingredient-name"
            className="text-input"
            value={nameFilter}
            onChange={(event) => {
              setNameFilter(event.target.value);
              setPage(1);
            }}
            placeholder="Filter by name"
          />
        </div>

        <div className="field-row">
          <label className="field-label" htmlFor="ingredient-category-filter">
            Category
          </label>
          <select
            id="ingredient-category-filter"
            className="select-input"
            value={categoryFilter}
            onChange={(event) => {
              setCategoryFilter(event.target.value);
              setPage(1);
            }}
          >
            <option value="">All categories</option>
            {categories.map((category) => (
              <option key={category.id} value={String(category.id)}>
                {category.name}
              </option>
            ))}
          </select>
        </div>

        <div className="field-row">
          <label className="field-label" htmlFor="ingredient-page-size">
            Items per page
          </label>
          <select
            id="ingredient-page-size"
            className="select-input"
            value={String(pageSize)}
            onChange={(event) => {
              setPageSize(Number(event.target.value));
              setPage(1);
            }}
          >
            {pageSizeOptions.map((size) => (
              <option key={size} value={String(size)}>
                {size}
              </option>
            ))}
          </select>
        </div>
      </div>

      <div className="table-actions">
        <button className="button" onClick={openAddForm}>
          Add ingredient
        </button>
      </div>

      {isFormOpen && (
        <IngredientForm
          categories={categories}
          initialValues={initialFormValues}
          onSubmit={handleSubmit}
          onCancel={closeForm}
          isSubmitting={isSubmitting}
        />
      )}

      {isLoading && <p className="muted">Loading ingredients...</p>}
      {error && <p className="warning">{error}</p>}

      {!isLoading && !error && ingredients.length === 0 && (
        <p className="muted">No ingredients found.</p>
      )}

      {ingredients.length > 0 && (
        <div className="table-wrapper">
          <table className="data-table">
            <thead>
              <tr>
                <th>Name</th>
                <th>Category</th>
                <th>Visible on card</th>
                <th>Image path</th>
                <th>Created</th>
                <th>Updated</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {ingredients.map((ingredient) => (
                <tr key={ingredient.id}>
                  <td>{ingredient.name}</td>
                  <td>{categoryMap.get(ingredient.categoryId) ?? "Unknown"}</td>
                  <td>{ingredient.isVisibleOnCard ? "Yes" : "No"}</td>
                  <td>{ingredient.imagePath || "-"}</td>
                  <td>{new Date(ingredient.createdOn).toLocaleDateString()}</td>
                  <td>{new Date(ingredient.updatedOn).toLocaleDateString()}</td>
                  <td>
                    <div className="table-actions">
                      <button className="button" onClick={() => openEditForm(ingredient)}>
                        Edit
                      </button>
                      <button
                        className="button secondary"
                        onClick={() => handleDelete(ingredient.id)}
                        disabled={isDeletingId === ingredient.id}
                      >
                        {isDeletingId === ingredient.id ? "Deleting..." : "Delete"}
                      </button>
                    </div>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}

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
