<script lang="ts">
	import type { Objekt } from '$lib/types/types';

	export let objekti: Objekt[] = [];
</script>

{#if objekti.length === 0}
	<div class="py-8 text-center text-gray-500">
		<p>Nema rezultata za prikazati</p>
	</div>
{:else}
	<div class="overflow-x-auto border">
		<table class="min-w-full table-auto bg-white">
			<thead class="bg-gray-100">
				<tr>
					<th class="px-3 py-2 text-left text-xs">Naziv</th>
					<th class="px-3 py-2 text-left text-xs">Opis</th>
					<th class="px-2 py-2 text-left text-xs">Tip</th>
					<th class="px-2 py-2 text-left text-xs">Rang</th>
					<th class="px-2 py-2 text-left text-xs">Ocjena</th>
					<th class="px-3 py-2 text-left text-xs">Vlasnik</th>
					<th class="px-2 py-2 text-left text-xs">Dostava</th>
					<th class="px-3 py-2 text-left text-xs">Kontakt</th>
					<th class="px-3 py-2 text-left text-xs">Lokacije</th>
					<th class="px-3 py-2 text-left text-xs">Tagovi</th>
				</tr>
			</thead>
			<tbody>
				{#each objekti as obj (obj.idObjekta)}
					<tr class="border-t hover:bg-gray-50">
						<td class="text-medium px-3 py-2 text-xs font-bold whitespace-nowrap">{obj.naziv}</td>
						<td class="max-w-[200px] px-3 py-2 text-xs">{obj.opis}</td>
						<td class="px-2 py-2 text-xs whitespace-nowrap">{obj.tipObjekta?.opis ?? '-'}</td>
						<td class="px-2 py-2 text-center text-xs">{obj.cjenovniRang ?? '-'}</td>
						<td class="px-2 py-2 text-center text-xs">{obj.prosjecnaOcjena ?? '-'}</td>
						<td class="px-3 py-2 text-xs whitespace-nowrap">{obj.vlasnik ?? '-'}</td>
						<td class="px-2 py-2 text-center text-xs">{obj.dostupnaDostava ? 'Da' : 'Ne'}</td>
						<td class="px-3 py-2 text-xs">
							{#if obj.kontakt}
								<div class="min-w-[80px] space-y-1">
									{#if obj.kontakt.webStranica}
										<div>
											<a
												href={obj.kontakt.webStranica}
												target="_blank"
												rel="noopener noreferrer"
												class="text-blue-600 hover:underline"
											>
												Web
											</a>
										</div>
									{/if}
									{#if obj.kontakt.kontaktBroj}
										<div class="whitespace-nowrap">{obj.kontakt.kontaktBroj}</div>
									{/if}
									{#if obj.kontakt.email}
										<div class="break-all">{obj.kontakt.email}</div>
									{/if}
								</div>
							{:else}
								-
							{/if}
						</td>
						<td class="px-3 py-2 text-xs">
							{#if obj.lokacije && obj.lokacije.length > 0}
								<div class="max-w-[250px] space-y-2">
									{#each obj.lokacije as lok}
										<div class="border-b pb-2 last:border-b-0 last:pb-0">
											<div>{lok.ulica ?? ''} {lok.kucniBroj ?? ''}, {lok.grad ?? '-'}</div>
											{#if lok.radnoVrijeme}
												<div class="text-gray-600">{lok.radnoVrijeme}</div>
											{/if}
											{#if lok.latitude && lok.longitude}
												<div class="text-[10px] text-gray-500">{lok.latitude}, {lok.longitude}</div>
											{/if}
										</div>
									{/each}
								</div>
							{:else}
								-
							{/if}
						</td>
						<td class="w-[150px] px-3 py-2 text-xs">
							{#if obj.objektTagovi && obj.objektTagovi.length > 0}
								<div class="break-words">
									{#each obj.objektTagovi as ot, i}
										{#if ot.tag}
											{ot.tag.naziv}{#if i < obj.objektTagovi.length - 1},
											{/if}
										{/if}
									{/each}
								</div>
							{:else}
								-
							{/if}
						</td>
					</tr>
				{/each}
			</tbody>
		</table>
	</div>
{/if}
