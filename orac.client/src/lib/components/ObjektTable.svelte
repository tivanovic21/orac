<script lang="ts">
  import type { Objekt } from '$lib/types/types';

  export let objekti: Objekt[] = [];
</script>

{#if objekti.length === 0}
  <div class="text-center py-8 text-gray-500">
    <p>Nema rezultata za prikazati</p>
  </div>
{:else}
  <div class="overflow-x-auto border">
    <table class="min-w-full bg-white table-auto">
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
            <td class="px-3 py-2 text-xs font-bold text-medium whitespace-nowrap">{obj.naziv}</td>
            <td class="px-3 py-2 text-xs max-w-[200px]">{obj.opis}</td>
            <td class="px-2 py-2 text-xs whitespace-nowrap">{obj.tipObjekta?.opis ?? '-'}</td>
            <td class="px-2 py-2 text-xs text-center">{obj.cjenovniRang ?? '-'}</td>
            <td class="px-2 py-2 text-xs text-center">{obj.prosjecnaOcjena ?? '-'}</td>
            <td class="px-3 py-2 text-xs whitespace-nowrap">{obj.vlasnik ?? '-'}</td>
            <td class="px-2 py-2 text-xs text-center">{obj.dostupnaDostava ? 'Da' : 'Ne'}</td>
            <td class="px-3 py-2 text-xs">
              {#if obj.kontakt}
                <div class="space-y-1 min-w-[80px]">
                  {#if obj.kontakt.webStranica}
                    <div>
                      <a href={obj.kontakt.webStranica} target="_blank" rel="noopener noreferrer" class="text-blue-600 hover:underline">
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
                <div class="space-y-2 max-w-[250px]">
                  {#each obj.lokacije as lok}
                    <div class="pb-2 border-b last:border-b-0 last:pb-0">
                      <div>{lok.ulica ?? ''} {lok.kucniBroj ?? ''}, {lok.grad ?? '-'}</div>
                      {#if lok.radnoVrijeme}
                        <div class="text-gray-600">{lok.radnoVrijeme}</div>
                      {/if}
                      {#if lok.latitude && lok.longitude}
                        <div class="text-gray-500 text-[10px]">{lok.latitude}, {lok.longitude}</div>
                      {/if}
                    </div>
                  {/each}
                </div>
              {:else}
                -
              {/if}
            </td>
            <td class="px-3 py-2 text-xs w-[150px]">
              {#if obj.objektTagovi && obj.objektTagovi.length > 0}
                <div class="break-words">
                  {#each obj.objektTagovi as ot, i}
                    {#if ot.tag}
                      {ot.tag.naziv}{#if i < obj.objektTagovi.length - 1}, {/if}
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
