<script lang="ts">
	import type { Objekt } from '$lib/types/types';
	import { onMount } from 'svelte';
	import FilterForm from '$lib/components/FilterForm.svelte';
	import ObjektTable from '$lib/components/ObjektTable.svelte';
	import ExportButtons from '$lib/components/ExportButtons.svelte';
	import { getObjekti, getFilteredObjekti, AuthenticationError } from '$lib/api';
	import { login } from '$lib/auth';

	let objekti: Objekt[] = [];
	let loading = true;
	let err: string | null = null;
	let isAuthError = false;

	async function fetchObjekti(
		searchTerm?: string,
		searchField?: string,
		dostupnaDostava?: boolean | null,
		cjenovniRang?: string | null,
		minOcjena?: number | null,
		maxOcjena?: number | null
	) {
		loading = true;
		err = null;
		isAuthError = false;

		try {
			if (
				(searchTerm && searchTerm.trim() !== '') ||
				dostupnaDostava !== null ||
				cjenovniRang !== null ||
				minOcjena !== null ||
				maxOcjena !== null
			) {
				objekti = await getFilteredObjekti(
					searchTerm?.trim() || '',
					searchField || 'wildcard',
					dostupnaDostava ?? null,
					cjenovniRang ?? null,
					minOcjena ?? null,
					maxOcjena ?? null
				);
			} else {
				objekti = await getObjekti();
			}
		} catch (e: any) {
			if (e instanceof AuthenticationError) {
				isAuthError = true;
				err = e.message;
			} else {
				err = e.message ?? 'Unknown error';
			}
			objekti = [];
		} finally {
			loading = false;
		}
	}

	function handleFilter(
		event: CustomEvent<{
			searchTerm: string;
			searchField: string;
			dostupnaDostava: boolean | null;
			cjenovniRang: string | null;
			minOcjena: number | null;
			maxOcjena: number | null;
		}>
	) {
		const { searchTerm, searchField, dostupnaDostava, cjenovniRang, minOcjena, maxOcjena } =
			event.detail;
		fetchObjekti(searchTerm, searchField, dostupnaDostava, cjenovniRang, minOcjena, maxOcjena);
	}

	function handleClear() {
		fetchObjekti();
	}

	onMount(() => {
		fetchObjekti();
	});
</script>

<div class="container mx-auto px-4 py-8">
	<div class="mb-6 flex items-center justify-between">
		<div>
			<h1 class="mb-1 text-2xl font-bold">Pregled Objekata</h1>
			<p class="text-gray-600">Pretražite i filtrirajte kafiće i restorane</p>
		</div>
		<a href="/" class="border px-4 py-2 hover:bg-gray-50"> &larr; Natrag </a>
	</div>

	<FilterForm on:filter={handleFilter} on:clear={handleClear} />

	{#if loading}
		<div class="py-12 text-center">
			<p class="text-gray-600">Učitavanje...</p>
		</div>
	{:else if err}
		<div class="rounded-lg border-2 bg-white p-8 text-center shadow-sm">
			{#if isAuthError}
				<h2 class="mb-2 text-xl font-bold text-gray-900">Autentikacija potrebna!</h2>
				<p class="mb-6 text-gray-600">{err}</p>
				<button
					onclick={login}
					class="rounded-lg bg-blue-600 px-6 py-3 font-medium text-white hover:bg-blue-700"
				>
					Prijavite se za nastavak
				</button>
			{:else}
				<div class="mb-4">
					<svg
						class="mx-auto h-16 w-16 text-red-500"
						fill="none"
						stroke="currentColor"
						viewBox="0 0 24 24"
					>
						<path
							stroke-linecap="round"
							stroke-linejoin="round"
							stroke-width="2"
							d="M12 8v4m0 4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z"
						/>
					</svg>
				</div>
				<h2 class="mb-2 text-xl font-bold text-gray-900">Error</h2>
				<p class="text-gray-600">{err}</p>
			{/if}
		</div>
	{:else}
		<div class="mb-3 flex items-center justify-between">
			<div class="text-sm text-gray-600">
				Pronađeno rezultata: <strong>{objekti.length}</strong>
			</div>
			<ExportButtons {objekti} />
		</div>
		<ObjektTable {objekti} />
	{/if}
</div>
