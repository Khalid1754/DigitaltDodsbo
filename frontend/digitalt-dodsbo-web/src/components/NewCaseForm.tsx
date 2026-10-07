import { useEffect, useState } from "react";
import { api } from "../api/client";
import type { Provider, EstateCase } from "../types";

interface Props {
  onCaseCreated: (estateCase: EstateCase) => void;
}

export function NewCaseForm({ onCaseCreated }: Props) {
  const [providers, setProviders] = useState<Provider[]>([]);
  const [selectedIds, setSelectedIds] = useState<number[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const [form, setForm] = useState({
    deceasedName: "",
    deceasedPersonnummer: "",
    dateOfDeath: "",
    relativeName: "",
    relativeRelation: "",
    relativeContact: "",
  });

  useEffect(() => {
    api.getProviders().then(setProviders).catch((e) => setError(e.message));
  }, []);

  function toggleProvider(id: number) {
    setSelectedIds((prev) =>
      prev.includes(id) ? prev.filter((x) => x !== id) : [...prev, id]
    );
  }

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    if (selectedIds.length === 0) {
      setError("Välj minst en tjänst.");
      return;
    }
    setLoading(true);
    setError(null);
    try {
      const created = await api.createCase({ ...form, providerIds: selectedIds });
      onCaseCreated(created);
    } catch (err) {
      setError(err instanceof Error ? err.message : "Något gick fel.");
    } finally {
      setLoading(false);
    }
  }

  return (
    <form onSubmit={handleSubmit} className="case-form">
      <h2>Nytt ärende</h2>

      <fieldset>
        <legend>Avliden person</legend>
        <label>
          Namn
          <input
            required
            value={form.deceasedName}
            onChange={(e) => setForm({ ...form, deceasedName: e.target.value })}
          />
        </label>
        <label>
          Personnummer
          <input
            required
            value={form.deceasedPersonnummer}
            onChange={(e) =>
              setForm({ ...form, deceasedPersonnummer: e.target.value })
            }
          />
        </label>
        <label>
          Dödsdatum
          <input
            required
            type="date"
            value={form.dateOfDeath}
            onChange={(e) => setForm({ ...form, dateOfDeath: e.target.value })}
          />
        </label>
      </fieldset>

      <fieldset>
        <legend>Anhörig (du)</legend>
        <label>
          Namn
          <input
            required
            value={form.relativeName}
            onChange={(e) => setForm({ ...form, relativeName: e.target.value })}
          />
        </label>
        <label>
          Relation
          <input
            required
            placeholder="t.ex. son, dotter, make/maka"
            value={form.relativeRelation}
            onChange={(e) =>
              setForm({ ...form, relativeRelation: e.target.value })
            }
          />
        </label>
        <label>
          Kontakt (e-post/telefon)
          <input
            required
            value={form.relativeContact}
            onChange={(e) =>
              setForm({ ...form, relativeContact: e.target.value })
            }
          />
        </label>
      </fieldset>

      <fieldset>
        <legend>Tjänster att avsluta</legend>
        {providers.map((p) => (
          <label key={p.id} className="provider-checkbox">
            <input
              type="checkbox"
              checked={selectedIds.includes(p.id)}
              onChange={() => toggleProvider(p.id)}
            />
            {p.name} <span className="category">({p.category})</span>
          </label>
        ))}
      </fieldset>

      {error && <p className="error">{error}</p>}

      <button type="submit" disabled={loading}>
        {loading ? "Genererar brev..." : "Skapa ärende och generera brev"}
      </button>
    </form>
  );
}
