using Microsoft.EntityFrameworkCore;
using orac.api.Data;
using orac.api.DTOS;
using orac.api.Extensions;
using orac.api.Interfaces;
using orac.api.Models;

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
    }
}