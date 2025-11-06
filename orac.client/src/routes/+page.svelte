<script lang="ts">
	import StaticDownloadButtons from '$lib/components/StaticDownloadButtons.svelte';
</script>

<div class="flex min-h-screen items-center justify-center">
	<div class="flex w-full max-w-2xl flex-col items-center justify-center border bg-white p-8">
		<h1 class="mb-4 text-3xl font-bold">Kafići i Restorani</h1>
		<p class="mb-8 text-gray-600">Pregled lokalnih kafića i restorana u Zagrebu</p>

		<div class="mb-6">
			<StaticDownloadButtons />
		</div>

		<a
			href="/datatable"
			class="inline-block rounded bg-blue-600 px-6 py-3 text-white hover:bg-blue-700"
		>
			Pregled objekata
		</a>
	</div>
</div>
