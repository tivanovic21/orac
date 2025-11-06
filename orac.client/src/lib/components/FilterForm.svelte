<script lang="ts">
	import { createEventDispatcher } from 'svelte';
	const dispatch = createEventDispatcher<{
		filter: {
			searchTerm: string;
			searchField: string;
			dostupnaDostava: boolean | null;
			cjenovniRang: string | null;
			minOcjena: number | null;
			maxOcjena: number | null;
		};
		clear: void;
	}>();

	let searchTerm = '';
	let searchField = 'wildcard';
	let dostavaFilter = 'all';
	let cjenovniRangFilter = 'all';
	let minOcjena: number | null = null;
	let maxOcjena: number | null = null;

	const searchFields = [
		{ value: 'wildcard', label: 'Sva polja (wildcard)' },
		{ value: 'naziv', label: 'Naziv' },
		{ value: 'opis', label: 'Opis' },
		{ value: 'vlasnik', label: 'Vlasnik' },
		{ value: 'tipObjekta', label: 'Tip objekta' },
		{ value: 'grad', label: 'Grad' },
		{ value: 'ulica', label: 'Ulica' },
		{ value: 'tag', label: 'Tag' }
	];

	function handleSubmit(e: Event) {
		e.preventDefault();

		const dostupnaDostava = dostavaFilter === 'all' ? null : dostavaFilter === 'true';
		const cjenovniRang = cjenovniRangFilter === 'all' ? null : cjenovniRangFilter;

		dispatch('filter', {
			searchTerm: searchTerm.trim(),
			searchField,
			dostupnaDostava,
			cjenovniRang,
			minOcjena,
			maxOcjena
		});
	}

	function handleClear() {
		searchTerm = '';
		searchField = 'wildcard';
		dostavaFilter = 'all';
		cjenovniRangFilter = 'all';
		minOcjena = null;
		maxOcjena = null;
		dispatch('clear');
	}
</script>

<form on:submit={handleSubmit} class="mb-4 border bg-white p-4">
	<div class="mb-3 flex flex-wrap gap-3">
		<div class="min-w-[200px] flex-1">
			<label for="searchTerm" class="mb-1 block text-sm">Pojam za pretragu:</label>
			<input
				id="searchTerm"
				type="text"
				bind:value={searchTerm}
				placeholder="Unesite pojam..."
				class="w-full border px-3 py-2"
			/>
		</div>

		<div class="min-w-[200px]">
			<label for="searchField" class="mb-1 block text-sm">Polje:</label>
			<select id="searchField" bind:value={searchField} class="w-full border px-3 py-2 pr-8">
				{#each searchFields as field}
					<option value={field.value}>{field.label}</option>
				{/each}
			</select>
		</div>

		<div class="min-w-[150px]">
			<label for="dostavaFilter" class="mb-1 block text-sm">Dostava:</label>
			<select id="dostavaFilter" bind:value={dostavaFilter} class="w-full border px-3 py-2 pr-8">
				<option value="all">Sve</option>
				<option value="true">Da</option>
				<option value="false">Ne</option>
			</select>
		</div>

		<div class="min-w-[130px]">
			<label for="cjenovniRangFilter" class="mb-1 block text-sm">Cijena:</label>
			<select id="cjenovniRangFilter" bind:value={cjenovniRangFilter} class="w-full border px-3 py-2 pr-8">
				<option value="all">Sve</option>
				<option value="$">$</option>
				<option value="$$">$$</option>
				<option value="$$$">$$$</option>
			</select>
		</div>
	</div>

	<div class="mb-3 flex flex-wrap gap-3">
		<div class="min-w-[120px]">
			<label for="minOcjena" class="mb-1 block text-sm">Min ocjena:</label>
			<input
				id="minOcjena"
				type="number"
				bind:value={minOcjena}
				placeholder="0.0"
				min="0"
				max="5"
				step="0.1"
				class="w-full border px-3 py-2"
			/>
		</div>

		<div class="min-w-[120px]">
			<label for="maxOcjena" class="mb-1 block text-sm">Max ocjena:</label>
			<input
				id="maxOcjena"
				type="number"
				bind:value={maxOcjena}
				placeholder="5.0"
				min="0"
				max="5"
				step="0.1"
				class="w-full border px-3 py-2"
			/>
		</div>
	</div>

	<div class="flex gap-2">
		<button type="submit" class="bg-blue-600 px-4 py-2 text-white hover:bg-blue-700">
			Pretraži
		</button>
		<button
			type="button"
			on:click={handleClear}
			class="border border-gray-300 bg-white px-4 py-2 hover:bg-gray-50"
		>
			Očisti
		</button>
	</div>
</form>
