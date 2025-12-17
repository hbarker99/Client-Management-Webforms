import type { Asset } from "./Assets";
import type { Expenditure } from "./Expenditure";
import type { Income } from "./Income";
import type { Journal } from "./Journal";
import type { Liability } from "./Liabilities";

export interface ClientInfo {
  id: number;
  name: string;
  firstName: string;
  lastName: string;
  dateOfBirth: string;
  status: number;
  email: string;
  phone: string;
  createdAt: string;
}

export interface Client extends ClientInfo {
  journals: Journal[];
  assets: Asset[];
  liabilities: Liability[];
  incomes: Income[];
  expenditures: Expenditure[];
}

export interface ClientSearchParams {
  sortBy?: string;
  direction?: 'asc' | 'desc';
  searchTerm?: string;
  status?: number;
  page?: number;
  pageSize?: number;
}

export interface ClientSearchResult {
  results: ClientInfo[];
  count: number;
}
