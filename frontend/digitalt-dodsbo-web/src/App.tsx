import { useState } from "react";
import { NewCaseForm } from "./components/NewCaseForm";
import { CaseDetail } from "./components/CaseDetail";
import { api } from "./api/client";
import type { EstateCase } from "./types";
import "./App.css";

export default function App() {
  const [activeCase, setActiveCase] = useState<EstateCase | null>(null);

  async function refreshCase() {
    if (!activeCase) return;
    const updated = await api.getCase(activeCase.id);
    setActiveCase(updated);
  }

  return (
    <div className="app">
      <header>
        <h1>Digitalt Dödsbo</h1>
        <p className="subtitle">
          Hjälp att avsluta en avliden persons digitala tjänster
        </p>
      </header>

      <main>
        {!activeCase ? (
          <NewCaseForm onCaseCreated={setActiveCase} />
        ) : (
          <>
            <button className="back-button" onClick={() => setActiveCase(null)}>
              ← Nytt ärende
            </button>
            <CaseDetail estateCase={activeCase} onUpdated={refreshCase} />
          </>
        )}
      </main>
    </div>
  );
}
