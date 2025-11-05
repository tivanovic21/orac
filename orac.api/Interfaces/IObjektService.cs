using orac.api.DTOS;

namespace orac.api.Interfaces
{
    public interface IObjektService
    {
        Task<IEnumerable<ObjektDto>> GetAllAsync();
        Task<IEnumerable<ObjektDto>> GetFilteredAsync(string searchTerm, string searchField);
    }
}