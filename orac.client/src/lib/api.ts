import type { Objekt } from './types/types';

const API_BASE = import.meta.env.VITE_API_BASE ?? 'http://localhost:5000'; 
const OBJEKT_URL = 'api/objekt';

export async function getObjekti(): Promise<Objekt[]> {
  const res = await fetch(`${API_BASE}/${OBJEKT_URL}/getall`, {
    credentials: 'omit'
  });

  if (!res.ok) {
    const text = await res.text();
    throw new Error(`API error ${res.status}: ${text}`);
  }

  const data = (await res.json()) as Objekt[];
  return data;
}

export async function getFilteredObjekti(
  searchTerm: string,
  searchField: string = 'wildcard'
): Promise<Objekt[]> {
  const params = new URLSearchParams({
    searchTerm,
    searchField
  });

  const res = await fetch(`${API_BASE}/${OBJEKT_URL}/getfiltered?${params.toString()}`, {
    credentials: 'omit'
  });

  if (!res.ok) {
    const text = await res.text();
    throw new Error(`API error ${res.status}: ${text}`);
  }

  const data = (await res.json()) as Objekt[];
  return data;
}
