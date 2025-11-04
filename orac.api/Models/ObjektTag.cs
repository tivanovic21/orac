using System.ComponentModel.DataAnnotations.Schema;

namespace orac.api.Models
{
    [Table("objekt_tag")]
    public class ObjektTag
    {
        [Column("objekt_id")]
        public int ObjektId { get; set; }

        [Column("tag_id")]
        public int TagId { get; set; }

        [Column("datum_dodavanja")]
        public DateTime DatumDodavanja { get; set; }

        public Objekt? Objekt { get; set; }
        public Tag? Tag { get; set; }
    }
}
