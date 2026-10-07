import { useState } from "react";
import { api } from "../api/client";
import type { EstateCase, ItemStatus } from "../types";

const STATUS_LABELS: Record<ItemStatus, string> = {
  Pending: "Väntar",
  LetterReady: "Brev klart",
  Sent: "Skickat",
  Completed: "Avslutat",
  NeedsAction: "Kräver åtgärd",
};

const STATUS_ORDER: ItemStatus[] = [
  "Pending",
  "LetterReady",
  "Sent",
  "Completed",
  "NeedsAction",
];

interface Props {
  estateCase: EstateCase;
  onUpdated: () => void;
}

export function CaseDetail({ estateCase, onUpdated }: Props) {
  const [expandedId, setExpandedId] = useState<number | null>(null);
  const [updatingId, setUpdatingId] = useState<number | null>(null);

  async function handleStatusChange(itemId: number, status: ItemStatus) {
    setUpdatingId(itemId);
    try {
      await api.updateItemStatus(estateCase.id, itemId, status);
      onUpdated();
    } finally {
      setUpdatingId(null);
    }
  }

  function copyToClipboard(text: string) {
    navigator.clipboard.writeText(text);
  }

  return (
    <div className="case-detail">
      <h2>
        Ärende: {estateCase.deceasedName} ({estateCase.relativeName} ansvarig)
      </h2>

      <ul className="item-list">
        {estateCase.items.map((item) => (
          <li key={item.id} className={`item status-${item.status}`}>
            <div className="item-header">
              <strong>{item.providerName}</strong>
              <select
                value={item.status}
                disabled={updatingId === item.id}
                onChange={(e) =>
                  handleStatusChange(item.id, e.target.value as ItemStatus)
                }
              >
                {STATUS_ORDER.map((s) => (
                  <option key={s} value={s}>
                    {STATUS_LABELS[s]}
                  </option>
                ))}
              </select>
            </div>

            {item.generatedLetter && (
              <>
                <button
                  className="toggle-letter"
                  onClick={() =>
                    setExpandedId(expandedId === item.id ? null : item.id)
                  }
                >
                  {expandedId === item.id ? "Dölj brev" : "Visa brev"}
                </button>

                {expandedId === item.id && (
                  <div className="letter-box">
                    <pre>{item.generatedLetter}</pre>
                    <button onClick={() => copyToClipboard(item.generatedLetter!)}>
                      Kopiera text
                    </button>
                  </div>
                )}
              </>
            )}
          </li>
        ))}
      </ul>
    </div>
  );
}
