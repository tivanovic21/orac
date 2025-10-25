# OTVORENO RAČUNARSTVO

## O PROJEKTU

| Podatak | Vrijednost |
|-------------|------------|
| **Licenca** | CC0 (Creative Commons Zero) |
| **Autor** | Toni Ivanović |
| **Verzija** | 1.0 |
| **Jezik podataka** | hrvatski |
| **Datum objave** | 2025-10-23 |
| **Prostorni obuhvat** | Republika Hrvatska |
| **Vremenski raspon** | listopad 2025 |
| **Format datoteka** | JSON, CSV |
| **Broj instanci** | 10+ objekata |
| **Učestalost ažuriranja** | jednokratno (v1.0) |
| **Izvori podataka** | Google Maps, Tripadvisor, CompanyWall, službene web stranice |

---

## O LICENCI

Sukladno sa CC0 (Creative Commons Zero) licencom, svi podaci u sklopu ovog projekta pripadaju javnom dobru. Dozvoljeno je slobodno i neograničeno preuzimanje, distribuiranje, umnažanje, uređivanje i uporaba podataka u sve svrhe.

---

## OPIS PODATAKA

Ovaj skup podataka opisuje **kafiće i restorane** na području Republike Hrvatske.  
Glavni izvori podataka su Google Maps (za geolokacijske informacije), Tripadvisor za preporuku podataka, CompanyWall za informacije o vlasnicima te službene web stranice kafića i restorana (za informacije o imenima, kontaktima i sl.).

Podaci su modelirani u **relacijsku bazu podataka** i organizirani u nekoliko povezanih tablica.  
Svaki *objekt* može imati više *lokacija* (poslovnica), više *kontaktnih metoda* te biti označen sa više *tagova* koji opisuju njegove karakteristike.

---

## STRUKTURA TABLICA

### **OBJEKT**

Predstavlja pojedini ugostiteljski objekt (kafić, restoran, fast-food, slastičarnicu i sl.).

| Atribut | Opis |
|----------|------|
| `id_objekta` | jedinstveni identifikator objekta (PK, INT) |
| `tip_objekta_id` | identifikator tipa objekta (FK, INT) |
| `kontakt_id` | identifikator kontaktnih podataka (FK, INT) |
| `naziv` | naziv objekta (VARCHAR) |
| `opis` | opis objekta (TEXT) |
| `prosjecna_ocjena` | prosječna ocjena objekta od strane korisnika (DECIMAL) |
| `cjenovni_rang` | cjenovni rang objekta u obliku enumeracije \$, \$\$, \$\$\$ (VARCHAR) |
| `vlasnik` | vlasnik objekta (VARCHAR) |
| `dostupna_dostava` | označava ima li objekt mogućnost dostave (BOOLEAN) |
| `datum_unosa` | datum i vrijeme unosa podatka u bazu (DATETIME) |

---

### **TIP OBJEKTA**

Predstavlja enumeraciju vrsta ugostiteljskih objekata.  
Omogućuje jednostavno proširenje skupa podataka za nove tipove poput slastičarnica, pizzeria ili pekarnica.

| Atribut | Opis |
|----------|------|
| `id_tipa` | jedinstveni identifikator tipa (PK, INT) |
| `opis` | opis tipa objekta (VARCHAR) |

---

### **LOKACIJA**

Lokacija je izdvojena u posebnu tablicu jer jedan objekt može imati više fizičkih lokacija s različitim adresama, koordinatama i radnim vremenom.

| Atribut | Opis |
|----------|------|
| `id_lokacije` | jedinstveni identifikator lokacije (PK, INT) |
| `objekt_id` | identifikator objekta kojem lokacija pripada (FK, INT) |
| `ulica` | naziv ulice (VARCHAR) |
| `kucni_broj` | kućni broj (VARCHAR) |
| `grad` | grad u kojem se nalazi objekt (VARCHAR) |
| `latitude` | geografska širina (DECIMAL) |
| `longitude` | geografska dužina (DECIMAL) |
| `radno_vrijeme` | radno vrijeme objekta (VARCHAR) |

---

### **KONTAKT**

Jedan objekt može imati više načina kontaktiranja (telefonski broj, web stranica, e-mail i sl.), pa su ti podaci izdvojeni u zasebnu tablicu.

| Atribut | Opis |
|----------|------|
| `id_kontakta` | jedinstveni identifikator kontakta (PK, INT) |
| `web_stranica` | poveznica na web stranicu objekta (VARCHAR) |
| `kontakt_broj` | telefonski broj za kontakt (VARCHAR) |
| `email` | email za kontakt (VARCHAR) |

---

### **TAG**

Tablica tagova sadrži standardizirane oznake koje opisuju karakteristike objekata (npr. veganski meni, parking, WiFi, terasa).

| Atribut | Opis |
|----------|------|
| `id_taga` | jedinstveni identifikator taga (PK, INT) |
| `naziv` | naziv taga (VARCHAR) |
| `opis` | opis taga (VARCHAR) |

---

### **OBJEKT_TAG**

Povezna tablica za many-to-many vezu između objekata i tagova. Jedan objekt može imati više tagova, a isti tag može biti pridružen više objekata.

| Atribut | Opis |
|----------|------|
| `objekt_id` | jedinstveni identifikator objekta (FK, INT) |
| `tag_id` | jedinstveni identifikator taga (FK, INT) |
| `datum_dodavanja` | datum kada je tag pridružen objektu (DATETIME) |

Primarni ključ načinjen je kao kompozitni ključ dvaju vanjskih ključeva. 

---

## PRIMJER JSON ZAPISA

```json
{
  "id_objekta": 1,
  "naziv": "Cool Coffee",
  "tip_objekta": "kafić",
  "opis": "Lanac modernih i cool kafića s izvrsnom kavom.",
  "prosjecna_ocjena": 4.6,
  "cjenovni_rang": "$$",
  "vlasnik": "Petar Marić",
  "dostupna_dostava": false,
  "datum_unosa": "2025-10-22T14:30:00",
  "kontakti": [
    {
      "web_stranica": "https://coolcoffee.hr",
      "kontakt_broj": "+385912345678",
      "email": "info@coolcoffee.hr"
    },
    {
      "web_stranica": "https://facebook.com/coolcoffee",
      "kontakt_broj": "+385915551111",
      "email": "social@coolcoffee.hr"
    }
  ],
  "lokacije": [
    {
      "ulica": "Ilica",
      "kucni_broj": "45",
      "grad": "Zagreb",
      "latitude": 45.8123,
      "longitude": 15.9775,
      "radno_vrijeme": "07:00–22:00"
    },
    {
      "ulica": "Obala kneza Branimira",
      "kucni_broj": "12",
      "grad": "Split",
      "latitude": 43.5081,
      "longitude": 16.4402,
      "radno_vrijeme": "08:00–23:00"
    }
  ],
  "tagovi": [
    {
      "naziv": "WiFi",
      "opis": "Besplatan bežični internet"
    },
    {
      "naziv": "Terasa",
      "opis": "Vanjski prostor za sjedenje"
    },
    {
      "naziv": "Vegan",
      "opis": "Nudi veganske opcije"
    }
  ]
}
```

## PRIMJER CSV ZAPISA
```csv
id_objekta,naziv,tip_objekta,opis,prosjecna_ocjena,cjenovni_rang,vlasnik,dostupna_dostava,datum_unosa,ulica,kucni_broj,grad,latitude,longitude,radno_vrijeme,web_stranica,kontakt_broj,email,tagovi
1,Cool Coffee,kafić,"Lanac modernih i cool kafića s izvrsnom kavom.",4.6,$$,Petar Marić,false,2025-10-22T14:30:00,Ilica,45,Zagreb,45.8123,15.9775,07:00–22:00,https://coolcoffee.hr,+385912345678,info@coolcoffee.hr,"WiFi|Terasa|Vegan"
1,Cool Coffee,kafić,"Lanac modernih i cool kafića s izvrsnom kavom.",4.6,$$,Petar Marić,false,2025-10-22T14:30:00,Obala kneza Branimira,12,Split,43.5081,16.4402,08:00–23:00,https://coolcoffee.hr,+385912345678,info@coolcoffee.hr,"WiFi|Terasa|Vegan"
2,Pizza Palace,restoran,"Autentična talijanska pizzeria.",4.8,$$$,Ana Horvat,true,2025-10-22T15:00:00,Trg bana Jelačića,1,Zagreb,45.8131,15.9775,10:00–23:00,https://pizzapalace.hr,+385911111111,kontakt@pizzapalace.hr,"Parkiranje|Dječji kutak|Dostupno OsI"
3,Vege Bistro,kafić,"100% veganski restoran.",4.9,$$,Marko Novak,true,2025-10-22T16:00:00,Savska cesta,50,Zagreb,45.8050,15.9700,09:00–21:00,https://vegebistro.hr,+385922222222,hello@vegebistro.hr,"WiFi|Vegan|Terasa|Pet-friendly"
```