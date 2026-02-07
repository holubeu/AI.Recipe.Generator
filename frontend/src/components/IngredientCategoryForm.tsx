import { useEffect, useState } from "react";

export type IngredientCategoryFormValues = {
  name: string;
};

type IngredientCategoryFormProps = {
  initialValues?: IngredientCategoryFormValues;
  onSubmit: (values: IngredientCategoryFormValues) => void;
  onCancel: () => void;
  isSubmitting: boolean;
};

const defaultValues: IngredientCategoryFormValues = {
  name: "",
};

export default function IngredientCategoryForm({
  initialValues,
  onSubmit,
  onCancel,
  isSubmitting,
}: IngredientCategoryFormProps) {
  const [values, setValues] = useState<IngredientCategoryFormValues>(
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

    setError(null);
    onSubmit({
      name: values.name.trim(),
    });
  };

  return (
    <form className="form-card" onSubmit={handleSubmit}>
      <div className="field-row">
        <label className="field-label" htmlFor="category-name">
          Name
        </label>
        <input
          id="category-name"
          className="text-input"
          value={values.name}
          onChange={(event) => setValues({ name: event.target.value })}
        />
      </div>

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
