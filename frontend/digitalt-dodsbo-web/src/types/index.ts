export interface Provider {
  id: number;
  name: string;
  category: string;
  requiresDeathCertificate: boolean;
  requiresEstateInventory: boolean;
  contactMethod: string;
  contactDetails: string;
  processNotes: string;
}

export type ItemStatus =
  | "Pending"
  | "LetterReady"
  | "Sent"
  | "Completed"
  | "NeedsAction";

export interface CaseItem {
  id: number;
  providerId: number;
  providerName: string;
  status: ItemStatus;
  generatedLetter: string | null;
}

export interface EstateCase {
  id: number;
  deceasedName: string;
  relativeName: string;
  createdAt: string;
  items: CaseItem[];
}

export interface CreateCaseRequest {
  deceasedName: string;
  deceasedPersonnummer: string;
  dateOfDeath: string; // yyyy-MM-dd
  relativeName: string;
  relativeRelation: string;
  relativeContact: string;
  providerIds: number[];
}
