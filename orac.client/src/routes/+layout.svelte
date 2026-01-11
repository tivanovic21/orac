<script lang="ts">
	import { onMount } from 'svelte';
	import '../app.css';
	import favicon from '$lib/assets/favicon.svg';
	import { initAuth0, isAuthenticated, isLoading, login, logout, user } from '$lib/auth';

	let { children } = $props();

	onMount(async () => {
		await initAuth0();
	});
</script>

<svelte:head>
	<link rel="icon" href={favicon} />
</svelte:head>

{#if $isLoading}
	<div class="flex min-h-screen items-center justify-center">
		<div class="text-xl">Učitavanje...</div>
	</div>
{:else}
	<div class="min-h-screen bg-gray-50">
		<nav class="border-b bg-white px-4 py-3 shadow-sm">
			<div class="mx-auto flex max-w-7xl items-center justify-between">
				<div class="flex items-center gap-4">
					<a href="/" class="text-xl font-bold">Kafići i restorani</a>
				</div>
				<div class="flex items-center gap-4">
					{#if $isAuthenticated && $user}
						<div class="flex items-center gap-3">
							<a href="/profile" class="text-sm text-gray-700 hover:text-blue-600">
								Korisnički profil
							</a>
							<span class="text-sm text-gray-700">{$user.name || $user.email}</span>
							<button
								onclick={logout}
								class="rounded bg-red-600 px-4 py-2 text-sm text-white hover:bg-red-700"
							>
								Odjava
							</button>
						</div>
					{:else}
						<button
							onclick={login}
							class="rounded bg-blue-600 px-4 py-2 text-sm text-white hover:bg-blue-700"
						>
							Prijava
						</button>
					{/if}
				</div>
			</div>
		</nav>
		<main>
			{@render children()}
		</main>
	</div>
{/if}
