using Microsoft.EntityFrameworkCore;
using orac.api.Models;

namespace orac.api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        public DbSet<Objekt> Objekti { get; set; }
        public DbSet<Kontakt> Kontakti { get; set; }
        public DbSet<Lokacija> Lokacije { get; set; }
        public DbSet<Tag> Tagovi { get; set; }
        public DbSet<TipObjekta> TipoviObjekta { get; set; }
        public DbSet<ObjektTag> ObjektTagovi { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ObjektTag>()
                .HasKey(ot => new { ot.ObjektId, ot.TagId });

            modelBuilder.Entity<ObjektTag>()
                .HasOne(ot => ot.Objekt)
                .WithMany(o => o.ObjektTagovi)
                .HasForeignKey(ot => ot.ObjektId);

            modelBuilder.Entity<ObjektTag>()
                .HasOne(ot => ot.Tag)
                .WithMany(t => t.ObjektTagovi)
                .HasForeignKey(ot => ot.TagId);
        }
    }
}
