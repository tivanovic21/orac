namespace orac.api.DTOS
{
    public class TagDto
    {
        public int IdTag { get; set; }
        public string Naziv { get; set; } = string.Empty;
        public string? Opis { get; set; }
    }
}