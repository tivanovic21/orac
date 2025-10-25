-- csv
COPY (
  SELECT 
    o.id_objekta,
    o.naziv,
    t.opis AS tip_objekta,
    o.opis,
    o.prosjecna_ocjena,
    o.cjenovni_rang,
    o.vlasnik,
    o.dostupna_dostava,
    o.datum_unosa,
    l.ulica,
    l.kucni_broj,
    l.grad,
    l.latitude,
    l.longitude,
    l.radno_vrijeme,
    k.web_stranica,
    k.kontakt_broj,
    k.email,
    STRING_AGG(DISTINCT tg.naziv, '|' ORDER BY tg.naziv) AS tagovi
  FROM objekt o
  JOIN tip_objekta t ON o.tip_objekta_id = t.id_tipa
  LEFT JOIN kontakt k ON o.kontakt_id = k.id_kontakta
  JOIN lokacija l ON o.id_objekta = l.objekt_id
  LEFT JOIN objekt_tag ot ON o.id_objekta = ot.objekt_id
  LEFT JOIN tag tg ON ot.tag_id = tg.id_taga
  GROUP BY 
    o.id_objekta, o.naziv, t.opis, o.opis, o.prosjecna_ocjena, 
    o.cjenovni_rang, o.vlasnik, o.dostupna_dostava, o.datum_unosa,
    l.id_lokacije, l.ulica, l.kucni_broj, l.grad, l.latitude, 
    l.longitude, l.radno_vrijeme, k.id_kontakta, k.web_stranica, 
    k.kontakt_broj, k.email
  ORDER BY o.id_objekta, l.id_lokacije
) TO '/tmp/kafici_restorani.csv' WITH (FORMAT CSV, HEADER true, ENCODING 'UTF8');

-- json
COPY (
  SELECT json_agg(
    json_build_object(
      'id_objekta', o.id_objekta,
      'naziv', o.naziv,
      'tip_objekta', t.opis,
      'opis', o.opis,
      'prosjecna_ocjena', o.prosjecna_ocjena,
      'cjenovni_rang', o.cjenovni_rang,
      'vlasnik', o.vlasnik,
      'dostupna_dostava', o.dostupna_dostava,
      'datum_unosa', o.datum_unosa,
      'kontakt', (
        SELECT json_build_object(
          'web_stranica', k.web_stranica,
          'kontakt_broj', k.kontakt_broj,
          'email', k.email
        )
        FROM kontakt k
        WHERE k.id_kontakta = o.kontakt_id
      ),
      'lokacije', (
        SELECT json_agg(
          json_build_object(
            'ulica', l.ulica,
            'kucni_broj', l.kucni_broj,
            'grad', l.grad,
            'latitude', l.latitude,
            'longitude', l.longitude,
            'radno_vrijeme', l.radno_vrijeme
          )
        )
        FROM lokacija l
        WHERE l.objekt_id = o.id_objekta
      ),
      'tagovi', (
        SELECT json_agg(
          json_build_object(
            'naziv', tg.naziv,
            'opis', tg.opis
          )
        )
        FROM objekt_tag ot
        JOIN tag tg ON ot.tag_id = tg.id_taga
        WHERE ot.objekt_id = o.id_objekta
      )
    )
  )
  FROM objekt o
  JOIN tip_objekta t ON o.tip_objekta_id = t.id_tipa
) TO '/tmp/kafici_restorani.json';

-- običan backup:
-- pg_dump -U postgres -d orac -F p -f /tmp/kafici_restorani_db_dump.sql


-- compressed backup:
-- pg_dump -U postgres -d orac -F c -f /tmp/kafici_restorani_db_dump.backup

-- restore:
-- psql -U postgres -d orac -f /tmp/kafici_restorani_db_dump.sql
-- ili
-- pg_restore -U postgres -d orac /tmp/kafici_restorani_db_dump.backup