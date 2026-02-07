import { useEffect, useState } from "react";
import type { GetIngredientCategoryResponseModel } from "../lib/apiResponseModels";

export type IngredientFormValues = {
  name: string;
  categoryId: string;
  isVisibleOnCard: boolean;
  imagePath: string;
};

type IngredientFormProps = {
  categories: GetIngredientCategoryResponseModel[];
  initialValues?: IngredientFormValues;
  onSubmit: (values: IngredientFormValues) => void;
  onCancel: () => void;
  isSubmitting: boolean;
};

const defaultValues: IngredientFormValues = {
  name: "",
  categoryId: "",
  isVisibleOnCard: false,
  imagePath: "",
};

export default function IngredientForm({
  categories,
  initialValues,
  onSubmit,
  onCancel,
  isSubmitting,
}: IngredientFormProps) {
  const [values, setValues] = useState<IngredientFormValues>(
    initialValues ?? defaultValues,
  );
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    setValues(initialValues ?? defaultValues);
    setError(null);
  }, [initialValues]);

  const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    if (!values.name.trim()) {
      setError("Name is required.");
      return;
    }

    if (!values.categoryId) {
      setError("Category is required.");
      return;
    }

    setError(null);
    onSubmit({
      ...values,
      name: values.name.trim(),
      imagePath: values.imagePath.trim(),
    });
  };

  return (
    <form className="form-card" onSubmit={handleSubmit}>
      <div className="field-row">
        <label className="field-label" htmlFor="ingredient-name">
          Name
        </label>
        <input
          id="ingredient-name"
          className="text-input"
          value={values.name}
          onChange={(event) => setValues((prev) => ({ ...prev, name: event.target.value }))}
        />
      </div>

      <div className="field-row">
        <label className="field-label" htmlFor="ingredient-category">
          Category
        </label>
        <select
          id="ingredient-category"
          className="select-input"
          value={values.categoryId}
          onChange={(event) =>
            setValues((prev) => ({ ...prev, categoryId: event.target.value }))
          }
        >
          <option value="">Select a category</option>
          {categories.map((category) => (
            <option key={category.id} value={String(category.id)}>
              {category.name}
            </option>
          ))}
        </select>
      </div>

      <div className="field-row">
        <label className="field-label" htmlFor="ingredient-image">
          Image path
        </label>
        <input
          id="ingredient-image"
          className="text-input"
          value={values.imagePath}
          onChange={(event) => setValues((prev) => ({ ...prev, imagePath: event.target.value }))}
        />
      </div>

      <label className="checkbox-row">
        <input
          type="checkbox"
          checked={values.isVisibleOnCard}
          onChange={(event) =>
            setValues((prev) => ({ ...prev, isVisibleOnCard: event.target.checked }))
          }
        />
        Visible on card
      </label>

      {error && <p className="field-error">{error}</p>}

      <div className="form-actions">
        <button className="button" type="submit" disabled={isSubmitting}>
          {isSubmitting ? "Saving..." : "Save"}
        </button>
        <button
          className="button secondary"
          type="button"
          onClick={onCancel}
          disabled={isSubmitting}
        >
          Cancel
        </button>
      </div>
    </form>
  );
}
