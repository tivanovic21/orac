<script lang="ts">
	import type { Objekt } from '$lib/types/types';
	import { onMount } from 'svelte';
	import FilterForm from '$lib/components/FilterForm.svelte';
	import ObjektTable from '$lib/components/ObjektTable.svelte';
	import ExportButtons from '$lib/components/ExportButtons.svelte';
	import { getObjekti, getFilteredObjekti } from '$lib/api';

	let objekti: Objekt[] = [];
	let loading = true;
	let err: string | null = null;

	async function fetchObjekti(searchTerm?: string, searchField?: string) {
		loading = true;
		err = null;

		try {
			if (searchTerm && searchTerm.trim() !== '') {
				objekti = await getFilteredObjekti(searchTerm.trim(), searchField || 'wildcard');
			} else {
				objekti = await getObjekti();
			}
		} catch (e: any) {
			err = e.message ?? 'Unknown error';
			objekti = [];
		} finally {
			loading = false;
		}
	}

	function handleFilter(event: CustomEvent<{ searchTerm: string; searchField: string }>) {
		const { searchTerm, searchField } = event.detail;
		fetchObjekti(searchTerm, searchField);
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
		<a href="/" class="border px-4 py-2 hover:bg-gray-50"> ← Natrag </a>
	</div>

	<FilterForm on:filter={handleFilter} on:clear={handleClear} />

	{#if loading}
		<div class="py-12 text-center">
			<p class="text-gray-600">Učitavanje...</p>
		</div>
	{:else if err}
		<div class="border border-red-300 bg-red-50 p-4">
			<p class="text-red-700">{err}</p>
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
