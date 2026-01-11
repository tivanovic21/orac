import type { ApiResponse, Objekt } from './types/types';
import { env } from '$env/dynamic/public';
import { getAccessToken } from './auth';

const API_BASE = env.PUBLIC_API_URL;
const OBJEKT_URL = env.PUBLIC_API_OBJEKTI_ENDPOINT;

// za hendlanje auth grešaka
export class AuthenticationError extends Error {
    constructor(message: string) {
        super(message);
        this.name = 'AuthenticationError';
    }
}

async function getAuthHeaders(): Promise<HeadersInit> {
    const token = await getAccessToken();
    const headers: HeadersInit = {
        'Content-Type': 'application/json'
    };
    if (token) {
        headers['Authorization'] = `Bearer ${token}`;
    }
    return headers;
}

export async function getObjekti(): Promise<Objekt[]> {
    const headers = await getAuthHeaders();
    const res = await fetch(`${API_BASE}/${OBJEKT_URL}/getall`, {
        credentials: 'omit',
        headers
    });

    if (!res.ok) {
        if (res.status === 401) {
            throw new AuthenticationError('Morate se prijaviti za pristup podacima');
        }

        const text = await res.text();
        throw new Error(`API error ${res.status}: ${text}`);
    }

    const apiResponse = (await res.json()) as ApiResponse<Objekt[]>;
    return apiResponse.response;
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

    const headers = await getAuthHeaders();
    const res = await fetch(`${API_BASE}/${OBJEKT_URL}/getfiltered?${params.toString()}`, {
        credentials: 'omit',
        headers
    });

    if (!res.ok) {
        if (res.status === 401) {
            throw new AuthenticationError('Morate se prijaviti za pristup podacima');
        }

        const text = await res.text();
        throw new Error(`API error ${res.status}: ${text}`);
    }

    const apiResponse = (await res.json()) as ApiResponse<Objekt[]>;
    return apiResponse.response;
}

export async function exportCsv(objekti: Objekt[]): Promise<Blob> {
    const headers = await getAuthHeaders();
    const res = await fetch(`${API_BASE}/${OBJEKT_URL}/exportcsv`, {
        method: 'POST',
        headers,
        body: JSON.stringify(objekti),
    });

    if (!res.ok) {
        if (res.status === 401) {
            throw new AuthenticationError('Morate se prijaviti za pristup podacima');
        }

        const text = await res.text();
        throw new Error(`Export CSV error ${res.status}: ${text}`);
    }

    return await res.blob();
}

export async function exportJson(objekti: Objekt[]): Promise<Blob> {
    const headers = await getAuthHeaders();
    const res = await fetch(`${API_BASE}/${OBJEKT_URL}/exportjson`, {
        method: 'POST',
        headers,
        body: JSON.stringify(objekti),
    });

    if (!res.ok) {
        if (res.status === 401) {
            throw new AuthenticationError('Morate se prijaviti za pristup podacima');
        }

        const text = await res.text();
        throw new Error(`Export JSON error ${res.status}: ${text}`);
    }

    return await res.blob();
}
