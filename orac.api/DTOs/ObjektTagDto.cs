namespace orac.api.DTOS
{
    public class ObjektTagDto
    {
        public int ObjektId { get; set; }
        public int TagId { get; set; }
        public DateTime DatumDodavanja { get; set; } 
        public TagDto Tag { get; set; } = null!;
    }
}