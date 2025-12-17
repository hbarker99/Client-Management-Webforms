import { useState, useEffect, useCallback } from 'react';
import { ClientService } from '../services/ClientService';
import type { Client } from '../models/Client';

export const useClientDetail = (id: string | undefined) => {
  const [client, setClient] = useState<Client | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const fetchDetail = useCallback(async () => {
    if (!id) return;
    setLoading(true);
    try {
      const data = await ClientService.getClientById(id);
      setClient(data);
      setError(null);
    } catch (err) {
      setError("Failed to load client details.");
      console.error(err);
    } finally {
      setLoading(false);
    }
  }, [id]);

  useEffect(() => {
    fetchDetail();
  }, [fetchDetail]);

  return { client, loading, error, refresh: fetchDetail };
};

export default useClientDetail;