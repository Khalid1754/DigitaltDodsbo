import type { Provider, EstateCase, CreateCaseRequest, ItemStatus } from "../types";

const BASE_URL = "http://localhost:5000/api";

async function handle<T>(res: Response): Promise<T> {
  if (!res.ok) {
    const text = await res.text();
    throw new Error(`API-fel (${res.status}): ${text}`);
  }
  // 204 No Content har ingen body
  if (res.status === 204) return undefined as T;
  return res.json();
}

export const api = {
  getProviders: (): Promise<Provider[]> =>
    fetch(`${BASE_URL}/providers`).then((r) => handle(r)),

  getCases: (): Promise<EstateCase[]> =>
    fetch(`${BASE_URL}/cases`).then((r) => handle(r)),

  getCase: (id: number): Promise<EstateCase> =>
    fetch(`${BASE_URL}/cases/${id}`).then((r) => handle(r)),

  createCase: (request: CreateCaseRequest): Promise<EstateCase> =>
    fetch(`${BASE_URL}/cases`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(request),
    }).then((r) => handle(r)),

  updateItemStatus: (
    caseId: number,
    itemId: number,
    status: ItemStatus
  ): Promise<void> =>
    fetch(`${BASE_URL}/cases/${caseId}/items/${itemId}/status`, {
      method: "PATCH",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(status),
    }).then((r) => handle(r)),
};
