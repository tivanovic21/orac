<script lang="ts">
	import type { Objekt } from '$lib/types/types';
	import { exportCsv, exportJson } from '$lib/api';

	export let objekti: Objekt[] = [];

	async function handleExportCsv() {
		try {
			const blob = await exportCsv(objekti);
			const url = window.URL.createObjectURL(blob);
			const a = document.createElement('a');
			a.href = url;
			a.download = 'kafici_restorani.csv';
			document.body.appendChild(a);
			a.click();
			document.body.removeChild(a);
			window.URL.revokeObjectURL(url);
		} catch (error) {
			console.error('CSV export failed:', error);
			alert('Failed to export CSV');
		}
	}

	async function handleExportJson() {
		try {
			const blob = await exportJson(objekti);
			const url = window.URL.createObjectURL(blob);
			const a = document.createElement('a');
			a.href = url;
			a.download = 'kafici_restorani.json';
			document.body.appendChild(a);
			a.click();
			document.body.removeChild(a);
			window.URL.revokeObjectURL(url);
		} catch (error) {
			console.error('JSON export failed:', error);
			alert('Failed to export JSON');
		}
	}
</script>

<div class="flex gap-2">
	<button
		on:click={handleExportCsv}
		class="bg-green-600 px-4 py-2 text-sm text-white hover:bg-green-700"
	>
		Izvezi CSV
	</button>
	<button
		on:click={handleExportJson}
		class="bg-blue-600 px-4 py-2 text-sm text-white hover:bg-blue-700"
	>
		Izvezi JSON
	</button>
</div>
