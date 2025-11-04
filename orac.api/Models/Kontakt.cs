using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace orac.api.Models
{
    [Table("kontakt")]
    public class Kontakt
    {
        [Key]
        [Column("id_kontakta")]
        public int IdKontakta { get; set; }

        [Column("web_stranica")]
        public string? WebStranica { get; set; }

        [Column("kontakt_broj")]
        public string? KontaktBroj { get; set; }

        [Column("email")]
        public string? Email { get; set; }

        public ICollection<Objekt> Objekti { get; set; } = [];
    }
}
