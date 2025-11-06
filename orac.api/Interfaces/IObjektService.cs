using orac.api.DTOS;

namespace orac.api.Interfaces
{
    public interface IObjektService
    {
        Task<IEnumerable<ObjektDto>> GetAllAsync();
        Task<IEnumerable<ObjektDto>> GetFilteredAsync(
            string searchTerm, 
            string searchField,
            bool? dostupnaDostava = null,
            string? cjenovniRang = null,
            decimal? minOcjena = null,
            decimal? maxOcjena = null
        );
        Task<string> ExportToCsvFromDataAsync(IEnumerable<ObjektDto> objekti);
        Task<string> ExportToJsonFromDataAsync(IEnumerable<ObjektDto> objekti);
    }
}