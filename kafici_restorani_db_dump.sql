--
-- PostgreSQL database dump
--

\restrict fkbgQWQDWaewQpLniye8yUb5JHAt25q9j5XeLZboZNMBMsCiorn4Ea323mCBjv1

-- Dumped from database version 17.6 (Homebrew)
-- Dumped by pg_dump version 17.6 (Homebrew)

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: kontakt; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.kontakt (
    id_kontakta integer NOT NULL,
    web_stranica character varying(255),
    kontakt_broj character varying(20),
    email character varying(100)
);


ALTER TABLE public.kontakt OWNER TO postgres;

--
-- Name: kontakt_id_kontakta_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.kontakt_id_kontakta_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.kontakt_id_kontakta_seq OWNER TO postgres;

--
-- Name: kontakt_id_kontakta_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.kontakt_id_kontakta_seq OWNED BY public.kontakt.id_kontakta;


--
-- Name: lokacija; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.lokacija (
    id_lokacije integer NOT NULL,
    objekt_id integer NOT NULL,
    ulica character varying(100) NOT NULL,
    kucni_broj character varying(10),
    grad character varying(50) NOT NULL,
    latitude numeric(10,8),
    longitude numeric(11,8),
    radno_vrijeme character varying(50)
);


ALTER TABLE public.lokacija OWNER TO postgres;

--
-- Name: lokacija_id_lokacije_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.lokacija_id_lokacije_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.lokacija_id_lokacije_seq OWNER TO postgres;

--
-- Name: lokacija_id_lokacije_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.lokacija_id_lokacije_seq OWNED BY public.lokacija.id_lokacije;


--
-- Name: objekt; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.objekt (
    id_objekta integer NOT NULL,
    tip_objekta_id integer NOT NULL,
    kontakt_id integer NOT NULL,
    naziv character varying(100) NOT NULL,
    opis text,
    prosjecna_ocjena numeric(3,2),
    cjenovni_rang character varying(3),
    vlasnik character varying(100),
    dostupna_dostava boolean DEFAULT false,
    datum_unosa timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT objekt_cjenovni_rang_check CHECK (((cjenovni_rang)::text = ANY ((ARRAY['$'::character varying, '$$'::character varying, '$$$'::character varying])::text[]))),
    CONSTRAINT objekt_prosjecna_ocjena_check CHECK (((prosjecna_ocjena >= (0)::numeric) AND (prosjecna_ocjena <= (5)::numeric)))
);


ALTER TABLE public.objekt OWNER TO postgres;

--
-- Name: objekt_id_objekta_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.objekt_id_objekta_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.objekt_id_objekta_seq OWNER TO postgres;

--
-- Name: objekt_id_objekta_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.objekt_id_objekta_seq OWNED BY public.objekt.id_objekta;


--
-- Name: objekt_tag; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.objekt_tag (
    objekt_id integer NOT NULL,
    tag_id integer NOT NULL,
    datum_dodavanja timestamp without time zone DEFAULT CURRENT_TIMESTAMP
);


ALTER TABLE public.objekt_tag OWNER TO postgres;

--
-- Name: tag; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tag (
    id_taga integer NOT NULL,
    naziv character varying(50) NOT NULL,
    opis text
);


ALTER TABLE public.tag OWNER TO postgres;

--
-- Name: tag_id_taga_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.tag_id_taga_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.tag_id_taga_seq OWNER TO postgres;

--
-- Name: tag_id_taga_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.tag_id_taga_seq OWNED BY public.tag.id_taga;


--
-- Name: tip_objekta; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tip_objekta (
    id_tipa integer NOT NULL,
    opis character varying(50) NOT NULL
);


ALTER TABLE public.tip_objekta OWNER TO postgres;

--
-- Name: tip_objekta_id_tipa_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.tip_objekta_id_tipa_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.tip_objekta_id_tipa_seq OWNER TO postgres;

--
-- Name: tip_objekta_id_tipa_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.tip_objekta_id_tipa_seq OWNED BY public.tip_objekta.id_tipa;


--
-- Name: kontakt id_kontakta; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.kontakt ALTER COLUMN id_kontakta SET DEFAULT nextval('public.kontakt_id_kontakta_seq'::regclass);


--
-- Name: lokacija id_lokacije; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.lokacija ALTER COLUMN id_lokacije SET DEFAULT nextval('public.lokacija_id_lokacije_seq'::regclass);


--
-- Name: objekt id_objekta; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.objekt ALTER COLUMN id_objekta SET DEFAULT nextval('public.objekt_id_objekta_seq'::regclass);


--
-- Name: tag id_taga; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tag ALTER COLUMN id_taga SET DEFAULT nextval('public.tag_id_taga_seq'::regclass);


--
-- Name: tip_objekta id_tipa; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tip_objekta ALTER COLUMN id_tipa SET DEFAULT nextval('public.tip_objekta_id_tipa_seq'::regclass);


--
-- Data for Name: kontakt; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.kontakt (id_kontakta, web_stranica, kontakt_broj, email) FROM stdin;
1	https://foodheritage.hr/heritage/	\N	croatianfoodheritage@gmail.com
2	https://www.lastruk.com	+38514837701	\N
3	https://batak.hr	+38516471111	\N
4	https://www.kiyomi.hr	+38516474897	reservations@kiyomi.hr
5	https://zerozero.com.hr/about-us/	+385995266444	\N
6	https://www.facebook.com/bacchusjazzbar	+38598322804	\N
7	https://www.facebook.com/pages/Krivi-Put/134656319955740	+38514837701	\N
8	https://www.facebook.com/BotanicarZagreb/	\N	botanicar.zg@gmail.com
9	https://www.facebook.com/BarMrFogg	+38516272650	mr.fogg.zagreb@gmail.com
10	https://www.instagram.com/program_bar/	+385989938279	program.programbar@gmail.com
\.


--
-- Data for Name: lokacija; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.lokacija (id_lokacije, objekt_id, ulica, kucni_broj, grad, latitude, longitude, radno_vrijeme) FROM stdin;
1	1	Petrinjska ulica	14	Zagreb	45.81080600	15.97969600	12:15–20:00
2	2	Skalinska ulica	5	Zagreb	45.81493600	15.97685800	11:00–22:00
3	3	Avenija Većeslava Holjevca	62	Zagreb	45.77769852	15.97968674	11:00–23:00
4	3	Ulica Ivana Tkaličića	70	Zagreb	45.81752000	15.97626000	11:00–23:00
5	3	Ulica Jakova Gotovca	1	Zagreb	45.81519300	15.99443300	11:00–23:00
6	3	Gajeva Ulica	10	Zagreb	45.81090700	15.97638400	12:00–00:00
7	4	Gajeva Ulica	10	Zagreb	45.81090710	15.97638390	12:00–00:00
8	5	Vlaška ulica	35	Zagreb	45.81398000	15.98198100	11:30–23:00
9	6	Trg Kralja Tomislava	16	Zagreb	45.80642300	15.97769400	09:00–00:00
10	7	Savska cesta	14	Zagreb	45.80477600	15.96555100	08:00–00:00
11	8	Trg Marka Marulića	6	Zagreb	45.80627400	15.97139700	08:00–23:00
12	9	Martićeva ulica	31	Zagreb	45.81243100	15.99149200	07:00–00:00
13	10	Martićeva ulica	14F	Zagreb	45.81207000	15.98930000	07:00–23:00
14	10	Petrova ulica	21	Zagreb	45.81690300	15.99548900	08:00–23:00
\.


--
-- Data for Name: objekt; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.objekt (id_objekta, tip_objekta_id, kontakt_id, naziv, opis, prosjecna_ocjena, cjenovni_rang, vlasnik, dostupna_dostava, datum_unosa) FROM stdin;
1	3	1	Heritage	Heritage is a tiny snack bar.	4.80	$	Silvije Polanšćak	f	2025-10-23 15:05:13.12959
2	2	2	La Štruk	Serving only strukli, traditional Croatian specialty from Zagreb area.	4.60	$	Alen Markulin	f	2025-10-23 15:20:22.418532
3	2	3	Batak	Najpoznatiji smo po sočnim i ukusnim jelima s roštilja poput nezaobilaznih mazalica, sočnih ćevapa, gurmanske pljeskavice, ili punjene vješalice.	4.60	$$	Boris Stojić	t	2025-10-23 15:28:57.038325
4	2	4	Kiyomi	KIYOMI is an Asian restaurant passionate about creating delicious and unique dishes that blend traditional Dim Sum, Robata, and Sushi flavors.	4.80	$$	Vid Nikolić	f	2025-10-23 15:41:26.470459
5	4	5	Pizzeria Zero Zero	At this place guests can delve deep into delicious dishes, and order good seafood pizza, tuna and meat pizza.	4.50	$$	Siniša Matijević	t	2025-10-23 15:50:34.271809
6	1	6	Bacchus Jazz Bar	Laidback jazz sessions, beer & coffee at a cellar bar with a courtyard sheltered by fig trees.	4.70	$	Duje Oman	f	2025-10-23 16:02:43.649361
7	1	7	Krivi Put	Krivi Put is the place to go to when you feel like avoiding responsibilities.	4.80	$	Nenad Barbić	f	2025-10-23 16:11:21.053375
8	1	8	Botaničar	Botaničar, a budding flower of creativity on Zagreb’s urban map. Bar with an original botanical twist. Home of arts and crafts, nurturing those little pleasures of life.	4.60	$	Marko Krneta	f	2025-10-23 16:16:29.797888
9	1	9	Mr. Fogg	The first steampunk bar in Zagreb.	4.70	$	Emil Tocauer	f	2025-10-23 16:23:56.099344
10	1	10	Program Bar	This cafe provides you with nice food and a place to rest after a long walk.	4.80	$	Saša Žerajić	f	2025-10-23 16:28:27.104604
\.


--
-- Data for Name: objekt_tag; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.objekt_tag (objekt_id, tag_id, datum_dodavanja) FROM stdin;
1	9	2025-10-23 15:14:15.120209
1	3	2025-10-23 15:14:15.120209
2	1	2025-10-23 15:23:23.686538
2	3	2025-10-23 15:23:23.686538
2	5	2025-10-23 15:23:23.686538
2	9	2025-10-23 15:23:23.686538
3	1	2025-10-23 15:33:45.8792
3	2	2025-10-23 15:33:45.8792
3	3	2025-10-23 15:33:45.8792
3	9	2025-10-23 15:33:45.8792
4	1	2025-10-23 15:42:26.638864
5	1	2025-10-23 15:51:23.494866
5	2	2025-10-23 15:51:23.494866
5	3	2025-10-23 15:51:23.494866
5	5	2025-10-23 15:51:23.494866
5	9	2025-10-23 15:51:23.494866
4	11	2025-10-23 15:53:50.296195
4	9	2025-10-23 15:53:50.296195
6	3	2025-10-23 16:04:22.77607
6	9	2025-10-23 16:04:22.77607
7	3	2025-10-23 16:12:46.885218
7	9	2025-10-23 16:12:46.885218
8	1	2025-10-23 16:17:16.076442
8	2	2025-10-23 16:17:16.076442
8	3	2025-10-23 16:17:16.076442
8	9	2025-10-23 16:17:16.076442
9	3	2025-10-23 16:24:22.662637
9	9	2025-10-23 16:24:22.662637
10	3	2025-10-23 16:29:40.739372
10	9	2025-10-23 16:29:40.739372
10	7	2025-10-23 16:29:40.739372
\.


--
-- Data for Name: tag; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.tag (id_taga, naziv, opis) FROM stdin;
1	WiFi	Besplatan bežični internet za goste
2	Parking	Dostupan parking prostor
3	Terasa	Vanjski prostor za sjedenje
4	Vegan	Nudi veganske opcije jela i pića
5	Vegetarijanski	Nudi vegetarijanske opcije
6	Pet-friendly	Dopušten pristup s kućnim ljubimcima
7	Klimatizacija	Klimatizirani unutarnji prostor
8	Live glazba	Povremeni ili redoviti glazbeni nastupi
9	Plaćanje karticom	Prihvaća plaćanje debitnim/kreditnim karticama
11	Japanska hrana	Restoran nudi japansku hranu poput sushi-a
\.


--
-- Data for Name: tip_objekta; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.tip_objekta (id_tipa, opis) FROM stdin;
1	kafić
2	restoran
3	snack bar
4	pizzeria
5	slastičarna
\.


--
-- Name: kontakt_id_kontakta_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.kontakt_id_kontakta_seq', 10, true);


--
-- Name: lokacija_id_lokacije_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.lokacija_id_lokacije_seq', 14, true);


--
-- Name: objekt_id_objekta_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.objekt_id_objekta_seq', 10, true);


--
-- Name: tag_id_taga_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.tag_id_taga_seq', 11, true);


--
-- Name: tip_objekta_id_tipa_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.tip_objekta_id_tipa_seq', 5, true);


--
-- Name: kontakt kontakt_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.kontakt
    ADD CONSTRAINT kontakt_pkey PRIMARY KEY (id_kontakta);


--
-- Name: lokacija lokacija_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.lokacija
    ADD CONSTRAINT lokacija_pkey PRIMARY KEY (id_lokacije);


--
-- Name: objekt objekt_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.objekt
    ADD CONSTRAINT objekt_pkey PRIMARY KEY (id_objekta);


--
-- Name: objekt_tag objekt_tag_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.objekt_tag
    ADD CONSTRAINT objekt_tag_pkey PRIMARY KEY (objekt_id, tag_id);


--
-- Name: tag tag_naziv_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tag
    ADD CONSTRAINT tag_naziv_key UNIQUE (naziv);


--
-- Name: tag tag_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tag
    ADD CONSTRAINT tag_pkey PRIMARY KEY (id_taga);


--
-- Name: tip_objekta tip_objekta_opis_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tip_objekta
    ADD CONSTRAINT tip_objekta_opis_key UNIQUE (opis);


--
-- Name: tip_objekta tip_objekta_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tip_objekta
    ADD CONSTRAINT tip_objekta_pkey PRIMARY KEY (id_tipa);


--
-- Name: lokacija lokacija_id_objekta_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.lokacija
    ADD CONSTRAINT lokacija_id_objekta_fkey FOREIGN KEY (objekt_id) REFERENCES public.objekt(id_objekta) ON DELETE CASCADE;


--
-- Name: objekt objekt_kontakt_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.objekt
    ADD CONSTRAINT objekt_kontakt_id_fkey FOREIGN KEY (kontakt_id) REFERENCES public.kontakt(id_kontakta) ON DELETE CASCADE;


--
-- Name: objekt_tag objekt_tag_id_objekta_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.objekt_tag
    ADD CONSTRAINT objekt_tag_id_objekta_fkey FOREIGN KEY (objekt_id) REFERENCES public.objekt(id_objekta) ON DELETE CASCADE;


--
-- Name: objekt_tag objekt_tag_id_taga_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.objekt_tag
    ADD CONSTRAINT objekt_tag_id_taga_fkey FOREIGN KEY (tag_id) REFERENCES public.tag(id_taga) ON DELETE CASCADE;


--
-- Name: objekt objekt_tip_objekta_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.objekt
    ADD CONSTRAINT objekt_tip_objekta_id_fkey FOREIGN KEY (tip_objekta_id) REFERENCES public.tip_objekta(id_tipa) ON DELETE CASCADE;


--
-- PostgreSQL database dump complete
--

\unrestrict fkbgQWQDWaewQpLniye8yUb5JHAt25q9j5XeLZboZNMBMsCiorn4Ea323mCBjv1

