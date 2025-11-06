<script lang="ts">
  import { createEventDispatcher } from 'svelte';

  const dispatch = createEventDispatcher<{
    filter: { searchTerm: string; searchField: string };
    clear: void;
  }>();

  let searchTerm = '';
  let searchField = 'wildcard';

  const searchFields = [
    { value: 'wildcard', label: 'Sva polja (wildcard)' },
    { value: 'naziv', label: 'Naziv' },
    { value: 'opis', label: 'Opis' },
    { value: 'vlasnik', label: 'Vlasnik' },
    { value: 'tipObjekta', label: 'Tip objekta' },
    { value: 'cjenovniRang', label: 'Cjenovni rang' },
    { value: 'grad', label: 'Grad' },
    { value: 'ulica', label: 'Ulica' },
    { value: 'tag', label: 'Tag' },
  ];

  function handleSubmit(e: Event) {
    e.preventDefault();
    dispatch('filter', { searchTerm: searchTerm.trim(), searchField });
  }

  function handleClear() {
    searchTerm = '';
    searchField = 'wildcard';
    dispatch('clear');
  }
</script>

<form on:submit={handleSubmit} class="bg-white border p-4 mb-4">
  <div class="flex flex-wrap gap-3 items-end">
    <div class="flex-1 min-w-[200px]">
      <label for="searchTerm" class="block text-sm mb-1">Pretraži</label>
      <input
        id="searchTerm"
        type="text"
        bind:value={searchTerm}
        placeholder="Unesite pojam..."
        class="w-full px-3 py-2 border"
      />
    </div>

    <div class="w-48">
      <label for="searchField" class="block text-sm mb-1">Pretraži po</label>
      <select
        id="searchField"
        bind:value={searchField}
        class="w-full px-3 py-2 border"
      >
        {#each searchFields as field}
          <option value={field.value}>{field.label}</option>
        {/each}
      </select>
    </div>

    <div class="flex gap-2">
      <button type="submit" class="px-4 py-2 bg-blue-600 hover:bg-blue-700 text-white">
        Pretraži
      </button>
      <button type="button" on:click={handleClear} class="px-4 py-2 border hover:bg-gray-100">
        Očisti
      </button>
    </div>
  </div>
</form>
