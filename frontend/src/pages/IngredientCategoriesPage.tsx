import { useEffect, useMemo, useState } from "react";
import {
  addIngredientCategory,
  getIngredientCategories,
  updateIngredientCategory,
} from "../lib/apiClient";
import IngredientCategoryForm, {
  type IngredientCategoryFormValues,
} from "../components/IngredientCategoryForm";
import type { GetIngredientCategoryResponseModel } from "../lib/apiResponseModels";

const pageSizeOptions = [25, 50, 100];

export default function IngredientCategoriesPage() {
  const [categories, setCategories] = useState<GetIngredientCategoryResponseModel[]>([]);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [pageSize, setPageSize] = useState(25);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [isFormOpen, setIsFormOpen] = useState(false);
  const [editingCategory, setEditingCategory] =
    useState<GetIngredientCategoryResponseModel | null>(null);
  const [isSubmitting, setIsSubmitting] = useState(false);

  const totalPages = useMemo(() => Math.max(1, Math.ceil(total / pageSize)), [total, pageSize]);

  const loadCategories = () => {
    setIsLoading(true);
    setError(null);

    return getIngredientCategories({
      skip: (page - 1) * pageSize,
      take: pageSize,
    })
      .then((result) => {
        setCategories(result.items);
        setTotal(result.total);
      })
      .catch((fetchError) => {
        setError(
          fetchError instanceof Error ? fetchError.message : "Failed to load categories.",
        );
      })
      .finally(() => {
        setIsLoading(false);
      });
  };

  useEffect(() => {
    loadCategories();
  }, [page, pageSize]);

  const openAddForm = () => {
    setEditingCategory(null);
    setIsFormOpen(true);
  };

  const openEditForm = (category: GetIngredientCategoryResponseModel) => {
    setEditingCategory(category);
    setIsFormOpen(true);
  };

  const closeForm = () => {
    setEditingCategory(null);
    setIsFormOpen(false);
  };

  const handleSubmit = async (values: IngredientCategoryFormValues) => {
    setIsSubmitting(true);
    setError(null);

    try {
      if (editingCategory) {
        await updateIngredientCategory({ id: editingCategory.id, name: values.name });
      } else {
        await addIngredientCategory({ name: values.name });
      }

      closeForm();
      await loadCategories();
    } catch (submitError) {
      setError(
        submitError instanceof Error ? submitError.message : "Failed to save category.",
      );
    } finally {
      setIsSubmitting(false);
    }
  };

  const initialFormValues: IngredientCategoryFormValues | undefined = editingCategory
    ? { name: editingCategory.name }
    : undefined;

  return (
    <section className="card">
      <h2>Ingredient Categories</h2>
      <p className="muted">Edit and organize ingredient categories.</p>

      <div className="filters-row">
        <div className="field-row">
          <label className="field-label" htmlFor="category-page-size">
            Items per page
          </label>
          <select
            id="category-page-size"
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
          Add category
        </button>
      </div>

      {isFormOpen && (
        <IngredientCategoryForm
          initialValues={initialFormValues}
          onSubmit={handleSubmit}
          onCancel={closeForm}
          isSubmitting={isSubmitting}
        />
      )}

      {isLoading && <p className="muted">Loading categories...</p>}
      {error && <p className="warning">{error}</p>}

      {!isLoading && !error && categories.length === 0 && (
        <p className="muted">No categories found.</p>
      )}

      {categories.length > 0 && (
        <div className="table-wrapper">
          <table className="data-table">
            <thead>
              <tr>
                <th>Name</th>
                <th>Created</th>
                <th>Updated</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {categories.map((category) => (
                <tr key={category.id}>
                  <td>{category.name}</td>
                  <td>{new Date(category.createdOn).toLocaleDateString()}</td>
                  <td>{new Date(category.updatedOn).toLocaleDateString()}</td>
                  <td>
                    <div className="table-actions">
                      <button className="button" onClick={() => openEditForm(category)}>
                        Edit
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
