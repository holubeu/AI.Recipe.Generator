import { useState } from "react";
import NoticeBlock from "../components/NoticeBlock";
import { addApiKey } from "../lib/apiClient";

const warningMessage =
  "WARNING: If you add an invalid key, recipe generation will stop working. Here you can add a new API key for the OpenRouter API for the model 'arcee-ai/trinity-large-preview:free'.";

export default function ApiKeysPage() {
  const [isFormOpen, setIsFormOpen] = useState(false);
  const [apiKey, setApiKey] = useState("");
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState<string | null>(null);

  const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    const trimmed = apiKey.trim();
    if (!trimmed) {
      return;
    }

    setIsSubmitting(true);
    setError(null);
    setSuccess(null);

    try {
      await addApiKey({ key: trimmed });
      setSuccess("API key added.");
      setApiKey("");
      setIsFormOpen(false);
    } catch (submitError) {
      setError(submitError instanceof Error ? submitError.message : "Failed to add API key.");
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <section className="card">
      <h2>API Keys</h2>
      <NoticeBlock title="Important" message={warningMessage} />

      <button className="button" onClick={() => setIsFormOpen(true)}>
        API key
      </button>

      {isFormOpen && (
        <form className="form-card" onSubmit={handleSubmit}>
          <div className="field-row">
            <label className="field-label" htmlFor="api-key-input">
              New API key
            </label>
            <input
              id="api-key-input"
              className="text-input"
              value={apiKey}
              onChange={(event) => setApiKey(event.target.value)}
              placeholder="Enter API key"
            />
          </div>

          <div className="form-actions">
            <button className="button" type="submit" disabled={isSubmitting}>
              {isSubmitting ? "Saving..." : "Save"}
            </button>
            <button
              className="button secondary"
              type="button"
              onClick={() => setIsFormOpen(false)}
              disabled={isSubmitting}
            >
              Cancel
            </button>
          </div>

          {error && <p className="warning">{error}</p>}
          {success && <p className="success">{success}</p>}
        </form>
      )}
    </section>
  );
}
