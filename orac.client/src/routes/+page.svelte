<script lang="ts">
    import { onMount } from "svelte";
    import { getObjekti } from "$lib/api";
    import type { Objekt } from "$lib/types/types";

    let objekti: Objekt[] = [];
    let err: string | null = null;
    let loading = true;

    onMount(async () => {
        try {
            objekti = await getObjekti();
        } catch (e) {
            err = e instanceof Error ? e.message : String(e);
        } finally {
            loading = false;
        }
    });
</script>

<h1>Objekti</h1>

{#if loading}
    <p>Loading...</p>
{:else if err}
    <p style="color:red">{err}</p>
{:else if objekti.length === 0}
    <p>No objekti found.</p>
{:else}
    <table>
        <thead>
            <tr>
                <th>Id</th>
                <th>Naziv</th>
                <th>Opis</th>
                <th>TipObjekta</th>
                <th>Kontakt</th>
                <th>Lokacije</th>
                <th>ObjektTagovi</th>
                <th>Prosjecna Ocjena</th>
                <th>Cjenovni Rang</th>
                <th>Vlasnik</th>
                <th>Dostupna Dostava</th>
                <th>Datum Unosa</th>
            </tr>
        </thead>
        <tbody>
            {#each objekti as obj}
                <tr>
                    <td>{obj.idObjekta}</td>
                    <td>{obj.naziv}</td>
                    <td>{obj.opis}</td>
                    <td>
                        {#if obj.tipObjekta}
                            ID: {obj.tipObjekta.idTipa}, {obj.tipObjekta.opis}
                        {:else}
                            -
                        {/if}
                    </td>
                    <td>
                        {#if obj.kontakt}
                            <div>ID: {obj.kontakt.idKontakta}</div>
                            <div>Web: {obj.kontakt.webStranica ?? '-'}</div>
                            <div>Broj: {obj.kontakt.kontaktBroj ?? '-'}</div>
                            <div>Email: {obj.kontakt.email ?? '-'}</div>
                        {:else}
                            -
                        {/if}
                    </td>
                    <td>
                        {#if obj.lokacije.length > 0}
                            <ul>
                                {#each obj.lokacije as loc}
                                    <li>
                                        ID: {loc.idLokacije ?? '-'}, {loc.ulica ?? '-'}, 
                                        {loc.grad ?? '-'}, {loc.radnoVrijeme ?? '-'}
                                        LAT: {loc.latitude ?? '-'}, LNG: {loc.longitude ?? '-'}
                                    </li>
                                {/each}
                            </ul>
                        {:else}
                            -
                        {/if}
                    </td>
                    <td>
                        {#if obj.objektTagovi.length > 0}
                            <ul>
                                {#each obj.objektTagovi as ot}
                                    <li>{ot.tag?.naziv ?? '-'}</li>
                                {/each}
                            </ul>
                        {:else}
                            -
                        {/if}
                    </td>
                    <td>{obj.prosjecnaOcjena ?? '-'}</td>
                    <td>{obj.cjenovniRang ?? '-'}</td>
                    <td>{obj.vlasnik ?? '-'}</td>
                    <td>{obj.dostupnaDostava ? 'Da' : 'Ne'}</td>
                    <td>{new Date(obj.datumUnosa).toLocaleDateString()}</td>
                </tr>
            {/each}
        </tbody>
    </table>
{/if}

<style>
    table {
        width: 100%;
        border-collapse: collapse;
        margin-top: 1rem;
    }

    th, td {
        border: 1px solid #ccc;
        padding: 0.5rem;
        vertical-align: top;
        text-align: left;
    }

    th {
        background-color: #f0f0f0;
    }

    tr:nth-child(even) {
        background-color: #fafafa;
    }

    ul {
        padding-left: 1rem;
        margin: 0;
    }

    li {
        margin-bottom: 0.25rem;
    }
</style>
