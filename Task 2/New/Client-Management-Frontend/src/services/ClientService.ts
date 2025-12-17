import type { Client, ClientSearchParams, ClientSearchResult } from "../models/Client";

const BASE_URL = 'https://localhost:7105/api/clients';

export const ClientService = {
  async getClients(params: ClientSearchParams): Promise<ClientSearchResult> {
    const query = new URLSearchParams();
    console.log(params);
    if (params.sortBy) query.append('sortBy', params.sortBy);
    if (params.direction) query.append('sortDir', params.direction);
    if (params.searchTerm) query.append('search', params.searchTerm);
    if (params.status) query.append('status', params.status.toString());
    if (params.page) query.append('pageNumber', params.page.toString());
    if (params.pageSize) query.append('pageSize', params.pageSize.toString());

    const response = await fetch(`${BASE_URL}?${query.toString()}`);
    
    if (!response.ok) {
      throw new Error('Failed to fetch clients');
    }
    
    return response.json();
  },
  
  async getClientById(id: string): Promise<Client> {
    const response = await fetch(`${BASE_URL}/${id}`);
    if (!response.ok) throw new Error('Client not found');
    return response.json();
  }
};

export default ClientService;