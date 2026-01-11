<script lang="ts">
	import { user, isAuthenticated } from '$lib/auth';
	import { goto } from '$app/navigation';
	import { onMount } from 'svelte';
	import { getObjekti, exportCsv, exportJson } from '$lib/api';

	let exporting = false;
	let exportError: string | null = null;

	onMount(() => {
		if (!$isAuthenticated) {
			goto('/');
		}
	});

	async function handleExport(format: 'csv' | 'json') {
		exporting = true;
		exportError = null;

		try {
            // dohvat najnovijih podataka iz baze
			const allData = await getObjekti();

			let blob: Blob;
			let filename: string;

            // export
			if (format === 'csv') {
				blob = await exportCsv(allData);
				filename = 'kafici_restorani.csv';
			} else {
				blob = await exportJson(allData);
				filename = 'kafici_restorani.json';
			}

            // download
			const url = window.URL.createObjectURL(blob);
			const a = document.createElement('a');
			a.href = url;
			a.download = filename;
			document.body.appendChild(a);
			a.click();
			window.URL.revokeObjectURL(url);
			document.body.removeChild(a);
		} catch (error: any) {
			exportError = error.message || 'Failed to export data';
		} finally {
			exporting = false;
		}
	}
</script>

<div class="container mx-auto px-4 py-8">
	<div class="mx-auto max-w-2xl">
		<div class="mb-6">
			<a href="/" class="text-blue-600 hover:text-blue-700"> &larr; Natrag na početnu </a>
		</div>

		<div class="rounded-lg border bg-white p-8 shadow-sm">
			<div class="mb-6 flex items-center gap-4">
				{#if $user?.picture}
					<img src={$user.picture} alt="User avatar" class="h-16 w-16 rounded-full" />
				{:else}
					<div
						class="flex h-16 w-16 items-center justify-center rounded-full bg-blue-500 text-white"
					>
						<svg class="h-8 w-8" fill="currentColor" viewBox="0 0 20 20">
							<path
								fill-rule="evenodd"
								d="M10 9a3 3 0 100-6 3 3 0 000 6zm-7 9a7 7 0 1114 0H3z"
								clip-rule="evenodd"
							/>
						</svg>
					</div>
				{/if}
				<div>
					<h1 class="text-2xl font-bold text-gray-900">Korisnički Profil</h1>
					<p class="text-gray-600">Osnovne informacije</p>
				</div>
			</div>

			<div class="space-y-4">
				{#if $user?.email}
					<div class="border-b pb-4">
						<div class="text-sm font-medium text-gray-500">Email</div>
						<p class="mt-1 text-lg text-gray-900">{$user.email}</p>
					</div>
				{/if}

				{#if $user?.name}
					<div class="pb-4">
						<div class="text-sm font-medium text-gray-500">Ime</div>
						<p class="mt-1 text-lg text-gray-900">{$user.name}</p>
					</div>
				{/if}
			</div>

			<div class="mt-8 border-t pt-6">
				<h3 class="mb-4 text-lg font-semibold text-gray-900">Osvježi preslike</h3>
				<p class="mb-4 text-sm text-gray-600">
					Preuzmite najnovije podatke iz baze u CSV ili JSON formatu.
				</p>

				{#if exportError}
					<div class="mb-4 rounded bg-red-50 p-3 text-sm text-red-700">
						{exportError}
					</div>
				{/if}

				<div class="flex gap-3">
					<button
						onclick={() => handleExport('csv')}
						disabled={exporting}
						class="flex items-center gap-2 rounded-lg bg-blue-600 px-6 py-3 font-medium text-white hover:bg-blue-700 disabled:opacity-50"
					>
						{#if exporting}
							<svg class="h-5 w-5 animate-spin" fill="none" viewBox="0 0 24 24">
								<circle
									class="opacity-25"
									cx="12"
									cy="12"
									r="10"
									stroke="currentColor"
									stroke-width="4"
								></circle>
								<path
									class="opacity-75"
									fill="currentColor"
									d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"
								></path>
							</svg>
							Preuzimanje...
						{:else}
							<svg class="h-5 w-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
								<path
									stroke-linecap="round"
									stroke-linejoin="round"
									stroke-width="2"
									d="M12 10v6m0 0l-3-3m3 3l3-3m2 8H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z"
								/>
							</svg>
							Preuzmi CSV
						{/if}
					</button>

					<button
						onclick={() => handleExport('json')}
						disabled={exporting}
						class="flex items-center gap-2 rounded-lg bg-green-600 px-6 py-3 font-medium text-white hover:bg-green-700 disabled:opacity-50"
					>
						{#if exporting}
							<svg class="h-5 w-5 animate-spin" fill="none" viewBox="0 0 24 24">
								<circle
									class="opacity-25"
									cx="12"
									cy="12"
									r="10"
									stroke="currentColor"
									stroke-width="4"
								></circle>
								<path
									class="opacity-75"
									fill="currentColor"
									d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"
								></path>
							</svg>
							Preuzimanje...
						{:else}
							<svg class="h-5 w-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
								<path
									stroke-linecap="round"
									stroke-linejoin="round"
									stroke-width="2"
									d="M12 10v6m0 0l-3-3m3 3l3-3m2 8H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z"
								/>
							</svg>
							Preuzmi JSON
						{/if}
					</button>
				</div>
			</div>
		</div>
	</div>
</div>
