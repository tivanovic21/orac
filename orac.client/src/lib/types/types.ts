export interface TipObjekta {
  idTipa: number;
  opis: string;
}

export interface Kontakt {
  idKontakta: number;
  webStranica?: string | null;
  kontaktBroj?: string | null;
  email?: string | null;
}

export interface Tag {
  idTaga: number;
  naziv: string;
  opis?: string | null;
}

export interface ObjektTag {
  objektId: number;
  tagId: number;
  datumDodavanja: Date;
  tag?: Tag | null;
}

export interface Lokacija {
  idLokacije: number;
  objektId: number;
  ulica: string;
  kucniBroj?: string | null;
  grad: string;
  latitude?: number | null;
  longitude?: number | null;
  radnoVrijeme?: string | null;
}

export interface Objekt {
  idObjekta: number;
  tipObjektaId: number;
  kontaktId: number;
  naziv: string;
  opis?: string | null;
  prosjecnaOcjena?: number | null;
  cjenovniRang?: string | null;
  vlasnik?: string | null;
  dostupnaDostava: boolean;
  datumUnosa: Date;

  tipObjekta?: TipObjekta | null;
  kontakt?: Kontakt | null;
  lokacije: Lokacija[];
  objektTagovi: ObjektTag[];
}

export interface ApiResponse<T> {
  status: string;
  message: string;
  response: T;
}