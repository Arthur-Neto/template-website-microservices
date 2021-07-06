SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

CREATE SCHEMA "TemplateMaster";
CREATE SCHEMA "Template_TESTE1";
CREATE SCHEMA "Template_TESTE2";

CREATE TABLE "TemplateMaster"."Enterprise" (
    "ID" uuid DEFAULT public.uuid_generate_v4() NOT NULL,
    "EnterpriseName" character varying(100) NOT NULL,
    "ConnectionString" character varying(100) NOT NULL
);

CREATE TABLE "TemplateMaster"."Tenant" (
    "ID" uuid DEFAULT public.uuid_generate_v4() NOT NULL,
    "Logon" character varying(100) NOT NULL,
    "Password" text NOT NULL,
    "EnterpriseID" uuid,
    "Role" smallint NOT NULL,
    "Salt" text NOT NULL
);

CREATE TABLE "Template_TESTE1"."User" (
    "ID" uuid DEFAULT public.uuid_generate_v4() NOT NULL,
    "Name" character varying(100) NOT NULL
);

CREATE TABLE "Template_TESTE2"."User" (
    "ID" uuid DEFAULT public.uuid_generate_v4() NOT NULL,
    "Name" character varying(100) NOT NULL
);

COPY "TemplateMaster"."Enterprise" ("ID", "EnterpriseName", "ConnectionString") FROM stdin;
123a390f-5362-445d-8d03-d2b234095cb6	Teste1	TESTE1
82564bde-01e3-4b23-bc69-5e4a4e114461	Teste2	TESTE2
\.

COPY "TemplateMaster"."Tenant" ("ID", "Logon", "Password", "EnterpriseID", "Role", "Salt") FROM stdin;
61927f43-395a-4c10-9dd6-978855f7f8bf	admin@teste2.com	xaCtUmFurH/xkS3fiU1Rk6BiUwbNZ4TT	82564bde-01e3-4b23-bc69-5e4a4e114461	1	EAYSQzDsbVneD7wOYZKobw==
eb66224d-aeb5-4f93-9286-24bb57ba2b91	admin@teste1.com	2LpnlZKnSN/8MlxjyU0aZ4PTHG4evX6t	123a390f-5362-445d-8d03-d2b234095cb6	1	QRpngBiXOEYWRJQBUQSEgw==
\.

COPY "Template_TESTE1"."User" ("ID", "Name") FROM stdin;
eb66224d-aeb5-4f93-9286-24bb57ba2b91	Administrator
\.

COPY "Template_TESTE2"."User" ("ID", "Name") FROM stdin;
61927f43-395a-4c10-9dd6-978855f7f8bf	Administrador 2
\.

ALTER TABLE ONLY "TemplateMaster"."Enterprise"
    ADD CONSTRAINT "PK_Enterprise" PRIMARY KEY ("ID");

ALTER TABLE ONLY "TemplateMaster"."Tenant"
    ADD CONSTRAINT "PK_Tenant" PRIMARY KEY ("ID");

ALTER TABLE ONLY "TemplateMaster"."Enterprise"
    ADD CONSTRAINT "Unique_EnterpriseName" UNIQUE ("EnterpriseName");

ALTER TABLE ONLY "TemplateMaster"."Tenant"
    ADD CONSTRAINT "Unique_Logon" UNIQUE ("Logon");

ALTER TABLE ONLY "Template_TESTE1"."User"
    ADD CONSTRAINT "PK_User" PRIMARY KEY ("ID");

ALTER TABLE ONLY "Template_TESTE2"."User"
    ADD CONSTRAINT "PK_User" PRIMARY KEY ("ID");

ALTER TABLE ONLY "TemplateMaster"."Tenant"
    ADD CONSTRAINT "FK_Tenant_Enterprise" FOREIGN KEY ("EnterpriseID") REFERENCES "TemplateMaster"."Enterprise"("ID");
