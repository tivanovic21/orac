import type { Objekt } from './types/types';
import { env } from '$env/dynamic/public';

const API_BASE = env.PUBLIC_API_URL;
const OBJEKT_URL = env.PUBLIC_API_OBJEKTI_ENDPOINT;

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
    searchField: string = 'wildcard',
    dostupnaDostava: boolean | null = null,
    cjenovniRang: string | null = null,
    minOcjena: number | null = null,
    maxOcjena: number | null = null
): Promise<Objekt[]> {
    const params = new URLSearchParams({
        searchTerm,
        searchField
    });

    if (dostupnaDostava !== null) {
        params.append('dostupnaDostava', dostupnaDostava.toString());
    }
    if (cjenovniRang !== null) {
        params.append('cjenovniRang', cjenovniRang);
    }
    if (minOcjena !== null) {
        params.append('minOcjena', minOcjena.toString());
    }
    if (maxOcjena !== null) {
        params.append('maxOcjena', maxOcjena.toString());
    }

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

export async function exportCsv(objekti: Objekt[]): Promise<Blob> {
    const res = await fetch(`${API_BASE}/${OBJEKT_URL}/exportcsv`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(objekti),
    });

    if (!res.ok) {
        const text = await res.text();
        throw new Error(`Export CSV error ${res.status}: ${text}`);
    }

    return await res.blob();
}

export async function exportJson(objekti: Objekt[]): Promise<Blob> {
    const res = await fetch(`${API_BASE}/${OBJEKT_URL}/exportjson`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(objekti),
    });

    if (!res.ok) {
        const text = await res.text();
        throw new Error(`Export JSON error ${res.status}: ${text}`);
    }

    return await res.blob();
}
