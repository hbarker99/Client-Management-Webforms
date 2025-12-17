import { useCallback, useEffect, useState } from "react";
import type { ClientInfo, ClientSearchParams } from "../models/Client";
import ClientService from "../services/ClientService";

export const useClients = (initialParams: ClientSearchParams) => {
  const [clientList, setClientList] = useState<ClientInfo[]>([]);
  const [clientCount, setClientCount] = useState<number>(0);
  
  const [page, setInternalPage] = useState<number>(initialParams.page || 1);
  const [pageSize, setPageSize] = useState<number>(initialParams.pageSize || 10);

  const [totalPages, setTotalPages] = useState<number>(1);
  
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const [params, setParams] = useState(initialParams);

  const fetchClients = useCallback(async () => {
    setLoading(true);
    try {
      const data = await ClientService.getClients(params);
      setClientList(data.results);
      setClientCount(data.count);
      setInternalPage(params.page || 1);
      setTotalPages(Math.ceil(data.count / pageSize));
      setError(null);
    } catch (err) {
      setError('Could not load clients.');
      console.error(err);
    } finally {
      setLoading(false);
    }
  }, [params]);

  const checkValidPage = (targetPage: number): boolean => {
    const totalPages = Math.ceil(clientCount / pageSize);
    return targetPage >= 1 && (clientCount === 0 || targetPage <= totalPages);
  };

  const goToPage = (newPage: number) => {
    if (!checkValidPage(newPage)) return;
    
    setParams((prev) => ({
      ...prev,
      page: newPage
    }));
  };

  const nextPage = () => goToPage(page + 1);
  const prevPage = () => goToPage(page - 1);

  const reset = () => {
    setParams({
      page: 1,
      pageSize: pageSize
    });
  }


  useEffect(() => {
    fetchClients();
  }, [fetchClients]);

  return { clientList, loading, error, page, setParams, params, setPageSize, nextPage, prevPage, goToPage, totalPages, clientCount, pageSize, reset };
};