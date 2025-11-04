using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace orac.api.Models
{
    [Table("tip_objekta")]
    public class TipObjekta
    {
        [Key]
        [Column("id_tipa")]
        public int IdTipa { get; set; }

        [Required]
        [Column("opis")]
        public string Opis { get; set; } = string.Empty;

        public ICollection<Objekt> Objekti { get; set; } = [];
    }
}
