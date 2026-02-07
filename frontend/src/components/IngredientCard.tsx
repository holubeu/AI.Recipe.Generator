type IngredientCardProps = {
  name: string;
  imagePath?: string;
  variant: "ingredient" | "selected";
  onSelect?: () => void;
  onRemove?: () => void;
};

export default function IngredientCard({
  name,
  imagePath,
  variant,
  onSelect,
  onRemove,
}: IngredientCardProps) {
  const className = variant === "ingredient" ? "ingredient-card" : "selected-card";

  const content = (
    <>
      {imagePath && (
        <img
          className="ingredient-image"
          src={imagePath}
          alt={name}
        />
      )}
      <span>{name}</span>
      {onRemove && (
        <button
          type="button"
          className="remove-button"
          onClick={(event) => {
            event.stopPropagation();
            onRemove();
          }}
          aria-label={`Remove ${name}`}
        >
          ×
        </button>
      )}
    </>
  );

  if (onSelect) {
    return (
      <button type="button" className={className} onClick={onSelect}>
        {content}
      </button>
    );
  }

  return <div className={className}>{content}</div>;
}
