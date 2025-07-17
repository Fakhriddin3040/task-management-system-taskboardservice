--
-- PostgreSQL database dump
--

-- Dumped from database version 17.5 (Homebrew)
-- Dumped by pg_dump version 17.5 (Homebrew)

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
-- Name: __EFMigrationsHistory; Type: TABLE; Schema: public; Owner: tms_board
--

CREATE TABLE public."__EFMigrationsHistory" (
    migration_id character varying(150) NOT NULL,
    product_version character varying(32) NOT NULL
);


ALTER TABLE public."__EFMigrationsHistory" OWNER TO tms_board;

--
-- Name: task_board_columns; Type: TABLE; Schema: public; Owner: tms_board
--

CREATE TABLE public.task_board_columns (
    id uuid NOT NULL,
    name character varying(100) NOT NULL,
    "order" integer NOT NULL,
    board_id uuid NOT NULL,
    created_by_id uuid NOT NULL,
    updated_by_id uuid NOT NULL,
    created_at timestamp with time zone NOT NULL,
    updated_at timestamp with time zone NOT NULL,
    board_id1 uuid NOT NULL
);


ALTER TABLE public.task_board_columns OWNER TO tms_board;

--
-- Name: task_boards; Type: TABLE; Schema: public; Owner: tms_board
--

CREATE TABLE public.task_boards (
    id uuid NOT NULL,
    name character varying(100) NOT NULL,
    description text,
    organization_id uuid NOT NULL,
    created_by_id uuid NOT NULL,
    updated_by_id uuid NOT NULL,
    created_at timestamp with time zone NOT NULL,
    updated_at timestamp with time zone NOT NULL
);


ALTER TABLE public.task_boards OWNER TO tms_board;

--
-- Data for Name: __EFMigrationsHistory; Type: TABLE DATA; Schema: public; Owner: tms_board
--

COPY public."__EFMigrationsHistory" (migration_id, product_version) FROM stdin;
20250715172959_Init>	9.0.7
\.


--
-- Data for Name: task_board_columns; Type: TABLE DATA; Schema: public; Owner: tms_board
--

COPY public.task_board_columns (id, name, "order", board_id, created_by_id, updated_by_id, created_at, updated_at, board_id1) FROM stdin;
019819e2-8a4c-77ad-8245-4b5d92c5ab1a	Niga siya	1	32c88622-d7fe-474e-a416-575c75b761cf	42174610-1430-448c-9913-fbc84962728f	42174610-1430-448c-9913-fbc84962728f	2025-07-18 00:35:33.706196+05	2025-07-18 00:35:33.706196+05	32c88622-d7fe-474e-a416-575c75b761cf
019819e2-9e7e-7025-bcd0-29d251d96db5	Niga siya2	2	32c88622-d7fe-474e-a416-575c75b761cf	42174610-1430-448c-9913-fbc84962728f	42174610-1430-448c-9913-fbc84962728f	2025-07-18 00:35:38.876407+05	2025-07-18 00:35:38.876407+05	32c88622-d7fe-474e-a416-575c75b761cf
019819e2-aa48-79fb-885e-b2adbeea4b19	Niga siya3	3	32c88622-d7fe-474e-a416-575c75b761cf	42174610-1430-448c-9913-fbc84962728f	42174610-1430-448c-9913-fbc84962728f	2025-07-18 00:35:41.896503+05	2025-07-18 00:35:41.896503+05	32c88622-d7fe-474e-a416-575c75b761cf
019819e2-b680-776f-9c39-2ebb7b74e14b	Niga siya4	4	32c88622-d7fe-474e-a416-575c75b761cf	42174610-1430-448c-9913-fbc84962728f	42174610-1430-448c-9913-fbc84962728f	2025-07-18 00:35:45.023821+05	2025-07-18 00:35:45.023822+05	32c88622-d7fe-474e-a416-575c75b761cf
019819e2-c129-7e32-9d63-c2568d1389c6	Niga siya5	5	32c88622-d7fe-474e-a416-575c75b761cf	42174610-1430-448c-9913-fbc84962728f	42174610-1430-448c-9913-fbc84962728f	2025-07-18 00:35:47.753325+05	2025-07-18 00:35:47.753325+05	32c88622-d7fe-474e-a416-575c75b761cf
\.


--
-- Data for Name: task_boards; Type: TABLE DATA; Schema: public; Owner: tms_board
--

COPY public.task_boards (id, name, description, organization_id, created_by_id, updated_by_id, created_at, updated_at) FROM stdin;
32c88622-d7fe-474e-a416-575c75b761cf	Amirjan Niger		0f2178e4-4e24-46ee-a06c-902e65278415	42174610-1430-448c-9913-fbc84962728f	42174610-1430-448c-9913-fbc84962728f	2025-07-18 00:35:20.53212+05	2025-07-18 00:35:20.532132+05
\.


--
-- Name: __EFMigrationsHistory pk___ef_migrations_history; Type: CONSTRAINT; Schema: public; Owner: tms_board
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT pk___ef_migrations_history PRIMARY KEY (migration_id);


--
-- Name: task_board_columns pk_task_board_columns; Type: CONSTRAINT; Schema: public; Owner: tms_board
--

ALTER TABLE ONLY public.task_board_columns
    ADD CONSTRAINT pk_task_board_columns PRIMARY KEY (id);


--
-- Name: task_boards pk_task_boards; Type: CONSTRAINT; Schema: public; Owner: tms_board
--

ALTER TABLE ONLY public.task_boards
    ADD CONSTRAINT pk_task_boards PRIMARY KEY (id);


--
-- Name: ix_task_board_columns_board_id; Type: INDEX; Schema: public; Owner: tms_board
--

CREATE INDEX ix_task_board_columns_board_id ON public.task_board_columns USING btree (board_id1);


--
-- Name: ix_task_board_columns_board_id_order; Type: INDEX; Schema: public; Owner: tms_board
--

CREATE UNIQUE INDEX ix_task_board_columns_board_id_order ON public.task_board_columns USING btree (board_id, "order");


--
-- Name: ix_task_board_columns_name_board_id; Type: INDEX; Schema: public; Owner: tms_board
--

CREATE UNIQUE INDEX ix_task_board_columns_name_board_id ON public.task_board_columns USING btree (name, board_id);


--
-- Name: ix_task_boards_organization_id; Type: INDEX; Schema: public; Owner: tms_board
--

CREATE INDEX ix_task_boards_organization_id ON public.task_boards USING btree (organization_id);


--
-- Name: ix_task_boards_organization_id_name; Type: INDEX; Schema: public; Owner: tms_board
--

CREATE UNIQUE INDEX ix_task_boards_organization_id_name ON public.task_boards USING btree (organization_id, name);


--
-- Name: task_board_columns fk_task_board_columns_task_boards_board_id; Type: FK CONSTRAINT; Schema: public; Owner: tms_board
--

ALTER TABLE ONLY public.task_board_columns
    ADD CONSTRAINT fk_task_board_columns_task_boards_board_id FOREIGN KEY (board_id1) REFERENCES public.task_boards(id) ON DELETE CASCADE;


--
-- PostgreSQL database dump complete
--

