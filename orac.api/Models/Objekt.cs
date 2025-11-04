using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace orac.api.Models
{
    [Table("objekt")]
    public class Objekt
    {
        [Key]
        [Column("id_objekta")]
        public int IdObjekta { get; set; }

        [Required]
        [Column("tip_objekta_id")]
        public int TipObjektaId { get; set; }

        [Required]
        [Column("kontakt_id")]
        public int KontaktId { get; set; }

        [Required]
        [Column("naziv")]
        public string Naziv { get; set; } = string.Empty;

        [Column("opis")]
        public string? Opis { get; set; }

        [Column("prosjecna_ocjena", TypeName = "numeric(3,2)")]
        public decimal? ProsjecnaOcjena { get; set; }

        [Column("cjenovni_rang")]
        public string? CjenovniRang { get; set; }

        [Column("vlasnik")]
        public string? Vlasnik { get; set; }

        [Column("dostupna_dostava")]
        public bool DostupnaDostava { get; set; }

        [Column("datum_unosa")]
        public DateTime DatumUnosa { get; set; }

        public TipObjekta? TipObjekta { get; set; }
        public Kontakt? Kontakt { get; set; }
        public ICollection<Lokacija> Lokacije { get; set; } = [];
        public ICollection<ObjektTag> ObjektTagovi { get; set; } = [];
    }
}
