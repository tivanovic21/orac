<script lang="ts">
  import type { Objekt } from '$lib/types/types';
  import { onMount } from 'svelte';
  import FilterForm from '$lib/components/FilterForm.svelte';
  import ObjektTable from '$lib/components/ObjektTable.svelte';

  let objekti: Objekt[] = [];
  let loading = true;
  let err: string | null = null;

  async function fetchObjekti(searchTerm?: string, searchField?: string) {
    loading = true;
    err = null;
    
    try {
      let url = 'http://localhost:5000/api/objekt';
      
      if (searchTerm && searchTerm.trim() !== '') {
        const params = new URLSearchParams({
          searchTerm: searchTerm.trim(),
          searchField: searchField || 'wildcard'
        });
        url += `/getfiltered?${params.toString()}`;
      } else {
        url += '/getall';
      }
      
      const res = await fetch(url);
      if (!res.ok) throw new Error('Failed to fetch objekti');
      objekti = await res.json();
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
      <h1 class="text-2xl font-bold mb-1">Pregled Objekata</h1>
      <p class="text-gray-600">Pretražite i filtrirajte kafiće i restorane</p>
    </div>
    <a href="/" class="px-4 py-2 border hover:bg-gray-50">
      ← Natrag
    </a>
  </div>

  <FilterForm on:filter={handleFilter} on:clear={handleClear} />

  {#if loading}
    <div class="text-center py-12">
      <p class="text-gray-600">Učitavanje...</p>
    </div>
  {:else if err}
    <div class="bg-red-50 border border-red-300 p-4">
      <p class="text-red-700">{err}</p>
    </div>
  {:else}
    <div class="mb-3 text-sm text-gray-600">
      Pronađeno rezultata: <strong>{objekti.length}</strong>
    </div>
    <ObjektTable {objekti} />
  {/if}
</div>
