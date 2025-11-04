#!/bin/bash

DB_NAME="orac"
DB_USER="postgres"
DB_PASSWORD="mojPwd"
DB_HOST="localhost"
DB_PORT="5432"
OUTPUT_DIR="./exports"
TIMESTAMP=$(date +%Y%m%d_%H%M%S)

export PGPASSWORD="$DB_PASSWORD"

mkdir -p $OUTPUT_DIR
echo "Kreiran direktorij: $OUTPUT_DIR"

echo ""
echo "[1/3] Export u CSV format..."

psql -U $DB_USER -h $DB_HOST -p $DB_PORT -d $DB_NAME -c "
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
" > $OUTPUT_DIR/kafici_restorani.csv

if [ $? -eq 0 ]; then
    echo "CSV export uspješan: $OUTPUT_DIR/kafici_restorani.csv"
else
    echo "Greška pri CSV exportu"
    exit 1
fi

echo ""
echo "[2/3] Export u JSON format..."

psql -U $DB_USER -h $DB_HOST -p $DB_PORT -d $DB_NAME -t -A -c "
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
" > $OUTPUT_DIR/kafici_restorani.json

if [ $? -eq 0 ]; then
    echo "JSON export uspješan: $OUTPUT_DIR/kafici_restorani.json"
else
    echo "Greška pri JSON exportu"
    exit 1
fi

echo ""
echo "[3/3] Database dump..."

pg_dump -U $DB_USER -h $DB_HOST -p $DB_PORT -d $DB_NAME -F p -f $OUTPUT_DIR/kafici_db_dump_$TIMESTAMP.sql

if [ $? -eq 0 ]; then
    echo "Database dump uspješan: $OUTPUT_DIR/kafici_db_dump_$TIMESTAMP.sql"
