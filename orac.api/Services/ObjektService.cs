using Microsoft.EntityFrameworkCore;
using orac.api.Data;
using orac.api.DTOS;
using orac.api.Extensions;
using orac.api.Interfaces;
using System.Text;
using System.Text.Json;

namespace orac.api.Services
{
    public class ObjektService : IObjektService
    {
        private readonly AppDbContext _context;

        public ObjektService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ObjektDto>> GetAllAsync()
        {
            var objekti = await _context.Objekti
                .Include(o => o.TipObjekta)
                .Include(o => o.Kontakt)
                .Include(o => o.Lokacije)
                .Include(o => o.ObjektTagovi)
                    .ThenInclude(ot => ot.Tag)
                .ToListAsync();

            return objekti.ToDtoList();
        }

        public async Task<IEnumerable<ObjektDto>> GetFilteredAsync(string searchTerm, string searchField)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await GetAllAsync();
            }

            var query = _context.Objekti
                .Include(o => o.TipObjekta)
                .Include(o => o.Kontakt)
                .Include(o => o.Lokacije)
                .Include(o => o.ObjektTagovi)
                    .ThenInclude(ot => ot.Tag)
                .AsQueryable();

            var lowerSearchTerm = searchTerm.ToLower();

            query = searchField.ToLower() switch
            {
                "naziv" => query.Where(o => o.Naziv.ToLower().Contains(lowerSearchTerm)),
                "opis" => query.Where(o => o.Opis != null && o.Opis.ToLower().Contains(lowerSearchTerm)),
                "vlasnik" => query.Where(o => o.Vlasnik != null && o.Vlasnik.ToLower().Contains(lowerSearchTerm)),
                "tipobjekta" => query.Where(o => o.TipObjekta != null && o.TipObjekta.Opis.ToLower().Contains(lowerSearchTerm)),
                "cjenovnirang" => query.Where(o => o.CjenovniRang != null && o.CjenovniRang.ToLower().Contains(lowerSearchTerm)),
                "grad" => query.Where(o => o.Lokacije.Any(l => l.Grad != null && l.Grad.ToLower().Contains(lowerSearchTerm))),
                "ulica" => query.Where(o => o.Lokacije.Any(l => l.Ulica != null && l.Ulica.ToLower().Contains(lowerSearchTerm))),
                "tag" => query.Where(o => o.ObjektTagovi.Any(ot => ot.Tag != null && ot.Tag.Naziv.ToLower().Contains(lowerSearchTerm))),
                _ => query.Where(o =>
                    o.Naziv.ToLower().Contains(lowerSearchTerm) ||
                    (o.Opis != null && o.Opis.ToLower().Contains(lowerSearchTerm)) ||
                    (o.Vlasnik != null && o.Vlasnik.ToLower().Contains(lowerSearchTerm)) ||
                    (o.TipObjekta != null && o.TipObjekta.Opis.ToLower().Contains(lowerSearchTerm)) ||
                    (o.CjenovniRang != null && o.CjenovniRang.ToLower().Contains(lowerSearchTerm)) ||
                    o.Lokacije.Any(l =>
                        (l.Grad != null && l.Grad.ToLower().Contains(lowerSearchTerm)) ||
                        (l.Ulica != null && l.Ulica.ToLower().Contains(lowerSearchTerm))) ||
                    o.ObjektTagovi.Any(ot => ot.Tag != null && ot.Tag.Naziv.ToLower().Contains(lowerSearchTerm)))
            };

            var objekti = await query.ToListAsync();
            return objekti.ToDtoList();
        }

        public Task<string> ExportToCsvFromDataAsync(IEnumerable<ObjektDto> objekti)
        {
            var sb = new StringBuilder();
            sb.AppendLine("id_objekta,naziv,tip_objekta,opis,prosjecna_ocjena,cjenovni_rang,vlasnik,dostupna_dostava,datum_unosa,ulica,kucni_broj,grad,latitude,longitude,radno_vrijeme,web_stranica,kontakt_broj,email,tagovi");

            foreach (var obj in objekti)
            {
                if (obj.Lokacije == null || !obj.Lokacije.Any())
                {
                    sb.AppendLine(FormatCsvRow(obj, null));
                }
                else
                {
                    foreach (var lok in obj.Lokacije)
                    {
                        sb.AppendLine(FormatCsvRow(obj, lok));
                    }
                }
            }

            return Task.FromResult(sb.ToString());
        }

        public Task<string> ExportToJsonFromDataAsync(IEnumerable<ObjektDto> objekti)
        {
            var exportList = objekti.Select(obj => new
            {
                id_objekta = obj.IdObjekta,
                naziv = obj.Naziv,
                tip_objekta = obj.TipObjekta?.Opis,
                opis = obj.Opis,
                prosjecna_ocjena = obj.ProsjecnaOcjena,
                cjenovni_rang = obj.CjenovniRang,
                vlasnik = obj.Vlasnik,
                dostupna_dostava = obj.DostupnaDostava,
                datum_unosa = obj.DatumUnosa,
                kontakt = obj.Kontakt != null ? new
                {
                    web_stranica = obj.Kontakt.WebStranica,
                    kontakt_broj = obj.Kontakt.KontaktBroj,
                    email = obj.Kontakt.Email
                } : null,
                lokacije = obj.Lokacije?.Select(l => new
                {
                    ulica = l.Ulica,
                    kucni_broj = l.KucniBroj,
                    grad = l.Grad,
                    latitude = l.Latitude,
                    longitude = l.Longitude,
                    radno_vrijeme = l.RadnoVrijeme
                }).ToList(),
                tagovi = obj.ObjektTagovi?.Select(ot => new
                {
                    naziv = ot.Tag?.Naziv,
                    opis = ot.Tag?.Opis
                }).ToList()
            }).ToList();

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = null,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            return Task.FromResult(JsonSerializer.Serialize(exportList, options));
        }

        private string FormatCsvRow(ObjektDto obj, LokacijaDto? lok)
        {
            var tagovi = obj.ObjektTagovi != null && obj.ObjektTagovi.Any()
                ? "{" + string.Join(",", obj.ObjektTagovi.Select(ot => ot.Tag?.Naziv).Where(n => n != null)) + "}"
                : "{}";

            return string.Join(",",
                obj.IdObjekta,
                EscapeCsv(obj.Naziv),
                EscapeCsv(obj.TipObjekta?.Opis ?? ""),
                EscapeCsv(obj.Opis ?? ""),
                obj.ProsjecnaOcjena?.ToString("F2", System.Globalization.CultureInfo.InvariantCulture) ?? "",
                EscapeCsv(obj.CjenovniRang ?? ""),
                EscapeCsv(obj.Vlasnik ?? ""),
                obj.DostupnaDostava ? "t" : "f",
                obj.DatumUnosa.ToString("yyyy-MM-dd HH:mm:ss.ffffff"),
                EscapeCsv(lok?.Ulica ?? ""),
                EscapeCsv(lok?.KucniBroj ?? ""),
                EscapeCsv(lok?.Grad ?? ""),
                lok?.Latitude?.ToString("F8", System.Globalization.CultureInfo.InvariantCulture) ?? "",
                lok?.Longitude?.ToString("F8", System.Globalization.CultureInfo.InvariantCulture) ?? "",
                EscapeCsv(lok?.RadnoVrijeme ?? ""),
                EscapeCsv(obj.Kontakt?.WebStranica ?? ""),
                EscapeCsv(obj.Kontakt?.KontaktBroj ?? ""),
                EscapeCsv(obj.Kontakt?.Email ?? ""),
                tagovi
            );
        }

        private string EscapeCsv(string value)
        {
            if (string.IsNullOrEmpty(value))
                return "";

            if (value.Contains(",") || value.Contains("\"") || value.Contains("\n"))
            {
                return "\"" + value.Replace("\"", "\"\"") + "\"";
            }

            return value;
        }
    }
}