-- BRISANJE TABLICA AKO POSTOJE
DROP TABLE IF EXISTS objekt_tag CASCADE;
DROP TABLE IF EXISTS kontakt CASCADE;
DROP TABLE IF EXISTS lokacija CASCADE;
DROP TABLE IF EXISTS objekt CASCADE;
DROP TABLE IF EXISTS tag CASCADE;
DROP TABLE IF EXISTS tip_objekta CASCADE;

-- TIP_OBJEKTA
CREATE TABLE tip_objekta (
    id_tipa SERIAL PRIMARY KEY,
    opis VARCHAR(50) NOT NULL UNIQUE
);

-- TAG
CREATE TABLE tag (
    id_taga SERIAL PRIMARY KEY,
    naziv VARCHAR(50) NOT NULL UNIQUE,
    opis TEXT
);

-- OBJEKT
CREATE TABLE objekt (
    id_objekta SERIAL PRIMARY KEY,
    tip_objekta_id INTEGER NOT NULL,
    kontakt_id INTEGER NOT NULL,
    naziv VARCHAR(100) NOT NULL,
    opis TEXT,
    prosjecna_ocjena DECIMAL(3,2) CHECK (prosjecna_ocjena >= 0 AND prosjecna_ocjena <= 5),
    cjenovni_rang VARCHAR(3) CHECK (cjenovni_rang IN ('$', '$$', '$$$')),
    vlasnik VARCHAR(100),
    dostupna_dostava BOOLEAN DEFAULT FALSE,
    datum_unosa TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (tip_objekta_id) REFERENCES tip_objekta(id_tipa) ON DELETE CASCADE,
    FOREIGN KEY (kontakt_id) REFERENCES kontakt(id_kontakta) ON DELETE CASCADE
);

-- LOKACIJA
CREATE TABLE lokacija (
    id_lokacije SERIAL PRIMARY KEY,
    id_objekta INTEGER NOT NULL,
    ulica VARCHAR(100) NOT NULL,
    kucni_broj VARCHAR(10),
    grad VARCHAR(50) NOT NULL,
    latitude DECIMAL(10,8),
    longitude DECIMAL(11,8),
    radno_vrijeme VARCHAR(50),
    FOREIGN KEY (id_objekta) REFERENCES objekt(id_objekta) ON DELETE CASCADE
);

-- KONTAKT
CREATE TABLE kontakt (
    id_kontakta SERIAL PRIMARY KEY,
    web_stranica VARCHAR(255),
    kontakt_broj VARCHAR(20),
    email VARCHAR(100),
);

-- OBJEKT_TAG
CREATE TABLE objekt_tag (
    id_objekta INTEGER NOT NULL,
    id_taga INTEGER NOT NULL,
    datum_dodavanja TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (id_objekta, id_taga),
    FOREIGN KEY (id_objekta) REFERENCES objekt(id_objekta) ON DELETE CASCADE,
    FOREIGN KEY (id_taga) REFERENCES tag(id_taga) ON DELETE CASCADE
);

-- Tipovi objekata
INSERT INTO tip_objekta (opis) VALUES
    ('kafić'),
    ('restoran'),
    ('snack bar'),
    ('pizzeria'),
    ('slastičarna')

-- Tagovi
INSERT INTO tag (naziv, opis) VALUES
    ('WiFi', 'Besplatan bežični internet za goste'),
    ('Parking', 'Dostupan parking prostor'),
    ('Terasa', 'Vanjski prostor za sjedenje'),
    ('Vegan', 'Nudi veganske opcije jela i pića'),
    ('Vegetarijanski', 'Nudi vegetarijanske opcije'),
    ('Pet-friendly', 'Dopušten pristup s kućnim ljubimcima'),
    ('Klimatizacija', 'Klimatizirani unutarnji prostor'),
    ('Live glazba', 'Povremeni ili redoviti glazbeni nastupi'),
    ('Plaćanje karticom', 'Prihvaća plaćanje debitnim/kreditnim karticama'),
    ('Dostava', 'Nudi uslugu dostave hrane i pića')

/*
-- Primjer unosa kontakta
INSERT INTO kontakt (web_stranica, kontakt_broj, email)
VALUES (
    'https://coolcoffee.hr', 
    '+385912345678', 
    'info@coolcoffee.hr'
);

-- Primjer unosa objekta
INSERT INTO objekt (tip_objekta_id, kontakt_id, naziv, opis, prosjecna_ocjena, cjenovni_rang, vlasnik, dostupna_dostava)
VALUES (1, 1, 'Cool Coffee', 'Lanac modernih i cool kafića s izvrsnom kavom.', 4.6, '$$', 'Petar Marić', FALSE);

-- Primjer unosa lokacije (objekt_id mora postojati)
INSERT INTO lokacija (objekt_id, ulica, kucni_broj, grad, latitude, longitude, radno_vrijeme)
VALUES (2, 'Skalinska ulica', '5', 'Zagreb', 45.814936, 15.976858, '11:00–22:00');

-- Primjer povezivanja objekta s tagovima
INSERT INTO objekt_tag (objekt_id, tag_id)
VALUES (1, 1), (1, 3), (1, 4);
*/