using orac.api.DTOS;
using orac.api.Models;

namespace orac.api.Extensions
{
    public static class ObjektMapper
    {
        public static ObjektDto ToDto(this Objekt o)
        {
            if (o == null) return null!;

            var dto = new ObjektDto
            {
                IdObjekta = o.IdObjekta,
                TipObjektaId = o.TipObjektaId,
                KontaktId = o.KontaktId,
                Naziv = o.Naziv,
                Opis = o.Opis,
                ProsjecnaOcjena = o.ProsjecnaOcjena,
                CjenovniRang = o.CjenovniRang,
                Vlasnik = o.Vlasnik,
                DostupnaDostava = o.DostupnaDostava,
                DatumUnosa = o.DatumUnosa
            };

            if (o.TipObjekta != null)
            {
                dto.TipObjekta = new TipObjektaDto
                {
                    IdTipa = o.TipObjekta.IdTipa,
                    Opis = o.TipObjekta.Opis
                };
            }

            if (o.Kontakt != null)
            {
                dto.Kontakt = new KontaktDto
                {
                    IdKontakta = o.Kontakt.IdKontakta,
                    WebStranica = o.Kontakt.WebStranica,
                    KontaktBroj = o.Kontakt.KontaktBroj,
                    Email = o.Kontakt.Email
                };
            }

            if (o.Lokacije != null)
            {
                dto.Lokacije = o.Lokacije.Select(l => new LokacijaDto
                {
                    IdLokacije = l.IdLokacije,
                    ObjektId = l.ObjektId,
                    Ulica = l.Ulica,
                    KucniBroj = l.KucniBroj,
                    Grad = l.Grad,
                    Latitude = l.Latitude,
                    Longitude = l.Longitude,
                    RadnoVrijeme = l.RadnoVrijeme
                }).ToList();
            }

            if (o.ObjektTagovi != null)
            {
                dto.ObjektTagovi = o.ObjektTagovi.Select(ot => new ObjektTagDto
                {
                    ObjektId = ot.ObjektId,
                    TagId = ot.TagId,
                    DatumDodavanja = ot.DatumDodavanja,
                    Tag = ot.Tag == null ? null! : new TagDto
                    {
                        IdTag = ot.Tag.IdTaga,
                        Naziv = ot.Tag.Naziv,
                        Opis = ot.Tag.Opis
                    }
                }).ToList();
            }

            return dto;
        }

        public static IEnumerable<ObjektDto> ToDtoList(this IEnumerable<Objekt> list)
            => list?.Select(o => o.ToDto()) ?? Enumerable.Empty<ObjektDto>();
    }
}