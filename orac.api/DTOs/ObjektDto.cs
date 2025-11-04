namespace orac.api.DTOS
{
    public class ObjektDto
    {
        public int IdObjekta { get; set; }
        public int TipObjektaId { get; set; }
        public int KontaktId { get; set; }
        public string Naziv { get; set; } = string.Empty;
        public string? Opis { get; set; }
        public decimal? ProsjecnaOcjena { get; set; }
        public string? CjenovniRang { get; set; }
        public string? Vlasnik { get; set; }
        public bool DostupnaDostava { get; set; }
        public DateTime DatumUnosa { get; set; }

        public TipObjektaDto? TipObjekta { get; set; }
        public KontaktDto? Kontakt { get; set; }
        public List<LokacijaDto> Lokacije { get; set; } = new();
        public List<ObjektTagDto> ObjektTagovi { get; set; } = new();
    }
}