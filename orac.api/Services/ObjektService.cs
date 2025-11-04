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
    }
}