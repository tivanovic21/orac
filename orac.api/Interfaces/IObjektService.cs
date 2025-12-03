using orac.api.DTOS;

namespace orac.api.Interfaces
{
    public interface IObjektService
    {
        Task<IEnumerable<ObjektDto>> GetAllAsync();
        Task<ObjektDto?> GetByIdAsync(int id);
        Task<bool> DeleteAsync(int id);
        Task<ObjektDto?> UpdateAsync(ObjektDto objektDto);
        Task<ObjektDto?> CreateAsync(ObjektDto objektDto);
        Task<IEnumerable<ObjektDto>> GetObjektiClosestToYouAsync(decimal latitude, decimal longitude, double? radiusInKm = null);
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