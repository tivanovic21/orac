using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace orac.api.Models
{
    [Table("tag")]
    public class Tag
    {
        [Key]
        [Column("id_taga")]
        public int IdTaga { get; set; }

        [Required]
        [Column("naziv")]
        public string Naziv { get; set; } = string.Empty;

        [Column("opis")]
        public string? Opis { get; set; }

        public ICollection<ObjektTag> ObjektTagovi { get; set; } = new List<ObjektTag>();
    }
}
