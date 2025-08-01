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
    "order" bigint NOT NULL,
    board_id uuid NOT NULL,
    created_by_id uuid NOT NULL,
    updated_by_id uuid NOT NULL,
    created_at bigint NOT NULL,
    updated_at bigint NOT NULL
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
    created_at bigint NOT NULL,
    updated_at bigint NOT NULL
);


ALTER TABLE public.task_boards OWNER TO tms_board;

--
-- Data for Name: __EFMigrationsHistory; Type: TABLE DATA; Schema: public; Owner: tms_board
--

COPY public."__EFMigrationsHistory" (migration_id, product_version) FROM stdin;
20250721193440_Init>	9.0.7
20250731190505_Update...	9.0.7
\.


--
-- Data for Name: task_board_columns; Type: TABLE DATA; Schema: public; Owner: tms_board
--

COPY public.task_board_columns (id, name, "order", board_id, created_by_id, updated_by_id, created_at, updated_at) FROM stdin;
0198621d-1a51-729d-9dc2-1142a2967474	Niga One	1000000000000	5353e1a8-fb82-499f-aa08-550892a34389	573d0e42-74ab-40d5-b97d-6266aca21513	573d0e42-74ab-40d5-b97d-6266aca21513	1753992731214	1753992731214
0198621d-2769-7123-8f86-eb04bef0c057	Niga two	2000000000000	5353e1a8-fb82-499f-aa08-550892a34389	573d0e42-74ab-40d5-b97d-6266aca21513	573d0e42-74ab-40d5-b97d-6266aca21513	1753992734568	1753992734568
0198621d-3a9d-74eb-8f19-eefb25c6a839	Niga three	3000000000000	5353e1a8-fb82-499f-aa08-550892a34389	573d0e42-74ab-40d5-b97d-6266aca21513	573d0e42-74ab-40d5-b97d-6266aca21513	1753992739485	1753992739485
0198621d-42fa-777a-9487-2fd0fa78d9cb	Niga four	4000000000000	5353e1a8-fb82-499f-aa08-550892a34389	573d0e42-74ab-40d5-b97d-6266aca21513	573d0e42-74ab-40d5-b97d-6266aca21513	1753992741626	1753992741626
0198621d-4f89-7b55-99aa-f55775bc1f45	Niga five and sex	5000000000000	5353e1a8-fb82-499f-aa08-550892a34389	573d0e42-74ab-40d5-b97d-6266aca21513	573d0e42-74ab-40d5-b97d-6266aca21513	1753992744841	1753992744841
\.


--
-- Data for Name: task_boards; Type: TABLE DATA; Schema: public; Owner: tms_board
--

COPY public.task_boards (id, name, description, organization_id, created_by_id, updated_by_id, created_at, updated_at) FROM stdin;
5353e1a8-fb82-499f-aa08-550892a34389	Niga Th eOne	nsit	3dcd0c8e-862d-48a7-922f-5a98d6ce4de8	573d0e42-74ab-40d5-b97d-6266aca21513	573d0e42-74ab-40d5-b97d-6266aca21513	1753992722366	1753992722366
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

CREATE INDEX ix_task_board_columns_board_id ON public.task_board_columns USING btree (board_id);


--
-- Name: ix_task_board_columns_name_board_id; Type: INDEX; Schema: public; Owner: tms_board
--

CREATE UNIQUE INDEX ix_task_board_columns_name_board_id ON public.task_board_columns USING btree (name, board_id);


--
-- Name: ix_task_board_columns_order_board_id; Type: INDEX; Schema: public; Owner: tms_board
--

CREATE UNIQUE INDEX ix_task_board_columns_order_board_id ON public.task_board_columns USING btree ("order", board_id);


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
    ADD CONSTRAINT fk_task_board_columns_task_boards_board_id FOREIGN KEY (board_id) REFERENCES public.task_boards(id) ON DELETE CASCADE;


--
-- PostgreSQL database dump complete
--

