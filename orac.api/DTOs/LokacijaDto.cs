namespace orac.api.DTOS
{
    public class LokacijaDto
    {
        public int IdLokacije { get; set; }
        public int ObjektId { get; set; }
        public string Ulica { get; set; } = string.Empty;
        public string? KucniBroj { get; set; }
        public string Grad { get; set; } = string.Empty;
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string? RadnoVrijeme { get; set; }
    }
}