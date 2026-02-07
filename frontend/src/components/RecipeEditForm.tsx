import type { GetRecipeResponseModel } from "../lib/apiResponseModels";

type EditableRecipeFields = {
  name: string;
  description: string;
  dishType: string;
};

type RecipeEditFormProps = {
  recipe: GetRecipeResponseModel;
  fields: EditableRecipeFields;
  onChange: (fields: EditableRecipeFields) => void;
  onSave: () => void;
  onCancel: () => void;
  isSaving: boolean;
};

export default function RecipeEditForm({
  recipe,
  fields,
  onChange,
  onSave,
  onCancel,
  isSaving,
}: RecipeEditFormProps) {
  return (
    <div className="recipe-form">
      <div className="field-row">
        <label className="field-label" htmlFor={`edit-name-${recipe.id}`}>
          Name
        </label>
        <input
          id={`edit-name-${recipe.id}`}
          className="text-input"
          value={fields.name}
          onChange={(event) => onChange({ ...fields, name: event.target.value })}
        />
      </div>

      <div className="field-row">
        <label className="field-label" htmlFor={`edit-type-${recipe.id}`}>
          Dish Type
        </label>
        <input
          id={`edit-type-${recipe.id}`}
          className="text-input"
          value={fields.dishType}
          onChange={(event) => onChange({ ...fields, dishType: event.target.value })}
        />
      </div>

      <div className="field-row">
        <label className="field-label" htmlFor={`edit-description-${recipe.id}`}>
          Description
        </label>
        <textarea
          id={`edit-description-${recipe.id}`}
          className="text-area"
          value={fields.description}
          onChange={(event) => onChange({ ...fields, description: event.target.value })}
        />
      </div>

      <div className="field-row">
        <span className="field-label">Cooking Instructions</span>
        <ul className="steps-list">
          {recipe.steps.map((step, index) => (
            <li key={`${step}-${index}`}>{step}</li>
          ))}
        </ul>
      </div>

      <div className="recipe-actions">
        <button className="button" onClick={onSave} disabled={isSaving}>
          {isSaving ? "Saving..." : "Save"}
        </button>
        <button className="button secondary" onClick={onCancel}>
          Cancel
        </button>
      </div>
    </div>
  );
}
