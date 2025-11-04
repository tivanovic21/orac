using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace orac.api.Models
{
    [Table("lokacija")]
    public class Lokacija
    {
        [Key]
        [Column("id_lokacije")]
        public int IdLokacije { get; set; }

        [Required]
        [Column("objekt_id")]
        public int ObjektId { get; set; }

        [Required]
        [Column("ulica")]
        public string Ulica { get; set; } = string.Empty;

        [Column("kucni_broj")]
        public string? KucniBroj { get; set; }

        [Required]
        [Column("grad")]
        public string Grad { get; set; } = string.Empty;

        [Column("latitude")]
        public decimal? Latitude { get; set; }

        [Column("longitude")]
        public decimal? Longitude { get; set; }

        [Column("radno_vrijeme")]
        public string? RadnoVrijeme { get; set; }

        public Objekt? Objekt { get; set; }
    }
}
