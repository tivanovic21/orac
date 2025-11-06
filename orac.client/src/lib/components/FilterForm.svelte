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
		{ value: 'tag', label: 'Tag' }
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

<form on:submit={handleSubmit} class="mb-4 border bg-white p-4">
	<div class="flex flex-wrap items-end gap-3">
		<div class="min-w-[200px] flex-1">
			<label for="searchTerm" class="mb-1 block text-sm">Pretraži</label>
			<input
				id="searchTerm"
				type="text"
				bind:value={searchTerm}
				placeholder="Unesite pojam..."
				class="w-full border px-3 py-2"
			/>
		</div>
		<div class="w-48">
			<label for="searchField" class="mb-1 block text-sm">Pretraži po</label>
			<select id="searchField" bind:value={searchField} class="w-full border px-3 py-2">
				{#each searchFields as field}
					<option value={field.value}>{field.label}</option>
				{/each}
			</select>
		</div>
		<div class="flex gap-2">
			<button type="submit" class="bg-blue-600 px-4 py-2 text-white hover:bg-blue-700">
				Pretraži
			</button>
			<button type="button" on:click={handleClear} class="border px-4 py-2 hover:bg-gray-100">
				Očisti
			</button>
		</div>
	</div>
</form>
