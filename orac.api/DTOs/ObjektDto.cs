using System.Text.Json.Serialization;

namespace orac.api.DTOS
{
    public class ObjektDto
    {
        [JsonPropertyName("@context")]
        public string? Context { get; set; } = "https://schema.org";
        
        [JsonPropertyName("@type")]
        public string? Type { get; set; } = "Restaurant";
        
        [JsonPropertyName("@id")]
        public string? Id { get; set; }

        public int IdObjekta { get; set; }
        public int TipObjektaId { get; set; }
        public int KontaktId { get; set; }

        [JsonPropertyName("name")]
        public string Naziv { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string? Opis { get; set; }

        [JsonPropertyName("aggregateRating")]
        public decimal? ProsjecnaOcjena { get; set; }

        [JsonPropertyName("priceRange")]
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